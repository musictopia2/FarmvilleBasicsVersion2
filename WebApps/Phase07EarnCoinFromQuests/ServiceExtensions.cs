using Phase07EarnCoinFromQuests.Components.Custom; //for now.
using Phase07EarnCoinFromQuests.DataAccess.Animals;
using Phase07EarnCoinFromQuests.DataAccess.Balance;
using Phase07EarnCoinFromQuests.DataAccess.Core;
using Phase07EarnCoinFromQuests.DataAccess.Crops;
using Phase07EarnCoinFromQuests.DataAccess.Quests; //not common enough.
using Phase07EarnCoinFromQuests.DataAccess.Trees;
using Phase07EarnCoinFromQuests.DataAccess.Upgrades;
using Phase07EarnCoinFromQuests.DataAccess.Workers;
using Phase07EarnCoinFromQuests.DataAccess.Workshops;
using Phase07EarnCoinFromQuests.DataAccess.Worksites;
//these was not common enough to put into global usings.

namespace Phase07EarnCoinFromQuests;

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