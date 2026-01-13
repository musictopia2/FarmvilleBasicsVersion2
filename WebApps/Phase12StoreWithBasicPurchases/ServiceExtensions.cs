using Phase12StoreWithBasicPurchases.Components.Custom; //for now.
using Phase12StoreWithBasicPurchases.DataAccess.Animals;
using Phase12StoreWithBasicPurchases.DataAccess.Balance;
using Phase12StoreWithBasicPurchases.DataAccess.Catalog;
using Phase12StoreWithBasicPurchases.DataAccess.Core;
using Phase12StoreWithBasicPurchases.DataAccess.Crops;
using Phase12StoreWithBasicPurchases.DataAccess.Progression;
using Phase12StoreWithBasicPurchases.DataAccess.Quests; //not common enough.
using Phase12StoreWithBasicPurchases.DataAccess.Store;
using Phase12StoreWithBasicPurchases.DataAccess.Trees;
using Phase12StoreWithBasicPurchases.DataAccess.Upgrades;
using Phase12StoreWithBasicPurchases.DataAccess.Workers;
using Phase12StoreWithBasicPurchases.DataAccess.Workshops;
using Phase12StoreWithBasicPurchases.DataAccess.Worksites;
//these was not common enough to put into global usings.

namespace Phase12StoreWithBasicPurchases;

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