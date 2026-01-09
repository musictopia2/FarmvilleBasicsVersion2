using Phase04EnforcingLimits.Components.Custom; //for now.
using Phase04EnforcingLimits.DataAccess.Animals;
using Phase04EnforcingLimits.DataAccess.Balance;
using Phase04EnforcingLimits.DataAccess.Core;
using Phase04EnforcingLimits.DataAccess.Crops;
using Phase04EnforcingLimits.DataAccess.Quests; //not common enough.
using Phase04EnforcingLimits.DataAccess.Trees;
using Phase04EnforcingLimits.DataAccess.Workers;
using Phase04EnforcingLimits.DataAccess.Workshops;
using Phase04EnforcingLimits.DataAccess.Worksites;
//these was not common enough to put into global usings.

namespace Phase04EnforcingLimits;

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