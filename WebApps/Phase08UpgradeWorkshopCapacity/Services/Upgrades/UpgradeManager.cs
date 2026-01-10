namespace Phase08UpgradeWorkshopCapacity.Services.Upgrades;
public class UpgradeManager(InventoryManager inventoryManager,
    IInventoryProfile inventoryProfile
    )
{
    //this focuses on upgrades but are cross cutting.

    //this is different because no tick.

    private InventoryStorageUpgradePlanModel _inventoryPlan = null!;
    private InventoryStorageProfileModel _inventoryStorageProfile = null!;
    public async Task SetInventoryStyleContextAsync(UpgradeServicesContext context,
        InventoryStorageProfileModel storage
        , FarmKey farm)
    {
        //this is where i set everything up.
        //start out with just upgrades to the barn/silo.  later will upgrade other things.
        _inventoryPlan = await context.InventoryStorageUpgradePlanProvider.GetPlanAsync(farm);
        _inventoryStorageProfile = storage;


    }

    //private int CoinAmount => inventory.CoinCount;

    
    //if this does not work, then rethink.
    public int NextBarnCoinCost
    {
        get
        {
            var temp = _inventoryPlan.BarnUpgrades[_inventoryStorageProfile.BarnLevel + 1];
            return GetCoinCost(temp);
        }
    }
    private static int GetCoinCost(UpgradeTier tier)
    {
        if (tier.Cost.Count > 1)
        {
            throw new CustomBasicException("Not Just Coin");
        }
        if (tier.Cost.Single().Key != CurrencyKeys.Coin)
        {
            throw new CustomBasicException("This was not coin");
        }
        return tier.Cost.Single().Value;
    }
    public int NextSiloCoinCost
    {
        get
        {
            var temp = _inventoryPlan.SiloUpgrades[_inventoryStorageProfile.SiloLevel + 1];
            return GetCoinCost(temp);
        }
    }
    public int NextBarnCount
    {
        get
        {
            var temp = _inventoryPlan.BarnUpgrades[_inventoryStorageProfile.BarnLevel + 1];
            return temp.Size;
        }
    }
    public int NextSiloCount
    {
        get
        {
            var temp = _inventoryPlan.SiloUpgrades[_inventoryStorageProfile.SiloLevel + 1];
            return temp.Size;
        }
    }
    public bool CanUpgradeBarn
    {
        get
        {
            if (_inventoryStorageProfile.BarnLevel >= _inventoryPlan.BarnUpgrades.Count - 1)
            {
                return false;
            }
            var temp = _inventoryPlan.BarnUpgrades[_inventoryStorageProfile.BarnLevel + 1];


            return CanAfford(temp);

            //cost = _inventoryPlan.BarnUpgrades[_inventoryStorageProfile.BarnLevel].Cost;
        }
    }
    public async Task UpgradeBarnAsync()
    {
        if (CanUpgradeBarn == false)
        {
            throw new CustomBasicException("Cannot upgrade the barn.  should had ran CanUpgradeBarn");
        }
        //do a lookup to see what you can do now
        var temp = _inventoryPlan.BarnUpgrades[_inventoryStorageProfile.BarnLevel + 1];
        _inventoryStorageProfile.BarnLevel++;
        _inventoryStorageProfile.BarnSize = temp.Size;

        inventoryManager.Consume(temp.Cost);
        inventoryManager.UpdateInventoryProfile(_inventoryStorageProfile);
        await inventoryProfile.SaveAsync(_inventoryStorageProfile);
    }

    private bool CanAfford(UpgradeTier tier)
    {
        foreach (var item in tier.Cost)
        {
            int has = inventoryManager.Get(item.Key);
            int required = item.Value;
            if ( required > has )
            {
                return false;
            }
        }
        return true;
    }

    public bool CanUpgradeSilo
    {
        get
        {
            if (_inventoryStorageProfile.SiloLevel >= _inventoryPlan.SiloUpgrades.Count - 1)
            {
                return false;
            }
            var temp = _inventoryPlan.SiloUpgrades[_inventoryStorageProfile.SiloLevel + 1];
            return CanAfford(temp);
        }
    }
    public async Task UpgradeSiloAsync()
    {
        if (CanUpgradeSilo == false)
        {
            throw new CustomBasicException("Cannot upgrade the silo.  should had ran CanUpgradeSilo");
        }
        //do a lookup to see what you can do now
        var temp = _inventoryPlan.SiloUpgrades[_inventoryStorageProfile.SiloLevel + 1];
        _inventoryStorageProfile.SiloLevel++;
        _inventoryStorageProfile.SiloSize = temp.Size;
        inventoryManager.Consume(temp.Cost);
        inventoryManager.UpdateInventoryProfile(_inventoryStorageProfile); //just in case.
        await inventoryProfile.SaveAsync(_inventoryStorageProfile);
    }
}