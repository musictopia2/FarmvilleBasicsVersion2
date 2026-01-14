namespace Phase12QuestsBasedOnLevel.Models;
public class AnimalProductionOption
{
    public ItemAmount Output { get; init; }
    public int Input { get; set; }
    public string Required { get; set; } = "";
    public TimeSpan Duration { get; init; }
}