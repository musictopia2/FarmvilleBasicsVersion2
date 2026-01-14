namespace Phase12QuestsBasedOnLevel.Models;

public class CropProgressionPlanDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    // Total capacity that *exists* (even if locked)
    public BasicList<int> SlotLevelRequired { get; set; } = [];
    public BasicList<ItemUnlockRule> UnlockRules { get; set; } = [];
}