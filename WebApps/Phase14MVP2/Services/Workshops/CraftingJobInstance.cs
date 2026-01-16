using Phase14MVP2.Services.Core;

namespace Phase14MVP2.Services.Workshops;
public class CraftingJobInstance(WorkshopRecipe recipe, double currentMultiplier)
{
    public WorkshopRecipe Recipe { get; } = recipe;
    public Guid Id { get; set; } = Guid.NewGuid();
    public EnumWorkshopState State { get; private set; } = EnumWorkshopState.Waiting;

    public DateTime? StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    // Current multiplier (not persisted)
    private readonly double _currentMultiplier = GameRegistry.ValidateMultiplier(currentMultiplier);

    // Locked promise for THIS job run (persisted while job exists)
    private double? _runMultiplier;

    // Use locked multiplier if present; otherwise current (for previews)
    private TimeSpan EffectiveDuration
    {
        get
        {
            var m = _runMultiplier ?? _currentMultiplier;
            return Recipe.Duration.Apply(m);
        }
    }

    public TimeSpan DurationForProcessing => EffectiveDuration;


    public bool IsComplete =>
        State == EnumWorkshopState.Active &&
        StartedAt is not null &&
        DateTime.Now - StartedAt.Value >= EffectiveDuration;

    public void Start()
    {
        // Lock promise at job start
        _runMultiplier = _currentMultiplier;

        StartedAt = DateTime.Now;
        CompletedAt = null;
        State = EnumWorkshopState.Active;
    }

    public void Load(CraftingAutoResumeModel craft)
    {
        State = craft.State;
        StartedAt = craft.StartedAt;
        CompletedAt = craft.CompletedAt;

        _runMultiplier = craft.RunMultiplier;

        // Back-compat / safety:
        // if job exists and has started but run multiplier missing, use current
        if (StartedAt is not null && _runMultiplier is null)
        {
            _runMultiplier = _currentMultiplier;
        }
        
    }

    public void Complete()
    {
        CompletedAt = DateTime.Now;
    }

    public void ReadyForManualPickup()
    {
        CompletedAt = DateTime.Now;
        State = EnumWorkshopState.ReadyToPickUpManually;
    }

    public void UpdateStartedAt(DateTime startedAt)
    {
        // If something external sets the start time, ensure we still have a promise
        _runMultiplier ??= _currentMultiplier;

        StartedAt = startedAt;
        CompletedAt = null;
        State = EnumWorkshopState.Active;
    }

    public TimeSpan? ReadyTime
    {
        get
        {
            if (State != EnumWorkshopState.Active || StartedAt is null)
            {
                return null;
            }

            var elapsed = DateTime.Now - StartedAt.Value;
            var remaining = EffectiveDuration - elapsed;
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }
    }

    public CraftingAutoResumeModel GetCraftingForSaving
    {
        get
        {
            // Persist the promise whenever the job exists / is in queue.
            // If you delete completed jobs from the queue, this still works.
            return new()
            {
                CompletedAt = CompletedAt,
                RecipeItem = Recipe.Item,
                StartedAt = StartedAt,
                State = State,

                // Save locked multiplier if job was ever started; otherwise null is fine
                RunMultiplier = StartedAt is null ? null : _runMultiplier
            };
        }
    }
}