using Phase12StoreWithBasicPurchases.Services.Core;

namespace Phase12StoreWithBasicPurchases.DataAccess.Quests;
public class QuestDocument : IFarmDocument
{
    required public FarmKey Farm { get; set; }
    required public BasicList<QuestRecipe> Quests { get; set; }
}