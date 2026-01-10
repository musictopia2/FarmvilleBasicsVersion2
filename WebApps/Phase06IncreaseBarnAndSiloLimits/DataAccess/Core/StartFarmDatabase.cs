using Phase06IncreaseBarnAndSiloLimits.Services.Core;

namespace Phase06IncreaseBarnAndSiloLimits.DataAccess.Core;
public class StartFarmDatabase() : ListDataAccess<FarmKey>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IStartFarmRegistry

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "Start";
    Task<BasicList<FarmKey>> IStartFarmRegistry.GetFarmsAsync()
    {
        return GetDocumentsAsync();
    }
}