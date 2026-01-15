namespace Phase12QuestsBasedOnLevel.Models;
//this does not do any random stuff now.
public class ItemPlanModel
{
    public int MinLevel { get; set; }             // eligibility gate
    public string ItemName { get; set; } = "";
    public EnumItemCategory Category { get; set; }

    public string Source { get; set; } = ""; //can be the building name, or the worksite place.
    //no need for rewards.  that comes later.
    //this is like the catalog but is for items (the results of having the catalog).
}