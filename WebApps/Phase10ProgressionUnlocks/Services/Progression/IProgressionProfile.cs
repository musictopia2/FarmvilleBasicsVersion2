namespace Phase10ProgressionUnlocks.Services.Progression;
public interface IProgressionProfile
{
    Task<ProgressionProfileModel> LoadAsync();
    Task SaveAsync(ProgressionProfileModel profile);
}