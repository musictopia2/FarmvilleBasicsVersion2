namespace Phase09LevelProgression.Components.Custom;
public partial class LevelDetailsModal(IToast toast)
{
    private string PointsToLevelUp => ProgressionManager.PointsNeededToLevelUp.ToString("N0");

    private string ImagePath(string name) => $"/{name}.png";
    private void ShowText(string key) => toast.ShowInfoToast(key);
}