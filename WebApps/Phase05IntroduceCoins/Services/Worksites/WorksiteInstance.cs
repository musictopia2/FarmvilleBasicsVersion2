namespace Phase05IntroduceCoins.Services.Worksites;
public class WorksiteInstance(WorksiteRecipe recipe, double currentMultiplier,
    BasicList<WorkerRecipe> allWorkers
    )
{
    public const string NoneLocation = "None";
    public Dictionary<string, int> FailureHistory { get; set; } = [];
    public bool Unlocked { get; set; }
    public bool Focused { get; set; }
    public string Location => recipe.Location;

    // Base duration from recipe (canonical)
    public TimeSpan BaseDuration => recipe.Duration;

    public DateTime? StartedAt { get; private set; }
    public EnumWorksiteState Status { get; set; } = EnumWorksiteState.None;

    public int MaximumWorkers => recipe.MaximumWorkers;
    public BasicList<WorkerRecipe> Workers { get; private set; } = [];
    private BasicList<ItemAmount> _rewards = [];

    // NEW: current vs run multiplier
    private readonly double _currentMultiplier = GameRegistry.ValidateMultiplier(currentMultiplier);
    private double? _runMultiplier; // locked per job run; null when idle


    public bool HasRecipe(string name) => recipe.BaselineBenefits.Exists(x => x.Item == name);
    public TimeSpan EffectiveDuration
    {
        get
        {
            var m = _runMultiplier ?? _currentMultiplier;
            return BaseDuration.Apply(m);
        }
    }

    public TimeSpan? ReadyTime
    {
        get
        {
            if (Status != EnumWorksiteState.Processing || StartedAt is null)
            {
                return null;
            }

            var elapsed = DateTime.Now - StartedAt.Value;
            var remaining = EffectiveDuration - elapsed;
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }
    }

    public void Load(WorksiteAutoResumeModel worksite)
    {
        Unlocked = worksite.Unlocked;
        _rewards = worksite.Rewards;
        Status = worksite.Status;
        StartedAt = worksite.StartedAt;
        Focused = worksite.Focused;
        FailureHistory = worksite.FailureHistory ?? [];
        //needs to rethink this part eventually if more fields are needed.  for now its manually done.
        _runMultiplier = worksite.RunMultiplier;

        //if a job is processing, still needs to use the run multiplier.
        if (Status == EnumWorksiteState.Collecting &&
            _runMultiplier is null)
        {
            _runMultiplier = _currentMultiplier;
        }

        Workers.Clear();
    }

    public void AddWorkerAfterLoading(WorkerRecipe worker)
    {
        Workers.Add(worker);
    }

    public WorksiteAutoResumeModel GetWorksiteForSaving
    {
        get
        {
            // Save promise whenever the worksite is not idle
            double? saveRun =
                Status == EnumWorksiteState.None ? null : _runMultiplier;
            FailureHistory = FailureHistory
                .Where(x => x.Value > 0)
                .ToDictionary(x => x.Key, x => x.Value);
            return new()
            {
                Name = Location,
                Rewards = _rewards,
                StartedAt = StartedAt,
                Status = Status,
                Unlocked = Unlocked,
                Workers = Workers,
                RunMultiplier = saveRun,
                Focused = Focused,
                FailureHistory = FailureHistory ?? []
            };
        }
    }

    public string StartText => recipe.StartText;

    public void AddWorker(WorkerRecipe worker)
    {
        if (Workers.Count >= MaximumWorkers)
        {
            var temp = Workers.First();
            FreeWorker(temp);
            Workers.RemoveFirstItem();
        }
        worker.WorkerStatus = EnumWorkerStatus.Selected;
        worker.CurrentLocation = recipe.Location;
        Workers.Add(worker);
    }

    private static void FreeWorker(WorkerRecipe worker)
    {
        worker.WorkerStatus = EnumWorkerStatus.None;
        worker.CurrentLocation = NoneLocation;
    }

    public void RemoveWorker(WorkerRecipe worker)
    {
        FreeWorker(worker);
        Workers.RemoveSpecificItem(worker);
    }

    public BasicList<ItemAmount> SuppliesNeeded
    {
        get
        {
            BasicList<ItemAmount> output = [];
            foreach (var item in recipe.Inputs)
            {
                output.Add(new ItemAmount(item.Key, item.Value));
            }
            return output;
        }
    }
    public bool CanStartJob(InventoryManager inventory)
    {
        if (inventory.Has(recipe.Inputs) == false)
        {
            return false;
        }
        return Status == EnumWorksiteState.None; //you don't need workers anymore because if you have none before starting, can do automatically now.
    }

    public bool HadRewards => _rewards.Count > 0;
    public bool CanCollectRewards => Status == EnumWorksiteState.Collecting;

    public void StoreRewards(BasicList<ItemAmount> rewards)
    {
        _rewards = rewards;
    }

    private void AssignWorkersAutomatically()
    {
        int maxs = recipe.MaximumWorkers;
        //looks like locked locations feature requires rethinking eventually.
        var list = allWorkers.Where(x => x.WorkerStatus == EnumWorkerStatus.None || x.WorkerStatus == EnumWorkerStatus.Selected).ToBasicList();
        if (list.Count < maxs)
        {
            maxs = list.Count;
        }
        maxs.Times(x =>
        {
            AddWorker(list.First());
            list.RemoveFirstItem();
        });
    }
    public bool CanResetToFocused
    {
        get
        {
            if (Status != EnumWorksiteState.Processing)
            {
                return false;
            }
            if (Focused)
            {
                return false;
            }
            return recipe.CanFocus;
        }
    }
    public void ResetToFocused()
    {
        if (Status != EnumWorksiteState.Processing || StartedAt is null)
        {
            throw new CustomBasicException("Job must be processing.");
        }
        if (Focused)
        {
            return; //already focused.
        }
        Focused = true;
        _runMultiplier ??= _currentMultiplier;
        _runMultiplier *= 2.0;      // Apply() uses time * multiplier, so this doubles effective time
        StartedAt = DateTime.Now;   // waste elapsed time (your rule)
    }

    public void StartJob(InventoryManager inventory)
    {
        _rewards.Clear();
        if (CanStartJob(inventory) == false)
        {
            throw new CustomBasicException("Unable to start job. Should have called CanStartJob first");
        }

        if (Workers.Count == 0)
        {
            AssignWorkersAutomatically();
            if (Workers.Count == 0)
            {
                return; //do this to protect me from myself.    so if no workers, then don't do anything (because no workers anyways).
            }
        }

        foreach (var item in recipe.Inputs)
        {
            inventory.Consume(item.Key, item.Value);
        }

        Workers.ForEach(worker => worker.WorkerStatus = EnumWorkerStatus.Working);

        // LOCK promise at start
        _runMultiplier = _currentMultiplier;

        StartedAt = DateTime.Now;
        Status = EnumWorksiteState.Processing;
    }

    private static bool ShouldAward(double chance) => chance >= 1 || Random.Shared.NextDouble() <= chance;

    public void CollectSpecificReward(ItemAmount amount)
    {
        if (_rewards.Count == 0)
        {
            throw new CustomBasicException("No more reward left");
        }

        _rewards.RemoveAllAndObtain(x => x.Item == amount.Item);
        if (_rewards.Count == 0)
        {
            CollectAllRewards();
        }
    }

    public ItemAmount GetFirstReward
    {
        get
        {
            if (_rewards.Count == 0)
            {
                throw new CustomBasicException("No more reward left");
            }

            ItemAmount output = _rewards.First();
            _rewards.RemoveFirstItem();

            if (_rewards.Count == 0)
            {
                CollectAllRewards();
            }
            return output;
        }
    }
    private void IncrementFailure(string item)
    {
        if (FailureHistory.TryGetValue(item, out int times))
        {
            FailureHistory[item] = times + 1;
        }
        else
        {
            FailureHistory[item] = 1;
        }
        FailureHistory[item] = Math.Min(FailureHistory[item], 4); //after 4, don't increase anymore no matter what.
    }

    private void ClearFailure(string item)
    {
        // remove completely once it succeeds
        FailureHistory.Remove(item);
    }
    public BasicList<ItemAmount> GetRewards()
    {
        if (_rewards.Count > 0)
        {
            return _rewards.ToBasicList();
        }

        if (CanCollectRewards == false)
        {
            throw new CustomBasicException("Cannot collect rewards because there was none to collect.");
        }

        BasicList<WorksiteRewardPreview> list = GetPreview(false);
        BasicList<ItemAmount> output = [];
        foreach (var item in list)
        {
            if (ShouldAward(item.Chance))
            {
                output.Add(new(item.Item, item.Amount));
                // Always clear on success (even if we weren't tracking this item yet).
                // This keeps the rule "success resets pity" true even if tracking rules change later.
                ClearFailure(item.Item);
                //remove from failure history.
            }
            else if (item.Optional == false)
            {
                //add to the failure history (or increment it).
                IncrementFailure(item.Item);
            }
        }
        return output;
    }

    public void CollectAllRewards()
    {
        _rewards.Clear();

        foreach (var item in Workers)
        {
            FreeWorker(item);
        }
        Focused = false; //no longer focused.
        Workers.Clear();
        StartedAt = null;
        Status = EnumWorksiteState.None;

        // Run is over; clear promise
        _runMultiplier = null;
    }

    private bool _needsSaving;
    public bool NeedsSaving => _needsSaving;

    public void UpdateTick()
    {
        _needsSaving = false;

        if (Status != EnumWorksiteState.Processing || StartedAt is null)
        {
            return;
        }

        // Defensive: ensure promise exists
        _runMultiplier ??= _currentMultiplier;

        var elapsed = DateTime.Now - StartedAt.Value;
        if (elapsed >= EffectiveDuration)
        {
            Status = EnumWorksiteState.Collecting;
            StartedAt = null;
            _needsSaving = true;
        }
    }

    // --- Your existing reward preview logic unchanged below ---

    public BasicList<WorksiteRewardPreview> GetPreview(bool uiOnly = true)
    {
        BasicList<WorksiteRewardPreview> output = [];
        WorksiteRewardPreview preview;

        if (Workers.Count == 0)
        {
            foreach (var firsts in recipe.BaselineBenefits)
            {
                preview = new()
                {
                    Chance = 0,
                    Amount = firsts.Quantity,
                    Item = firsts.Item
                };
                output.Add(preview);
            }
            return output;
        }

        HashSet<string> candidateItems = [];

        foreach (var b in recipe.BaselineBenefits)
        {
            candidateItems.Add(b.Item);
        }
        if (Focused == false)
        {
            foreach (var worker in Workers)
            {
                foreach (var benefit in worker.Benefits)
                {
                    if (benefit.Worksite == recipe.Location)
                    {
                        candidateItems.Add(benefit.Item);
                    }
                }
            }
        }


        foreach (var review in candidateItems)
        {
            WorksiteBaselineBenefit? startBenefit = recipe.BaselineBenefits.SingleOrDefault(x => x.Item == review);
            BasicList<WorkerBenefit> workerBenefits = GetWorkerBenefits(review);
            if (workerBenefits.Count == 0 && startBenefit is null)
            {
                continue;
            }

            int amount = startBenefit?.Quantity ?? 0;

            if (workerBenefits.Any(x => x.GivesExtra))
            {
                amount++;
            }
            else if (startBenefit is not null)
            {
                if (startBenefit.EachWorkerGivesOne)
                {
                    amount = startBenefit.Quantity * Workers.Count;
                }
            }
            else
            {
                amount = 1;
            }

            double chances;
            if (startBenefit is not null && startBenefit.Guarantee)
            {
                chances = 1;
            }
            else if (workerBenefits.Any(x => x.Guarantee))
            {
                chances = 1;
            }
            else
            {
                double startBase = startBenefit?.Chance ?? 0;

                if (FailureHistory.TryGetValue(review, out int times))
                {
                    startBase += 0.08 * times;
                }

                double baseChance = startBase * Workers.Count;
                double extras = workerBenefits.Sum(x => x.ChanceModifier);
                chances = baseChance + extras;
            }

            if (chances > 1)
            {
                chances = 1;
            }

            if (chances == 0)
            {
                throw new CustomBasicException($"Must have at least a small chance or why bother including {review}");
            }
            if (Focused)
            {
                chances = 1; //make all guaranteed because you focused.
            }
            if (uiOnly)
            {
                chances *= 100;
            }
            bool optional;
            if (startBenefit is null)
            {
                optional = true;
            }
            else
            {
                optional = startBenefit.Optional;
            }

            preview = new()
            {
                Item = review,
                Amount = amount,
                Chance = chances,
                Optional = optional
            };
            output.Add(preview);
        }

        return output;
    }

    private BasicList<WorkerBenefit> GetWorkerBenefits(string item)
    {
        if (Focused)
        {
            return [];
        }
        BasicList<WorkerBenefit> output = [];
        Workers.ForConditionalItems(x => x.CurrentLocation == recipe.Location, worker =>
        {
            var seconds = worker.Benefits.Where(x => x.Item == item);
            output.AddRange(seconds);
        });
        return output;
    }
}
