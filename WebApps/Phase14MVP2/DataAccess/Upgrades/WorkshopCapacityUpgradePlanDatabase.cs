namespace Phase14MVP2.DataAccess.Upgrades;
public class WorkshopCapacityUpgradePlanDatabase() : ListDataAccess<WorkshopCapacityUpgradePlanDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IWorkshopCapacityUpgradePlanProvider

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "WorkshopCapacityUpgradePlan";
    async Task<BasicList<WorkshopCapacityUpgradePlanModel>> IWorkshopCapacityUpgradePlanProvider.GetPlansAsync(FarmKey farm)
    {
        BasicList<WorkshopCapacityUpgradePlanDocument> list = await GetDocumentsAsync();
        var filters = list.Where(x => x.Farm.Equals(farm)).ToBasicList();
        BasicList<WorkshopCapacityUpgradePlanModel> output = [];
        foreach (var item in filters)
        {
            output.Add(new()
            {
                Upgrades = item.Upgrades,
                WorkshopName = item.WorkshopName
            });
        }
        return output;
    }
}