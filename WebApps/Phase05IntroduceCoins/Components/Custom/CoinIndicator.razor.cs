namespace Phase05IntroduceCoins.Components.Custom;
public partial class CoinIndicator
{
    private int CoinCount => Inventory.Get(CurrencyKeys.Coin);
}