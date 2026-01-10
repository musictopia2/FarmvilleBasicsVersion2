namespace Phase09LevelProgression.Components.Custom;
public partial class CoinIndicator
{
    private int CoinCount => InventoryManager.Get(CurrencyKeys.Coin);
    private string CoinCountDisplay => CoinCount.ToString("N0");
}