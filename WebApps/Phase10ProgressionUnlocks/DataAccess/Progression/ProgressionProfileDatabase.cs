namespace Phase10ProgressionUnlocks.DataAccess.Progression;
public class ProgressionProfileDatabase(FarmKey farm) : ListDataAccess<ProgressionProfileDocument>
    (DatabaseName, CollectionName, mm1.DatabasePath),
    ISqlDocumentConfiguration, IProgressionProfile

{
    public static string DatabaseName => mm1.DatabaseName;
    public static string CollectionName => "ProgressionProfile";
    async Task<ProgressionProfileModel> IProgressionProfile.LoadAsync()
    {
        var list = await GetDocumentsAsync();
        var item = list.Single(x => x.Farm.Equals(farm));
        return new()
        {
            Level = item.Level,
            CompletedGame = item.CompletedGame,
            PointsThisLevel = item.PointsThisLevel, 
        };
    }
    async Task IProgressionProfile.SaveAsync(ProgressionProfileModel profile)
    {
        var list = await GetDocumentsAsync();

        var current = list.Single(x => x.Farm.Equals(farm));
        current.Level = profile.Level;
        current.PointsThisLevel = profile.PointsThisLevel;
        current.CompletedGame = profile.CompletedGame;
        await UpsertRecordsAsync(list);
    }
}