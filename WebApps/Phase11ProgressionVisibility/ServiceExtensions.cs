using Phase11ProgressionVisibility.Components.Custom; //for now.
using Phase11ProgressionVisibility.DataAccess.Animals;
using Phase11ProgressionVisibility.DataAccess.Balance;
using Phase11ProgressionVisibility.DataAccess.Core;
using Phase11ProgressionVisibility.DataAccess.Crops;
using Phase11ProgressionVisibility.DataAccess.Progression;
using Phase11ProgressionVisibility.DataAccess.Quests; //not common enough.
using Phase11ProgressionVisibility.DataAccess.Trees;
using Phase11ProgressionVisibility.DataAccess.Upgrades;
using Phase11ProgressionVisibility.DataAccess.Workers;
using Phase11ProgressionVisibility.DataAccess.Workshops;
using Phase11ProgressionVisibility.DataAccess.Worksites;
//these was not common enough to put into global usings.

namespace Phase11ProgressionVisibility;

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