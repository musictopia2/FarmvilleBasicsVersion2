using Phase02BarnAndSiloLimits.Components.Custom; //for now.
using Phase02BarnAndSiloLimits.DataAccess.Animals;
using Phase02BarnAndSiloLimits.DataAccess.Balance;
using Phase02BarnAndSiloLimits.DataAccess.Core;
using Phase02BarnAndSiloLimits.DataAccess.Crops;
using Phase02BarnAndSiloLimits.DataAccess.Quests; //not common enough.
using Phase02BarnAndSiloLimits.DataAccess.Trees;
using Phase02BarnAndSiloLimits.DataAccess.Workers;
using Phase02BarnAndSiloLimits.DataAccess.Workshops;
using Phase02BarnAndSiloLimits.DataAccess.Worksites;
//these was not common enough to put into global usings.

namespace Phase02BarnAndSiloLimits;

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