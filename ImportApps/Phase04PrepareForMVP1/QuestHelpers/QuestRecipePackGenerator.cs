using System;
using System.Collections.Generic;
using System.Text;

namespace Phase04PrepareForMVP1.QuestHelpers;

internal sealed class QuestPackGenerator
{
    private readonly IRandomNumberList _rng;

    public QuestPackGenerator()
    {
        _rng = rs1.GetRandomGenerator();
    }
    //public QuestPackGenerator(IRandomNumberList rng) => _rng = rng;

    public BasicList<QuestRecipe> Generate(QuestContainer container, QuestGenerationSettings settings)
    {
        QuestPackState state = new();
        BasicList<QuestRecipe> output = [];

        // Pools. Remove from workshop/worksite pools to reduce repeats.
        List<BasicItem> basicPool = container.Basics.ToList();
        List<WorkshopOutputAnalysis> workshopPool = container.Workshops.ToList();
        List<WorksiteOutputAnalysis> worksitePool = container.Worksites.ToList();

        for (int i = 0; i < settings.TotalQuests; i++)
        {
            var pick = PickOne(basicPool, workshopPool, worksitePool, state, settings);
            if (pick == null) break;

            output.Add(pick.Value.recipe);

            // Apply state + pool depletion
            switch (pick.Value.type)
            {
                case PickType.Basic:
                    state.ApplyBasic(pick.Value.basic!);
                    break;

                case PickType.Workshop:
                    state.ApplyWorkshop(pick.Value.workshop!, settings);

                    if (settings.AllowWorkshopRepeatsInPack == false)
                    {
                        workshopPool.Remove(pick.Value.workshop!);
                    }

                    break;

                case PickType.Worksite:
                    state.ApplyWorksite(pick.Value.worksite!);
                    worksitePool.Remove(pick.Value.worksite!);
                    break;
            }
        }

        return output;
    }

    private enum PickType { Basic, Workshop, Worksite }

    private (QuestRecipe recipe, PickType type, BasicItem? basic, WorkshopOutputAnalysis? workshop, WorksiteOutputAnalysis? worksite)?
        PickOne(
            List<BasicItem> basics,
            List<WorkshopOutputAnalysis> workshops,
            List<WorksiteOutputAnalysis> worksites,
            QuestPackState state,
            QuestGenerationSettings s)
    {
        for (int attempt = 0; attempt < s.MaxAttemptsPerQuest; attempt++)
        {
            var eligible = BuildEligibleTypeWeights(basics, workshops, worksites, state, s);
            if (eligible.Count == 0) return null;

            var chosenType = WeightedPickType(eligible);

            if (chosenType == PickType.Basic)
            {
                var b = PickBasic(basics, state, s);
                if (b == null) continue;

                int required = PickRequirementForBasic(b);

                return (new QuestRecipe
                {
                    Item = b.Item,
                    Required = required,
                    Completed = false
                }, PickType.Basic, b, null, null);
            }

            if (chosenType == PickType.Workshop)
            {
                var w = PickWorkshop(workshops, state, s);
                if (w == null) continue;

                int required = RequirementPicker.ForWorkshop(w);

                return (new QuestRecipe
                {
                    Item = w.OutputItem,
                    Required = required,
                    Completed = false
                }, PickType.Workshop, null, w, null);
            }

            // Worksite
            var ws = PickWorksite(worksites, state, s);
            if (ws == null) continue;

            int required2 = WorksiteRequirementPicker.PickRequiredAmount(ws);

            return (new QuestRecipe
            {
                Item = ws.Item,
                Required = required2,
                Completed = false
            }, PickType.Worksite, null, null, ws);
        }

        return FirstFit(basics, workshops, worksites, state, s);
    }

    private List<(PickType type, int weight)> BuildEligibleTypeWeights(
    List<BasicItem> basics,
    List<WorkshopOutputAnalysis> workshops,
    List<WorksiteOutputAnalysis> worksites,
    QuestPackState state,
    QuestGenerationSettings s)
    {
        var list = new List<(PickType type, int weight)>();

        if (basics.Any(b => state.CanTakeBasic(b, s)))
            list.Add((PickType.Basic, s.Weighting.Basics));

        if (workshops.Any(w => state.CanTakeWorkshop(w, s)))
            list.Add((PickType.Workshop, s.Weighting.Workshops));

        if (worksites.Any(ws => state.CanTakeWorksite(ws, s)))
            list.Add((PickType.Worksite, s.Weighting.Worksites));

        list.RemoveAll(x => x.weight <= 0);
        return list;
    }

