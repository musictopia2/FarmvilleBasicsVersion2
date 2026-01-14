namespace Phase13QuestsBasedOnLevel.DataAccess.Progression;
public class ProgressionProfileDocument
{
    required public FarmKey Farm { get; set; }
    // Player-facing
    public int Level { get; set; } = 1;
    // Internal counter for THIS level (player sees as progress bar value) (used so it can figure out the progress)
    public int PointsThisLevel { get; set; } = 0;
    public bool CompletedGame { get; set; } //if you are at the highest level and completed it and no more left, then you completed the game.
}