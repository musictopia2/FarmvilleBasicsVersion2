namespace Phase02BarnAndSiloLimits.Services.Core;
public class GameRegistry
{
    private readonly BasicList<IGameTimer> _farms = [];
    public static TimeSpan SaveThrottle { get; set; } = TimeSpan.FromSeconds(2); //for testing.

    internal static double ValidateMultiplier(double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value) || value <= 0)
        {
            throw new CustomBasicException($"Time multiplier must be > 0 and finite. Value={value}");
        }

        // Your chosen guard rail: never slower than base
        if (value > 1.0)
        {
            throw new CustomBasicException($"Time multiplier must be <= 1.0. Value={value}");
        }

        return value;
    }

    public async Task InitializeFarmAsync(IGameTimer timer, FarmKey farm)
    {
        await timer.SetThemeContextAsync(farm);
        _farms.Add(timer);
    }
    public MainFarmContainer GetFarm(FarmKey currentFarm)
    {
        foreach (var farm in _farms)
        {
            CustomBasicException.ThrowIfNull(farm.FarmKey);
            CustomBasicException.ThrowIfNull(farm.FarmContainer);
            if (farm.FarmKey.Equals(currentFarm))
            {
                return farm.FarmContainer;
            }
        }
        throw new CustomBasicException($"No farm found for {currentFarm}");
    }
    public async Task TickAsync()
    {
        await _farms.ForEachAsync(async farm =>
        {
            await farm.TickAsync();
        });
    }
}
