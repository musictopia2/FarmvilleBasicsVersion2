namespace Phase14MVP2.Services.Worksites;
public class WorksiteOutput
{
    public bool Original { get; set; } = true; //most are original.   some are extra only with some workers.
    public int Amount { get; set; }
    public double Chances { get; set; }
    public string Item { get; set; } = ""; //okay to duplicate it here i think (if i am wrong, rethink).
}