namespace Phase13QuestsBasedOnLevel.Components.Custom;
public partial class AnimalsComponent : IDisposable
{
    private BasicList<AnimalView> _animals = [];
    override protected void OnInitialized()
    {
        AnimalManager.OnAnimalsUpdated += Refresh;
        UpdateAnimals();
    }
    private void UpdateAnimals()
    {
        _animals = AnimalManager.GetUnlockedAnimals;
    }
    private void Refresh()
    {
        UpdateAnimals();
        StateHasChanged();
    }
    public void Dispose()
    {
        AnimalManager.OnAnimalsUpdated -= Refresh;
        GC.SuppressFinalize(this);
    }
}