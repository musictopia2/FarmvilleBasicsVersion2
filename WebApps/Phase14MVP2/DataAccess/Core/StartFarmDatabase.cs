using Phase14MVP2.Services.Core;

namespace Phase14MVP2.DataAccess.Core;
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