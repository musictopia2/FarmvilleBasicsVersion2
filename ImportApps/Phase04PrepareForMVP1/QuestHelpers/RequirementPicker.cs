namespace Phase04PrepareForMVP1.QuestHelpers;
public static class RequirementPicker
{
    private readonly static IRandomNumberList _rs;
    static RequirementPicker()
    {
        _rs = rs1.GetRandomGenerator();
    }

    public static int ForTree(TimeSpan duration, int outputAmount)
    {
        if (outputAmount < 1)
        {
            outputAmount = 1;
        }
        ActionRange range = TreeRequirementPolicy.GetRange(duration);

        //int actions = rng.Next(range.MinimumActions, range.MaxActions + 1);
        int actions = _rs.GetRandomNumber(range.MaximumActions, range.MinimumActions);
        int amount = actions * outputAmount;

        if (amount > AnimalRequirementPolicy.MaxAmount)
        {
            amount = AnimalRequirementPolicy.MaxAmount;
        }

        // Ensure at least one action worth
        if (amount < outputAmount)
        {
            amount = outputAmount;
        }

        return amount;
    }

    public static int ForCrop(TimeSpan duration)
    {
        
        ActionRange range = CropRequirementPolicy.GetRange(duration);

        //int actions = rng.Next(range.MinimumActions, range.MaxActions + 1);
        int amount = _rs.GetRandomNumber(range.MaximumActions, range.MinimumActions);
        return amount;
    }

    public static int ForAnimal(TimeSpan duration, int outputAmount)
    {
        if (outputAmount < 1)
        {
            outputAmount = 1;
        }

        ActionRange range = AnimalRequirementPolicy.GetRange(duration);

        //int actions = rng.Next(range.MinimumActions, range.MaxActions + 1);
        int actions = _rs.GetRandomNumber(range.MaximumActions, range.MinimumActions);
        int amount = actions * outputAmount;

        if (amount > AnimalRequirementPolicy.MaxAmount)
        {
            amount = AnimalRequirementPolicy.MaxAmount;
        }

        // Ensure at least one action worth
        if (amount < outputAmount)
        {
            amount = outputAmount;
        }

        return amount;
    }


    public static int ForWorkshop(WorkshopOutputAnalysis a)
    {
        // 1) time band

        int band = WorkshopRequirementPolicy.TimeBand.GetBandIndex(a.CraftTime + a.InputPrep);

        ActionRange range = band switch
        {
            0 => WorkshopRequirementPolicy.VeryFast,
            1 => WorkshopRequirementPolicy.Medium,
            2 => WorkshopRequirementPolicy.Long1,
            3 => WorkshopRequirementPolicy.VeryLong,
            _ => WorkshopRequirementPolicy.Longest
        };

        // 2) pick actions
        //int actions = rng.Next(range.MinimumActions, range.MaximumActions + 1);
        int actions = _rs.GetRandomNumber(range.MaximumActions, range.MinimumActions);

        // 3) penalties (harder => fewer actions)
        if (a.WorkshopInputs > 0)
        {
            actions -= WorkshopRequirementPolicy.MultiStepPenaltyActions;
        }

        if (a.WorstWorksiteInputRarity == EnumWorksiteRarity.Common)
        {
            actions -= WorkshopRequirementPolicy.WorksiteCommonPenaltyActions;
        }

        if (a.WorstWorksiteInputRarity == EnumWorksiteRarity.Rare)
        {
            actions -= WorkshopRequirementPolicy.WorksiteRarePenaltyActions;
        }

        if (actions < WorkshopRequirementPolicy.MinActions)
        {
            actions = WorkshopRequirementPolicy.MinActions;
        }

        // 4) convert to amount using output yield
        int output = Math.Max(1, a.OutputAmount);
        int amount = actions * output;

        // 5) clamp
        if (amount > WorkshopRequirementPolicy.MaxAmount)
        {
            amount = WorkshopRequirementPolicy.MaxAmount;
        }

        if (amount < output)
        {
            amount = output; // at least one craft
        }

        return amount;
    }

}