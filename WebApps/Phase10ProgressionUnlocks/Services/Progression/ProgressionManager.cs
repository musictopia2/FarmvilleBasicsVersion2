namespace Phase10ProgressionUnlocks.Services.Progression;
public class ProgressionManager(InventoryManager inventoryManager,
    CropManager cropManager,
    AnimalManager animalManager,
    TreeManager treeManager,
    WorksiteManager worksiteManager
    )
{
    private LevelProgressionPlanModel _levelPlan = null!;
    private CropProgressionPlanModel _cropPlan = null!;
    private BasicList<ItemUnlockRule> _animalPlan = null!;
    private BasicList<ItemUnlockRule> _treePlan = null!;
    private BasicList<ItemUnlockRule> _worksitePlan = null!;
    private BasicList<ItemUnlockRule> _workerPlan = null!;
    private ProgressionProfileModel _currentProfile = null!;
    private IProgressionProfile _profileService = null!;
    public event Action? Changed;
    private void NotifyChanged() => Changed?.Invoke();
    public async Task SetProgressionStyleContextAsync(ProgressionServicesContext context,
        FarmKey farm)
    {
        _levelPlan = await context.LevelProgressionPlanProvider.GetPlanAsync(farm);
        _currentProfile = await context.ProgressionProfile.LoadAsync();
        _profileService = context.ProgressionProfile;
        _cropPlan = await context.CropProgressionPlanProvider.GetPlanAsync(farm);
        _animalPlan = await context.AnimalProgressionPlanProvider.GetPlanAsync(farm);
        _treePlan = await context.TreeProgressionPlanProvider.GetPlanAsync(farm);
        _worksitePlan = await context.WorksiteProgressionPlanProvider.GetPlanAsync(farm);
        _workerPlan = await context.WorkerProgressionPlanProvider.GetPlanAsync(farm);
    }
    public int Level => _currentProfile.Level;
    public int CurrentPoints => _currentProfile.PointsThisLevel;
    public int PointsRequired
    {
        get
        {
            var profile = GetCurrentTier();
            return profile.RequiredPoints;
        }
    }
    public bool CompletedGame => _currentProfile.CompletedGame;
    public async Task IncrementLevelAsync()
    {
        if (CompletedGame)
        {
            return;
        }
        if (_levelPlan.IsEndless)
        {
            RewardEndOfLevel();
            _currentProfile.Level++;
            await ProcessUnlocksAsync();
            await SaveAsync();
            return;
        }
        if (_currentProfile.Level >= _levelPlan.Tiers.Count)
        {
            RewardEndOfLevel(); //if you ended game, no need to unlock anything.
            _currentProfile.CompletedGame = true;
            await SaveAsync();
            return;
        }
        RewardEndOfLevel();
        _currentProfile.Level++;
        await ProcessUnlocksAsync();
        await SaveAsync();
    }

    private async Task ProcessUnlocksAsync()
    {
        cropManager.ApplyCropProgressionUnlocks(_cropPlan, _currentProfile.Level); //new level.
        animalManager.ApplyAnimalProgressionUnlocks(_animalPlan, _currentProfile.Level);
        treeManager.ApplyTreeProgressionUnlocks(_treePlan, _currentProfile.Level);
        worksiteManager.ApplyWorksiteProgressionUnlocks(_worksitePlan, _currentProfile.Level);
        await worksiteManager.ApplyWorkerProgressionUnlocksAsync(_workerPlan, _currentProfile.Level);
    }

    private LevelProgressionTier GetCurrentTier()
    {
        if (_currentProfile.Level - 1 >= _levelPlan.Tiers.Count)
        {
            return _levelPlan.Tiers.Last();
        }
        return _levelPlan.Tiers[_currentProfile.Level - 1];
    }
    private async Task SaveAsync()
    {
        await _profileService.SaveAsync(_currentProfile);
        NotifyChanged();
    }

    

    private void RewardEndOfLevel()
    {
        _currentProfile.PointsThisLevel = 0;
        LevelProgressionTier tier = GetCurrentTier();
        
        inventoryManager.Add(tier.RewardsOnLevelComplete);
        //has to figure out how to communicate with the crop manager to get the data.
        //well see how this can work (?)
        //refer to how i upgraded workshop capacity for ideas.


    }
    private static int GetThresholdPoint(int requiredPoints, int percent)
    {
        if (percent <= 0 || percent >= 100)
        {
            throw new CustomBasicException("Milestone percent must be between 1 and 99.");
        }
        return (int)Math.Ceiling(requiredPoints * (percent / 100.0));
    }
    public string GetNextRewardDetails()
    {
        if (CompletedGame)
        {
            return "";
        }

        LevelProgressionTier tier = GetCurrentTier();
        int required = tier.RequiredPoints;
        int current = _currentProfile.PointsThisLevel;

        if (required <= 0)
        {
            return "";
        }

        // If no milestone rewards exist, next reward is level-up
        if (tier.ProgressMilestoneRewards is null || tier.ProgressMilestoneRewards.Count == 0)
        {
            int toLevel = Math.Max(0, required - current);
            return $"";
        }

        // Build ordered thresholds (lowest -> highest)
        var thresholds = tier.ProgressMilestoneRewards
            .Select(m => GetThresholdPoint(required, m.Percent))
            .Distinct()
            .OrderBy(x => x)
            .ToBasicList();

        // Next milestone is the smallest threshold strictly greater than current points
        int? nextMilestone = thresholds.FirstOrDefault(t => t > current);

        if (nextMilestone.HasValue && nextMilestone.Value > 0)
        {
            int toMilestone = nextMilestone.Value - current;
            return $"{toMilestone:N0} more points to next milestone reward";
        }

        // All milestones are already passed; next reward is level-up
        int toNextLevel = Math.Max(0, required - current);
        return $"";
    }
    public int PointsNeededToLevelUp
    {
        get
        {
            LevelProgressionTier tier = GetCurrentTier();
            return tier.RequiredPoints - _currentProfile.PointsThisLevel;
        }
    }
    public Dictionary<string, int> GetMilestoneRewards()
    {
        //may even be 0 (if no more left).
        LevelProgressionTier tier = GetCurrentTier();
        //var list = tier.ProgressMilestoneRewards.ord(x => x.Percent).ToBasicList();
        foreach (var milestone in tier.ProgressMilestoneRewards)
        {
            int thresholdPoint = GetThresholdPoint(tier.RequiredPoints, milestone.Percent);

            if (thresholdPoint > _currentProfile.PointsThisLevel)
            {
                return milestone.Rewards;
            }

            // Award ONLY if this point EXACTLY hits the threshold
            //if (_currentProfile.PointsThisLevel == thresholdPoint)
            //{
            //    inventoryManager.Add(milestone.Rewards);
            //    break; // guarantee only one milestone per point
            //}
        }
        return [];
    }
    public Dictionary<string, int> GetLevelRewards()
    {
        LevelProgressionTier tier = GetCurrentTier();
        return tier.RewardsOnLevelComplete;
    }
    public BasicList<int> GetCompleteThresholds()
    {
        LevelProgressionTier tier = GetCurrentTier();
        BasicList<int> output = [];
        foreach (var item in tier.ProgressMilestoneRewards)
        {
            output.Add(GetThresholdPoint(tier.RequiredPoints, item.Percent));
        }
        return output;
    }
    public async Task AddPointSinglePointAsync()
    {
        if (CompletedGame)
        {
            return;
        }
        LevelProgressionTier tier = GetCurrentTier();
        // 1. Increment by exactly one
        _currentProfile.PointsThisLevel++;

        // 2. Check for level completion FIRST
        if (_currentProfile.PointsThisLevel >= tier.RequiredPoints)
        {
            await IncrementLevelAsync();
            return;
        }
        // 3. Otherwise, check milestone rewards
        if (tier.ProgressMilestoneRewards.Count == 0)
        {
            await SaveAsync();
            return;
        }
        var list = tier.ProgressMilestoneRewards.OrderByDescending(x => x.Percent).ToBasicList();
        foreach (var milestone in list)
        {
            int thresholdPoint = GetThresholdPoint(tier.RequiredPoints, milestone.Percent);

            // Award ONLY if this point EXACTLY hits the threshold
            if (_currentProfile.PointsThisLevel == thresholdPoint)
            {
                inventoryManager.Add(milestone.Rewards);
                break; // guarantee only one milestone per point
            }
        }

        // 4. Persist profile
        await SaveAsync();
    }
}