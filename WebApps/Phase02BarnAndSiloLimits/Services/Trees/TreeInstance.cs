using Phase02BarnAndSiloLimits.Services.Core;

namespace Phase02BarnAndSiloLimits.Services.Trees;

public class TreeInstance(
    TreeRecipe tree,
    ITreesCollecting collecting,
    double currentMultiplier
)
{
    public Guid Id { get; } = Guid.NewGuid();
    public bool Unlocked { get; set; }

    public string Name => tree.Item;
    public string TreeName => tree.TreeName;

    public int TreesReady { get; private set; } = collecting.TreesCollectedAtTime;
    public EnumTreeState State { get; private set; } = EnumTreeState.Collecting;

    private TimeSpan ProductionTimePerTree => tree.ProductionTimeForEach;

    // production start time (used mostly for UI/pause semantics)
    private DateTime? StartedAt { get; set; }

    // production clock for per-tree accumulation
    private DateTime? TempStart { get; set; }

    private bool IsCollecting { get; set; } = false;

    private readonly double _currentMultiplier = GameRegistry.ValidateMultiplier(currentMultiplier);
    private double? _runMultiplier; // locked per production batch; null when not producing

    // Apply multiplier to time per tree
    private TimeSpan ProductionTimePerTreeAdjusted
    {
        get
        {
            var m = _runMultiplier ?? _currentMultiplier;
            return ProductionTimePerTree.Apply(m);
        }
    }
    public TimeSpan BaseTime => ProductionTimePerTree;

    public TimeSpan? ReadyTime
    {
        get
        {
            if (State != EnumTreeState.Producing || TempStart is null)
            {
                return null;
            }

            var elapsed = DateTime.Now - TempStart.Value;

            var perTree = ProductionTimePerTreeAdjusted;
            var totalTime = TimeSpan.FromTicks(perTree.Ticks * collecting.TreesCollectedAtTime);

            var remaining = totalTime - elapsed;
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }
    }

    public bool CanCollectOneTree => TreesReady > 0;

    private void StartCollecting()
    {
        if (State == EnumTreeState.Collecting && !IsCollecting)
        {
            IsCollecting = true;
            StartedAt = null; // pause timer
        }
    }

    public TreeAutoResumeModel GetTreeForSaving
    {
        get
        {
            return new()
            {
                StartedAt = StartedAt,
                TempStart = TempStart,
                State = State,
                TreeName = TreeName,
                TreesReady = TreesReady,
                Unlocked = Unlocked,

                // Save the promise ONLY while producing
                RunMultiplier = State == EnumTreeState.Producing ? _runMultiplier : null
            };
        }
    }

    public void Load(TreeAutoResumeModel model)
    {
        State = model.State;
        TempStart = model.TempStart;
        Unlocked = model.Unlocked;
        TreesReady = model.TreesReady;
        StartedAt = model.StartedAt;

        _runMultiplier = model.RunMultiplier;

        // Back-compat / safety: if producing but multiplier missing, fall back to current
        if (State == EnumTreeState.Producing && _runMultiplier is null)
        {
            _runMultiplier = _currentMultiplier;
        }
    }

    public void CollectTree()
    {
        StartCollecting();

        if (TreesReady <= 0)
        {
            return;
        }

        TreesReady--;

        if (TreesReady == 0)
        {
            // Start production for next batch
            _runMultiplier = _currentMultiplier; // LOCK promise
            State = EnumTreeState.Producing;
            IsCollecting = false;

            // both clocks start now
            StartedAt = DateTime.Now;
            TempStart = DateTime.Now;
        }
    }

    private bool _needsSaving;
    public bool NeedsToSave => _needsSaving;

    public void UpdateTick()
    {
        _needsSaving = false;

        if (State != EnumTreeState.Producing || TempStart is null)
        {
            return;
        }

        // Ensure run multiplier exists if something started producing without it (defensive)
        _runMultiplier ??= _currentMultiplier;

        var elapsed = DateTime.Now - TempStart.Value;

        var perTree = ProductionTimePerTreeAdjusted;
        var perTreeSeconds = perTree.TotalSeconds;

        if (perTreeSeconds <= 0)
        {
            // should never happen if recipes are valid, but avoid divide-by-zero
            throw new CustomBasicException("Invalid per-tree production time (<= 0 seconds).");
        }

        // How many full trees should have been produced
        int produced = (int)(elapsed.TotalSeconds / perTreeSeconds);

        if (produced <= 0)
        {
            return;
        }

        TreesReady += produced;
        _needsSaving = true;

        if (TreesReady >= collecting.TreesCollectedAtTime)
        {
            TreesReady = collecting.TreesCollectedAtTime;
            State = EnumTreeState.Collecting;

            StartedAt = null;
            TempStart = null;

            // Promise no longer needed once batch is done
            _runMultiplier = null;
        }
        else
        {
            // Advance TempStart by the amount of time used for produced trees
            TempStart = TempStart.Value.AddSeconds(produced * perTreeSeconds);
        }
    }
}