namespace Phase12QuestsBasedOnLevel.Models;
public class AnimalProgressionPlanDocument : IFarmDocument //repeat for others for future understanding.
{
    required public FarmKey Farm { get; set; }
    public BasicList<ItemUnlockRule> UnlockRules { get; set; } = [];
}