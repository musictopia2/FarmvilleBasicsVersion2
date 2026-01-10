using Phase06IncreaseBarnAndSiloLimits.Components.Custom; //for now.
using Phase06IncreaseBarnAndSiloLimits.DataAccess.Animals;
using Phase06IncreaseBarnAndSiloLimits.DataAccess.Balance;
using Phase06IncreaseBarnAndSiloLimits.DataAccess.Core;
using Phase06IncreaseBarnAndSiloLimits.DataAccess.Crops;
using Phase06IncreaseBarnAndSiloLimits.DataAccess.Quests; //not common enough.
using Phase06IncreaseBarnAndSiloLimits.DataAccess.Trees;
using Phase06IncreaseBarnAndSiloLimits.DataAccess.Upgrades;
using Phase06IncreaseBarnAndSiloLimits.DataAccess.Workers;
using Phase06IncreaseBarnAndSiloLimits.DataAccess.Workshops;
using Phase06IncreaseBarnAndSiloLimits.DataAccess.Worksites;
//these was not common enough to put into global usings.

namespace Phase06IncreaseBarnAndSiloLimits;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection RegisterFarmServices()
        {
            services.AddHostedService<GameTimerService>()
                .AddSingleton<GameRegistry>()
                .AddSingleton<IInventoryRepository, InventoryStockDatabase>()
                .AddSingleton<IStartFarmRegistry, StartFarmDatabase>()
                .AddSingleton<IInventoryFactory, InventoryFactory>()
                .AddSingleton<ITreeFactory, TreeFactory>()
                .AddSingleton<ICropFactory, CropFactory>()
                .AddSingleton<IAnimalFactory, AnimalFactory>()
                .AddSingleton<IWorkshopFactory, WorkshopFactory>()
                .AddSingleton<IWorksiteFactory, WorksiteFactory>()
                .AddSingleton<IWorkerFactory, WorkerFactory>()
                .AddSingleton<IQuestFactory, QuestFactory>()
                .AddSingleton<IUpgradeFactory, UpgradeFactory>()
                .AddScoped<ReadyStatusService>()
                .AddScoped<OverlayService>()
                .AddScoped<FarmContext>()
                .AddSingleton<IBaseBalanceProvider, BalanceProfileDatabase>() //i think this is safe this time (refer to inventory persistence)
                ;
            return services;
        }
        //not sure about quests.  not until near the end
    }
}