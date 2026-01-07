namespace Phase04PrepareForMVP1.QuestHelpers;
public sealed class WorkshopOutputAnalysis
{
    public string WorkshopName { get; init; } = "";
    public string OutputItem { get; init; } = "";

    // direct input summary (start simple)
    public int CropInputs { get; set; }
    public int TreeInputs { get; set; }
    public int AnimalInputs { get; set; }
    public int WorkshopInputs { get; set; } // input is itself craftable
    public int WorksiteInputs { get; set; } // input obtainable from worksite (any rarity)

    public EnumWorksiteRarity WorstWorksiteInputRarity { get; set; } = EnumWorksiteRarity.None;

    // optional conveniences
    public bool HasRareWorksiteInput => WorstWorksiteInputRarity == EnumWorksiteRarity.Rare;


    public TimeSpan CraftTime { get; init; }   // <-- add this
    public int OutputAmount { get; init; } //usually be 1 but can change.
    // NEW: total “gather/prep” minutes needed per craft run, considering qty + yield
    //public double InputPrepMinutesPerCraft { get; set; }

    public TimeSpan InputPrep { get; set; }

}
