namespace Phase09LevelProgression.Services.Progression;
public class ProgressionManager(InventoryManager inventoryManager)
{
    private LevelProgressionPlanModel _levelPlan = null!;
    private ProgressionProfileModel _currentProfile = null!;
    private IProgressionProfile _profileService = null!;
    public async Task SetProgressionStyleContextAsync(ProgressionServicesContext context,
        FarmKey farm)
    {
        _levelPlan = await context.LevelProgressionPlanProvider.GetPlanAsync(farm);
        _currentProfile = await context.ProgressionProfile.LoadAsync();
    }
    public int Level => _currentProfile.Level;
    public int CurrentPoints => _currentProfile.Level;
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
            await _profileService.SaveAsync(_currentProfile);
            return;
        }
        if (_currentProfile.Level + 1 >= _levelPlan.Tiers.Count)
        {
            RewardEndOfLevel();
            _currentProfile.CompletedGame = true;
            await _profileService.SaveAsync(_currentProfile);
            return;
        }
        RewardEndOfLevel();
        _currentProfile.Level++;
        await _profileService.SaveAsync(_currentProfile);
    }

    private LevelProgressionTier GetCurrentTier()
    {
        if (_currentProfile.Level - 1 >= _levelPlan.Tiers.Count)
        {
            return _levelPlan.Tiers.Last();
        }
        return _levelPlan.Tiers[_currentProfile.Level - 1];
    }

    private void RewardEndOfLevel()
    {
        _currentProfile.PointsThisLevel = 0;
        LevelProgressionTier tier = GetCurrentTier();
        
        inventoryManager.Add(tier.RewardsOnLevelComplete);
    }

    private static int GetThresholdPoint(int requiredPoints, int percent)
    {
        if (percent <= 0 || percent >= 100)
        {
            throw new CustomBasicException("Milestone percent must be between 1 and 99.");
        }

        return (int)Math.Ceiling(requiredPoints * (percent / 100.0));
    }

    public async Task AddPointSinglePointAsync()
    {
        if (CompletedGame)
        {
            return;
        }
        _currentProfile.PointsThisLevel++;
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
            await _profileService.SaveAsync(_currentProfile);
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
        await _profileService.SaveAsync(_currentProfile);
    }
}