namespace Phase12StoreWithBasicPurchases.Components.Custom;

public partial class DiscardComponent(IntegerQuantityPickerService quantityPickerService) : IDisposable
{
    [Parameter]
    [EditorRequired]
    public string ItemName { get; set; } = "";

    [Parameter]
    [EditorRequired]
    public int Limit { get; set; }
    

    [Parameter]
    [EditorRequired]
    public int CurrentCount { get; set; }

    [Parameter]
    public EventCallback<ItemAmount> OnDiscard { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    private int _currentAmount;

    private int _proposed;

    private string FullDisplay => $"{CurrentCount} Current {Limit} Limit"; //for now okay.

    protected override void OnInitialized()
    {
        quantityPickerService.Completed = ChoseCustom;
        base.OnInitialized();
    }
    private void ChoseCustom(int amount)
    {
        if (amount <= 0)
        {
            return; //don't do because chose 0.
        }
        if (_proposed > _currentAmount)
        {
            _proposed = _currentAmount;
            return;
        }
        _proposed = amount;
        InvokeAsync(StateHasChanged);
    }

    private void OpenCustom()
    {
        quantityPickerService.Pick(_currentAmount);
         
    }

    private void Submit()
    {
        if (_proposed <= 0)
        {
            return;
        }
        ItemAmount item = new(ItemName, _proposed);
        OnDiscard.InvokeAsync(item);
    }

    private void GoLeft()
    {
        if (_proposed <= 1)
        {
            return;
        }
        _proposed--;
    }
    private void GoRight()
    {
        if (_proposed >= _currentAmount)
        {
            return;
        }
        _proposed++;
    }

    protected override void OnParametersSet()
    {
        _currentAmount = InventoryManager.Get(ItemName);
        base.OnParametersSet();
    }
    private void ProposeAll()
    {
        _proposed = _currentAmount;
    }
    private void ProposeOne()
    {
        _proposed = 1;
    }

    public void Dispose()
    {
        quantityPickerService.Completed = null;
        GC.SuppressFinalize(this);
    }
}