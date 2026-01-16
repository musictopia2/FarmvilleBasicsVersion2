namespace Phase14MVP2.Services.Store;

public class StoreItemRowModel
{
    public EnumCatalogCategory Category { get; init; }
    public string TargetName { get; init; } = "";

    // 1) LevelRequired (for the NEXT purchase tier)
    public int LevelRequired { get; init; }

    // 2) Total possible items (how many tiers exist)
    public int TotalPossible { get; init; }

    // 3) Currently owned items
    public int OwnedCount { get; init; }

    // 4) Costs for the NEXT purchase tier (multi-currency)
    public Dictionary<string, int> Costs { get; init; } = [];

    // These make UI display easy (recommended)
    public bool IsLocked { get; init; }      // locked by level
    public bool IsMaxedOut { get; init; }    // owned >= total possible
    public bool IsFree => Costs.Count == 0;
}