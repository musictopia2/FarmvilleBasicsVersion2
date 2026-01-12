namespace Phase10ProgressionUnlocks.Services.Crops;
public interface ICropRepository
{
    Task<CropSystemState> LoadAsync();
    Task SaveAsync(CropSystemState state);
}