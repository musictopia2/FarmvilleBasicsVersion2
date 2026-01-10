namespace Phase07EarnCoinFromQuests.Components.Custom;
public partial class CoinIndicator
{
    private int CoinCount => InventoryManager.Get(CurrencyKeys.Coin);
}