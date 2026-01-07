namespace Phase04PrepareForMVP1.QuestHelpers;
internal class QuestContainer
{
    required public BasicList<BasicItem> Basics { get; init; }
    required public BasicList<WorkshopOutputAnalysis> Workshops { get; init; }
    required public BasicList<WorksiteOutputAnalysis> Worksites { get; init; }
}