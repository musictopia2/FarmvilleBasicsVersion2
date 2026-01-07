using Phase02BarnAndSiloLimits.Services.Core;

namespace Phase02BarnAndSiloLimits.DataAccess.Balance;

public class BalanceProfileDatabase() : ListDataAccess<BalanceProfileDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IBaseBalanceProvider

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "BalanceCraftingTimes";
    async Task<BaseBalanceProfile> IBaseBalanceProvider.GetBaseBalanceAsync(FarmKey farm)
    {
        var list = await GetDocumentsAsync();

        var firsts = list.Single(x => x.Farm.Equals(farm));
        BaseBalanceProfile output = new()
        {
            AnimalTimeMultiplier = firsts.AnimalTimeMultiplier,
            CropTimeMultiplier = firsts.CropTimeMultiplier,
            TreeTimeMultiplier = firsts.TreeTimeMultiplier,
            WorkshopTimeMultiplier = firsts.WorkshopTimeMultiplier,
            WorksiteTimeMultiplier = firsts.WorksiteTimeMultiplier
        };
        return output;
    }
}