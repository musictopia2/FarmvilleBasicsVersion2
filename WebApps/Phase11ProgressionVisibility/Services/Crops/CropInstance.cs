using Phase11ProgressionVisibility.Services.Core;

namespace Phase11ProgressionVisibility.Services.Crops;
public class CropInstance(double currentMultiplier, CropRecipe? currentRecipe)
{
    public Guid Id { get; } = Guid.NewGuid(); // unique per instance
    public bool Unlocked { get; set; }
    public EnumCropState State { get; private set; } = EnumCropState.Empty;
    public DateTime? PlantedAt { get; private set; }
    public string? Crop { get; private set; } = null;
    public TimeSpan? GrowTime { get; private set; } = null;
    // NEW: separate the two meanings
    private readonly double _currentMultiplier = GameRegistry.ValidateMultiplier(currentMultiplier);
    private double? _runMultiplier; // locked per run; null when idle



    public TimeSpan? ReadyTime
    {
        get
        {
            if (State != EnumCropState.Growing || PlantedAt is null)
            {
                return null;
            }
            var elapsed = DateTime.Now - PlantedAt.Value;
            var remaining = GrowTime - elapsed;
            return remaining > TimeSpan.Zero
                ? remaining
                : TimeSpan.Zero;
        }
    }

    private TimeSpan? GetDuration
    {
        get
        {
            if (currentRecipe is null)
            {
                return null;
            }
            // If producing, use locked promise. If idle (UI preview), use current.
            var m = _runMultiplier ?? _currentMultiplier;
            return currentRecipe.Duration.Apply(m);
        }
    }

    public void Load(CropAutoResumeModel slot)
    {
        Crop = slot.Crop;
        State = slot.State;
        PlantedAt = slot.PlantedAt;
        Unlocked = slot.Unlocked;

        _runMultiplier = slot.RunMultiplier;
        // If something is/was planted, ensure a run multiplier exists
        if (Crop is not null && _runMultiplier is null)
        {
            _runMultiplier = _currentMultiplier;
        }
        GrowTime = Crop is null ? null : GetDuration;
    }
    public void Plant(string crop, CropRecipe recipe)
    {
        State = EnumCropState.Growing;
        currentRecipe = recipe;
        _runMultiplier = _currentMultiplier;
        GrowTime = GetDuration; //must send the recipe now.  can't trust the time sent anymore.
        Crop = crop;
        PlantedAt = DateTime.Now;
    }
    public void UpdateTick()
    {
        if (State != EnumCropState.Growing || PlantedAt == null)
        {
            return;
        }

        var elapsed = DateTime.Now - PlantedAt.Value;
        if (elapsed >= GrowTime)
        {
            State = EnumCropState.Ready;
            PlantedAt = null;
        }
    }
    public double? GetCurrentRun => currentRecipe is null ? null : _runMultiplier;
    public void Clear()
    {
        State = EnumCropState.Empty;
        Crop = null;
        GrowTime = null;
        PlantedAt = null;
        _runMultiplier = null;
        currentRecipe = null; //don't know anymore the recipe until sent again.
    }
}