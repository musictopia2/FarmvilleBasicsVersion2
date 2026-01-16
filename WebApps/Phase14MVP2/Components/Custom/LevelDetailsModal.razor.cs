namespace Phase14MVP2.Components.Custom;
public partial class LevelDetailsModal(IToast toast)
{
    private string PointsToLevelUp => ProgressionManager.PointsNeededToLevelUp.ToString("N0");
    private static string ImagePath(string name)
    {
        if (name.ToLower().Contains("tree"))
        {
            return "/tree.png";
        }
        return $"/{name}.png";
    }

    //private static string ImagePath(string name) => $"/{name}.png";
    private void ShowText(string key) => toast.ShowInfoToast(key.GetWords);


    private string GetDetails
    {
        get
        {
            string levelText = PointsToLevelUp;
            int nextLevel = ProgressionManager.NextLevel;
            return $"{levelText} more points to each level {nextLevel} to unlock:";


        }
    }
    

}