namespace Phase14MVP2.Components.Custom;
public class FarmContext
{
    public MainFarmContainer? Current { get; private set; }
    public void Set(MainFarmContainer farm) => Current = farm;
}