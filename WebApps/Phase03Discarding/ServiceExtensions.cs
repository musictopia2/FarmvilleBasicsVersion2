using Phase03Discarding.Components.Custom; //for now.
using Phase03Discarding.DataAccess.Animals;
using Phase03Discarding.DataAccess.Balance;
using Phase03Discarding.DataAccess.Core;
using Phase03Discarding.DataAccess.Crops;
using Phase03Discarding.DataAccess.Quests; //not common enough.
using Phase03Discarding.DataAccess.Trees;
using Phase03Discarding.DataAccess.Workers;
using Phase03Discarding.DataAccess.Workshops;
using Phase03Discarding.DataAccess.Worksites;
//these was not common enough to put into global usings.

namespace Phase03Discarding;

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