namespace Phase04PrepareForMVP1.QuestHelpers;
public static class WorkshopRequirementPolicy
{
    // 4 bands usually works well for crafts
    public static TimeBandRules TimeBand => new()
    {
        MaxTimes =
        [
            TimeSpan.FromMinutes(5),   // very fast crafts
            TimeSpan.FromMinutes(10),  // medium
            TimeSpan.FromMinutes(30),
            TimeSpan.FromHours(1)
            // >1 hour = very long (for this version)
        ]
    };

    // actions = how many craft cycles to request
    // (then multiplied by OutputAmount)
    public static ActionRange VeryFast => new(5, 8);
    public static ActionRange Medium => new(4, 6);
    public static ActionRange Long1 => new(3, 4);
    public static ActionRange VeryLong => new(2, 3);
    public static ActionRange Longest => new(1, 2);

    // Penalties reduce actions (harder => fewer crafts required)
    // These are applied after choosing the base range.
    public static int MultiStepPenaltyActions => 0; // if WorkshopInputs > 0
    public static int WorksiteCommonPenaltyActions => 1; // WorstWorksiteInputRarity == Common
    public static int WorksiteRarePenaltyActions => 2;   // WorstWorksiteInputRarity == Rare

    public static int MinActions => 1;
    public static int MaxAmount => 20; // clamp final requested amount (after outputAmount)
}