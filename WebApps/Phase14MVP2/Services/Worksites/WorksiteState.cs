namespace Phase14MVP2.Services.Worksites;
public class WorksiteState
{
    public string Name { get; set; } = "";
    public bool Unlocked { get; set; }
    public EnumWorksiteState State { get; set; }
}
