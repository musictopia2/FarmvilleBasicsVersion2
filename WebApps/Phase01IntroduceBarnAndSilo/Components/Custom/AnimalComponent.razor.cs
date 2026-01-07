namespace Phase01IntroduceBarnAndSilo.Components.Custom;

public partial class AnimalComponent(IToast toast)
{

    [Parameter]
    [EditorRequired]
    public AnimalView Animal { get; set; } = default!;

    private int _selectedIndex;
    private bool _showOptions;
    private AnimalProductionOption? _selectedOption;

    protected override void OnParametersSet()
    {
        EnsureSelectedOption();
        base.OnParametersSet();
    }

    private void EnsureSelectedOption()
    {
        var options = AnimalManager.GetUnlockedProductionOptions(Animal);
        if (options.Count == 0)
        {
            _selectedOption = null;
            return;
        }

        if (_selectedIndex < 0 || _selectedIndex >= options.Count)
        {
            _selectedIndex = 0;
        }

        _selectedOption = options[_selectedIndex];
    }

    private EnumAnimalState State => AnimalManager.GetState(Animal);

    private bool CanOpenOptions => State == EnumAnimalState.None;
    private bool CanCollect => State == EnumAnimalState.Collecting && AnimalManager.Left(Animal) > 0;

    private bool _showProgress;

    private bool CanProduce
    {
        get
        {
            if (State != EnumAnimalState.None)
            {
                return false;
            }
            return AnimalManager.CanProduce(Animal, _selectedIndex);
        }
    }

    private async void CardClickedAsync()
    {
        if (CanCollect)
        {
            await AnimalManager.CollectAsync(Animal);
            return;
        }
        if (State == EnumAnimalState.Producing)
        {
            _showProgress = true;
            return;
        }
        if (CanProduce == false)
        {
            toast.ShowUserErrorToast("Unable to produce because you do not have enough required ingredients");
            return;
        }
        if (CanOpenOptions)
        {
            _showOptions = true;
            return;
        }
       
    }


    private void ClosePopup() => _showOptions = false;

    // NEW: when an option is chosen, immediately start producing
    private void SelectOptionAndStart(int index)
    {
        int original = _selectedIndex;
        _selectedIndex = index;
        EnsureSelectedOption();
        ClosePopup();

        if (State != EnumAnimalState.None)
        {
            return;
        }

        if (AnimalManager.CanProduce(Animal, _selectedIndex) == false)
        {
            toast.ShowUserErrorToast("You don't have enough required ingredients for that option.");
            _selectedIndex = original;
            EnsureSelectedOption();
            return;
        }

        AnimalManager.Produce(Animal, _selectedIndex);
    }

    // --- Image path helpers (edit these to match your project) ---
    private static string Normalize(string text)
        => text.Replace(" ", "").Replace("-", "").Replace("_", "").ToLowerInvariant();

    private static string GetAnimalImage(string animalName)
        => $"/{Normalize(animalName)}.png";

    private static string GetItemImage(string itemName)
        => $"/{Normalize(itemName)}.png";


    private string GetBackGround
    {
        get
        {
            var state = State;
            if (state == EnumAnimalState.Producing)
            {
                return cc1.DarkGray.ToWebColor;
            }
            if (state == EnumAnimalState.Collecting)
            {
                return cc1.Lime.ToWebColor;
            }
            if (CanProduce == false)
            {
                return cc1.DarkGray.ToWebColor;
            }
            return cc1.White.ToWebColor;
        }
    }


}