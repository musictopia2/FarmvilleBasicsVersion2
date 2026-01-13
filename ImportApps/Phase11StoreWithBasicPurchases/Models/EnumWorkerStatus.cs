namespace Phase11StoreWithBasicPurchases.Models;
public enum EnumWorkerStatus
{
    None,
    Selected,
    Working //if they are working, they can't be on another site.
}