namespace Phase06IncreaseBarnAndSiloLimits.Components.Custom;
public partial class CoinIndicator
{
    private int CoinCount => Inventory.Get(CurrencyKeys.Coin);
}