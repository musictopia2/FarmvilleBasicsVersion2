namespace Phase12QuestsBasedOnLevel.Models;
public class QuestInstanceModel
{

    //this will not focus on forever mode.  that is future enhancement (way later).


    public string QuestId { get; set; } = "";      // GUID string


    //may eventually take this out.
    public int CreatedAtLevel { get; set; }         // player level when generated


    

    // What the quest asks for (from ItemPlanModel)
    public string ItemName { get; set; } = "";

    //this is the real quest.   at this point, does not matter where it comes from.

    //public EnumItemCategory Category { get; set; }
    //public string Source { get; set; } = "";        // blank allowed

    // Meta-state
    public bool Seen { get; set; }                  // first time it appeared on board
    public bool Tracked { get; set; }               // pinned (cap 4)
    public int Order { get; set; }
    //public EnumQuestStatus Status { get; set; } = EnumQuestStatus.Active;

    // Completion bookkeeping (even if completion is immediate, this is handy)
    //public DateTime? CompletedUtc { get; set; }
}
