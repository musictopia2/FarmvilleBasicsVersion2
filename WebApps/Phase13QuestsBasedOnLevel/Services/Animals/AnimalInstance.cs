using Phase13QuestsBasedOnLevel.Services.Core;

namespace Phase13QuestsBasedOnLevel.Services.Animals;
public class AnimalInstance(AnimalRecipe recipe, double currentMultiplier)
{
    public Guid Id { get; } = Guid.NewGuid();
    public bool Unlocked { get; set; } = true;

    public BasicList<AnimalProductionOption> GetUnlockedProductionOptions()
        => recipe.Options.Take(ProductionOptionsAllowed).ToBasicList();

    public int ProductionOptionsAllowed { get; set; }
    public int TotalProductionOptions => recipe.Options.Count;
    public AnimalProductionOption NextProductionOption => recipe.Options.Skip(ProductionOptionsAllowed).Take(1).Single();
    public string Name => recipe.Animal;

    public int OutputReady { get; private set; } = 0;
    public EnumAnimalState State { get; set; } = EnumAnimalState.None;

    public TimeSpan? Duration { get; private set; }
    public DateTime? StartedAt { get; private set; }

    private int? _selected;

    // NEW: separate the two meanings
    private readonly double _currentMultiplier = GameRegistry.ValidateMultiplier(currentMultiplier);
    private double? _runMultiplier; // locked per run; null when idle


    //if i need rebalancing, rethink then.  not until then.
    public void UpdateReady(int amount)
    {
        OutputReady = amount;
        _selected = 0; //must choose this as well.
    }
    public void Load(AnimalAutoResumeModel animal)
    {
        State = animal.State;
        OutputReady = animal.OutputReady;
        StartedAt = animal.StartedAt;
        ProductionOptionsAllowed = animal.ProductionOptionsAllowed;
        Unlocked = animal.Unlocked;
        _selected = animal.Selected;

        // Restore locked promise only (do NOT overwrite current multiplier)
        _runMultiplier = animal.RunMultiplier;

        // Back-compat / safety: if something is in-progress but no run multiplier was saved
        if (_selected is not null && _runMultiplier is null)
        {
            _runMultiplier = _currentMultiplier;
        }

        if (_selected is not null)
        {
            Duration = GetDuration(_selected.Value);
        }
        else
        {
            Duration = null;
        }
    }

    public AnimalAutoResumeModel GetAnimalForSaving
    {
        get
        {
            return new()
            {
                Name = Name,
                StartedAt = StartedAt,
                Unlocked = Unlocked,
                OutputReady = OutputReady,
                ProductionOptionsAllowed = ProductionOptionsAllowed,
                State = State,
                Selected = _selected,

                // Save the promise only when a run exists; otherwise null
                RunMultiplier = _selected is null ? null : _runMultiplier
            };
        }
    }

    public string RequiredName(int selected) => recipe.Options[selected].Required;

    public string ReceivedName
    {
        get
        {
            if (_selected is null)
            {
                throw new CustomBasicException("There was nothing selected");
            }
            return recipe.Options[_selected.Value].Output.Item;
        }
    }

    public int RequiredCount(int selected) => recipe.Options[selected].Input;
    public int Returned(int selected) => recipe.Options[selected].Output.Amount;

    public int AmountInProgress
    {
        get
        {
            if (_selected is null)
            {
                throw new CustomBasicException("There was nothing selected");
            }

            return recipe.Options[_selected.Value].Output.Amount;
        }
    }

    public TimeSpan GetDuration(int selected)
    {
        var option = recipe.Options[selected];

        // If producing, use locked promise. If idle (UI preview), use current.
        var m = _runMultiplier ?? _currentMultiplier;

        return option.Duration.Apply(m);
    }

    public void Produce(int selected)
    {
        if (State != EnumAnimalState.None)
        {
            return;
        }

        _selected = selected;

        // Lock promise for this run
        _runMultiplier = _currentMultiplier;

        State = EnumAnimalState.Producing;
        OutputReady = Returned(selected);
        Duration = GetDuration(selected);
        StartedAt = DateTime.Now; // consider UtcNow later if you want
    }

    private bool _needsSaving;
    public bool NeedsToSave => _needsSaving;

    public void UpdateTick()
    {
        _needsSaving = false;

        if (State != EnumAnimalState.Producing || StartedAt is null || Duration is null)
        {
            return;
        }

        var elapsed = DateTime.Now - StartedAt.Value;
        if (elapsed >= Duration)
        {
            State = EnumAnimalState.Collecting;
            StartedAt = null;
            _needsSaving = true;
        }
    }

    public void Collect()
    {
        if (OutputReady <= 0)
        {
            return;
        }

        OutputReady--;
        if (OutputReady == 0)
        {
            State = EnumAnimalState.None;
            _selected = null;

            // Clear promise for next run
            _runMultiplier = null;

            Duration = null;
        }
    }

    public TimeSpan? ReadyTime
    {
        get
        {
            if (State != EnumAnimalState.Producing || StartedAt is null || Duration is null)
            {
                return null;
            }

            var elapsed = DateTime.Now - StartedAt.Value;
            var remaining = Duration - elapsed;
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }
    }

    
}
