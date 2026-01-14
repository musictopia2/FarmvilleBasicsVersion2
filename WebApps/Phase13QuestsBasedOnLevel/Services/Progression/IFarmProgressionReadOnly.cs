namespace Phase13QuestsBasedOnLevel.Services.Progression;
public interface IFarmProgressionReadOnly
{
    int CurrentLevel { get; }
    bool CompletedGame { get; }
    event Action? Changed;
}