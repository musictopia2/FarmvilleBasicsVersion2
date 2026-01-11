namespace Phase10ProgressionUnlocks.Services.Progression;
public interface IProgressionFactory
{
    ProgressionServicesContext GetProgressionServices(FarmKey farm);
}