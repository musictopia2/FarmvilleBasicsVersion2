using Phase10ProgressionUnlocks.Components.Custom; //for now.
using Phase10ProgressionUnlocks.DataAccess.Animals;
using Phase10ProgressionUnlocks.DataAccess.Balance;
using Phase10ProgressionUnlocks.DataAccess.Core;
using Phase10ProgressionUnlocks.DataAccess.Crops;
using Phase10ProgressionUnlocks.DataAccess.Progression;
using Phase10ProgressionUnlocks.DataAccess.Quests; //not common enough.
using Phase10ProgressionUnlocks.DataAccess.Trees;
using Phase10ProgressionUnlocks.DataAccess.Upgrades;
using Phase10ProgressionUnlocks.DataAccess.Workers;
using Phase10ProgressionUnlocks.DataAccess.Workshops;
using Phase10ProgressionUnlocks.DataAccess.Worksites;
//these was not common enough to put into global usings.

namespace Phase10ProgressionUnlocks;

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