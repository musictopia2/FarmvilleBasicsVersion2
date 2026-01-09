namespace Phase04EnforcingLimits.Components.Custom;
public partial class ShowWorksiteModal
{
    [Parameter]
    public bool Visible { get; set; }
    [Parameter]
    public EventCallback<bool> VisibleChanged { get; set; }
    [Parameter]
    public string? Location { get; set; }
    [Parameter]
    public EventCallback<string?> LocationChanged { get; set; }

    private void CloseWorksite()
    {
        Visible = false;
        VisibleChanged.InvokeAsync();
        Location = null;
        LocationChanged.InvokeAsync(Location);
    }

    private bool CanCloseWorksiteAutomatically
    {
        get
        {
            return this.CanCloseWorksiteAutomatically(Location);
            
        }
    }

    
}