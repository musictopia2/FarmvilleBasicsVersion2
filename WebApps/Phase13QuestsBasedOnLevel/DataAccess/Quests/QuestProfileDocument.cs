namespace Phase13QuestsBasedOnLevel.DataAccess.Quests;
public class QuestProfileDocument : IFarmDocument
{
    required public FarmKey Farm { get; init; }
    public BasicList<QuestInstanceModel> Quests { get; set; } = []; //must be blank since its resetting.
    //any other player stuff needed will be in the real project, not here.

}