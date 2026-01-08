namespace Phase03Discarding.Components.Custom;

public partial class DiscardComponent
{
    [Parameter]
    [EditorRequired]
    public string ItemName { get; set; } = "";

    [Parameter]
    public EventCallback<ItemAmount> OnDiscard { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }


    private void PretendDiscard()
    {
        ItemAmount item = new(ItemName, 10);
        OnDiscard.InvokeAsync(item);
    }

    



}