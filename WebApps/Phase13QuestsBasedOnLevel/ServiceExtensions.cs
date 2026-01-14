using Phase13QuestsBasedOnLevel.Components.Custom; //for now.
using Phase13QuestsBasedOnLevel.DataAccess.Animals;
using Phase13QuestsBasedOnLevel.DataAccess.Balance;
using Phase13QuestsBasedOnLevel.DataAccess.Catalog;
using Phase13QuestsBasedOnLevel.DataAccess.Core;
using Phase13QuestsBasedOnLevel.DataAccess.Crops;
using Phase13QuestsBasedOnLevel.DataAccess.Progression;
using Phase13QuestsBasedOnLevel.DataAccess.Quests; //not common enough.
using Phase13QuestsBasedOnLevel.DataAccess.Store;
using Phase13QuestsBasedOnLevel.DataAccess.Trees;
using Phase13QuestsBasedOnLevel.DataAccess.Upgrades;
using Phase13QuestsBasedOnLevel.DataAccess.Workers;
using Phase13QuestsBasedOnLevel.DataAccess.Workshops;
using Phase13QuestsBasedOnLevel.DataAccess.Worksites;
//these was not common enough to put into global usings.

namespace Phase13QuestsBasedOnLevel;

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
                .AddSingleton<IProgressionFactory, ProgressionFactory>()
                .AddSingleton<ICatalogFactory, CatalogFactory>()
                .AddSingleton<IStoreFactory, StoreFactory>()
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