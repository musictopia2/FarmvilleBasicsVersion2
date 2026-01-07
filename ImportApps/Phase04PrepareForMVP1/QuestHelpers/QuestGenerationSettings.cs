namespace Phase04PrepareForMVP1.QuestHelpers;
public sealed class QuestGenerationSettings
{
    public int TotalQuests { get; init; }

    public Weights Weighting { get; init; } = new();
    public Caps Limits { get; init; } = new();
    public DuplicatePolicy Duplicates { get; init; } = new();

    public int MaxAttemptsPerQuest { get; init; } = 25;

    public sealed class Weights
    {
        public int Basics { get; init; } = 60;
        public int Workshops { get; init; } = 30;
        public int Worksites { get; init; } = 10;
    }

    public sealed class Caps
    {
        public int MaxWorksiteQuests { get; init; } = 2;
        public int MaxRareWorksiteQuests { get; init; } = 1;

        public int MaxMultiStepWorkshopQuests { get; init; } = 2;     // WorkshopInputs > 0
        public int MaxRareInputWorkshopQuests { get; init; } = 1;     // WorstWorksiteInputRarity == Rare

        public int MaxSameBasicItemQuests { get; init; } = 3;         // optional hard cap

        // NEW: optional hard cap for repeating the same workshop output item
        public int MaxSameWorkshopItemQuests { get; init; } = 2; // 0 = no cap

    }

    public sealed class DuplicatePolicy
    {
        public double BasicRepeatWeight1 { get; init; } = 0.55;       // second time
        public double BasicRepeatWeight2Plus { get; init; } = 0.25;   // third+ times

        // NEW: soft repeat shaping for workshops
        public double WorkshopRepeatWeight1 { get; init; } = 0.65;
        public double WorkshopRepeatWeight2Plus { get; init; } = 0.35;

    }
    // NEW: choose whether to allow repeats at all
    public bool AllowWorkshopRepeatsInPack { get; init; } = true;

    // Optional: if true, repeats are tracked by OutputItem (Biscuits) not by WorkshopName
    public bool WorkshopRepeatKeyIsOutputItem { get; init; } = true;
}