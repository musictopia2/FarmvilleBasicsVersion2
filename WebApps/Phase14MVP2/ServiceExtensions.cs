using Phase14MVP2.Components.Custom; //for now.
using Phase14MVP2.DataAccess.Animals;
using Phase14MVP2.DataAccess.Balance;
using Phase14MVP2.DataAccess.Catalog;
using Phase14MVP2.DataAccess.Core;
using Phase14MVP2.DataAccess.Crops;
using Phase14MVP2.DataAccess.Items;
using Phase14MVP2.DataAccess.Progression;
using Phase14MVP2.DataAccess.Quests; //not common enough.
using Phase14MVP2.DataAccess.Store;
using Phase14MVP2.DataAccess.Trees;
using Phase14MVP2.DataAccess.Upgrades;
using Phase14MVP2.DataAccess.Workers;
using Phase14MVP2.DataAccess.Workshops;
using Phase14MVP2.DataAccess.Worksites;
//these was not common enough to put into global usings.

namespace Phase14MVP2;

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
                .AddSingleton<IItemFactory, ItemFactory>()
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