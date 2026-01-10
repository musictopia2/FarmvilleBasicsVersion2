using Phase08UpgradeWorkshopCapacity.Components.Custom; //for now.
using Phase08UpgradeWorkshopCapacity.DataAccess.Animals;
using Phase08UpgradeWorkshopCapacity.DataAccess.Balance;
using Phase08UpgradeWorkshopCapacity.DataAccess.Core;
using Phase08UpgradeWorkshopCapacity.DataAccess.Crops;
using Phase08UpgradeWorkshopCapacity.DataAccess.Quests; //not common enough.
using Phase08UpgradeWorkshopCapacity.DataAccess.Trees;
using Phase08UpgradeWorkshopCapacity.DataAccess.Upgrades;
using Phase08UpgradeWorkshopCapacity.DataAccess.Workers;
using Phase08UpgradeWorkshopCapacity.DataAccess.Workshops;
using Phase08UpgradeWorkshopCapacity.DataAccess.Worksites;
//these was not common enough to put into global usings.

namespace Phase08UpgradeWorkshopCapacity;

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