using Phase09LevelProgression.Components.Custom; //for now.
using Phase09LevelProgression.DataAccess.Animals;
using Phase09LevelProgression.DataAccess.Balance;
using Phase09LevelProgression.DataAccess.Core;
using Phase09LevelProgression.DataAccess.Crops;
using Phase09LevelProgression.DataAccess.Progression;
using Phase09LevelProgression.DataAccess.Quests; //not common enough.
using Phase09LevelProgression.DataAccess.Trees;
using Phase09LevelProgression.DataAccess.Upgrades;
using Phase09LevelProgression.DataAccess.Workers;
using Phase09LevelProgression.DataAccess.Workshops;
using Phase09LevelProgression.DataAccess.Worksites;
//these was not common enough to put into global usings.

namespace Phase09LevelProgression;

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