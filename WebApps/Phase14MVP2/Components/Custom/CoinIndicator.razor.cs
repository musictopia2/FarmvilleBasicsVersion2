namespace Phase14MVP2.Components.Custom;
public partial class CoinIndicator
{
    private int CoinCount => InventoryManager.Get(CurrencyKeys.Coin);
    private string CoinCountDisplay => CoinCount.ToString("N0");
}