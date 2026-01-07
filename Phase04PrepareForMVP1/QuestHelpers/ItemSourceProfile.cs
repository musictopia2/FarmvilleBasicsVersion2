namespace Phase04PrepareForMVP1.QuestHelpers;
public sealed class ItemSourceProfile(string itemName, TimeSpan time, int amount)
{
    public string ItemName { get; } = itemName;
    public int Amount { get; } = amount;
    public EnumItemSource CurrentSource { get; private set; }
    public BasicList<EnumItemSource> Sources { get; } = [];
    //public BasicList<string> WorkshopProducers { get; } = []; // workshop names that output it
    //public BasicList<string> WorksiteSources { get; } = [];   // worksite names that can reward it (optional)
    public TimeSpan Time { get; } = time;
    public BasicList<WorksiteAvailability> WorksiteAvailabilities { get; } = [];
    //if this comes from an animal, then also has to consider the time before you can even do it.
    public string CropFrom { get; private set; } = "";


    public void AddAnimal(string crop)
    {
        AddSource(EnumItemSource.Animal);
        CropFrom = crop;
    }

    public void AddSource(EnumItemSource source)
    {
        if (Sources.Contains(source) == false)
        {
            CurrentSource = source;
            Sources.Add(source);
        }
    }


    public void AddWorksiteAvailability(WorksiteAvailability entry)
    {
        AddSource(EnumItemSource.Worksite);
        // simplest: just add; or merge by taking best chance, etc.
        WorksiteAvailabilities.Add(entry);
    }

    // Pick the “best” (easiest) rarity across locations.
    public EnumWorksiteRarity WorstWorksiteRarity()
    {
        if (WorksiteAvailabilities.Count == 0)
        {
            return EnumWorksiteRarity.None;
        }

        // hardest-to-easiest ordering
        if (WorksiteAvailabilities.Any(x => x.Rarity == EnumWorksiteRarity.Rare))
        {
            return EnumWorksiteRarity.Rare;
        }

        if (WorksiteAvailabilities.Any(x => x.Rarity == EnumWorksiteRarity.Common))
        {
            return EnumWorksiteRarity.Common;
        }

        if (WorksiteAvailabilities.Any(x => x.Rarity == EnumWorksiteRarity.Guaranteed))
        {
            return EnumWorksiteRarity.Guaranteed;
        }
        throw new CustomBasicException("None");
    }

    public bool Has(EnumItemSource source) => Sources.Contains(source);

    public bool IsWorksiteOnly =>
        Sources.Count == 1 && Sources[0] == EnumItemSource.Worksite;
}