    private PickType WeightedPickType(List<(PickType type, int weight)> weights)
    {
        int total = weights.Sum(x => x.weight);
        int roll = _rng.GetRandomNumber(total, 1);

        int acc = 0;
        foreach (var (t, w) in weights)
        {
            acc += w;
            if (roll <= acc) return t;
        }
        return weights[0].type;
    }

    private BasicItem? PickBasic(List<BasicItem> pool, QuestPackState state, QuestGenerationSettings s)
    {
        List<(BasicItem item, int weight)> weighted = new();

        foreach (var b in pool)
        {
            if (!state.CanTakeBasic(b, s)) continue;

            int used = state.TimesUsedBasic(b.Item);
            double mult = used switch
            {
                0 => 1.0,
                1 => s.Duplicates.BasicRepeatWeight1,
                _ => s.Duplicates.BasicRepeatWeight2Plus
            };

            int w = (int)Math.Max(1, Math.Round(100 * mult));
            weighted.Add((b, w));
        }

        return WeightedPick(weighted);
    }

    private WorkshopOutputAnalysis? PickWorkshop(List<WorkshopOutputAnalysis> pool, QuestPackState state, QuestGenerationSettings s)
    {
        List<(WorkshopOutputAnalysis item, int weight)> weighted = new();

        foreach (var w in pool)
        {
            if (!state.CanTakeWorkshop(w, s)) continue;

            // soft penalties before caps
            int weight = 100;
            if (w.WorkshopInputs > 0) weight = (int)(weight * 0.45);
            if (w.WorstWorksiteInputRarity == EnumWorksiteRarity.Rare) weight = (int)(weight * 0.45);

            weighted.Add((w, Math.Max(1, weight)));
        }

        return WeightedPick(weighted);
    }

    private WorksiteOutputAnalysis? PickWorksite(List<WorksiteOutputAnalysis> pool, QuestPackState state, QuestGenerationSettings s)
    {
        List<(WorksiteOutputAnalysis item, int weight)> weighted = new();

        foreach (var ws in pool)
        {
            if (!state.CanTakeWorksite(ws, s)) continue;

            // soft: rare less likely even before cap
            int weight = ws.Rarity == EnumWorksiteRarity.Rare ? 25 : 100;
            weighted.Add((ws, weight));
        }

        return WeightedPick(weighted);
    }

    private T? WeightedPick<T>(List<(T item, int weight)> weighted) where T : class
    {
        if (weighted.Count == 0) return null;

        int total = weighted.Sum(x => x.weight);
        int roll = _rng.GetRandomNumber(total, 1);

        int acc = 0;
        foreach (var (item, w) in weighted)
        {
            acc += w;
            if (roll <= acc) return item;
        }

        return weighted[0].item;
    }

    private static int PickRequirementForBasic(BasicItem item)
    {
        return item.Source switch
        {
            EnumItemSource.Crop => RequirementPicker.ForCrop(item.BaseTime),
            EnumItemSource.Tree => RequirementPicker.ForTree(item.BaseTime, item.Amount),
            EnumItemSource.Animal => RequirementPicker.ForAnimal(item.BaseTime, item.Amount),
            _ => throw new CustomBasicException("BasicItem.Source must be Crop/Tree/Animal")
        };
    }

    private (QuestRecipe recipe, PickType type, BasicItem? basic, WorkshopOutputAnalysis? workshop, WorksiteOutputAnalysis? worksite)?
        FirstFit(
            List<BasicItem> basics,
            List<WorkshopOutputAnalysis> workshops,
            List<WorksiteOutputAnalysis> worksites,
            QuestPackState state,
            QuestGenerationSettings s)
    {
        foreach (var w in workshops)
        {
            if (!state.CanTakeWorkshop(w, s)) continue;

            return (new QuestRecipe
            {
                Item = w.OutputItem,
                Required = RequirementPicker.ForWorkshop(w),
                Completed = false
            }, PickType.Workshop, null, w, null);
        }

        foreach (var b in basics)
        {
            if (!state.CanTakeBasic(b, s)) continue;

            return (new QuestRecipe
            {
                Item = b.Item,
                Required = PickRequirementForBasic(b),
                Completed = false
            }, PickType.Basic, b, null, null);
        }

        foreach (var ws in worksites)
        {
            if (!state.CanTakeWorksite(ws, s)) continue;

            return (new QuestRecipe
            {
                Item = ws.Item,
                Required = WorksiteRequirementPicker.PickRequiredAmount(ws),
                Completed = false
            }, PickType.Worksite, null, null, ws);
        }

        return null;
    }
}