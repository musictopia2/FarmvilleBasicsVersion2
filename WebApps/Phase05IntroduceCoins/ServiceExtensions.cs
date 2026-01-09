using Phase05IntroduceCoins.Components.Custom; //for now.
using Phase05IntroduceCoins.DataAccess.Animals;
using Phase05IntroduceCoins.DataAccess.Balance;
using Phase05IntroduceCoins.DataAccess.Core;
using Phase05IntroduceCoins.DataAccess.Crops;
using Phase05IntroduceCoins.DataAccess.Quests; //not common enough.
using Phase05IntroduceCoins.DataAccess.Trees;
using Phase05IntroduceCoins.DataAccess.Workers;
using Phase05IntroduceCoins.DataAccess.Workshops;
using Phase05IntroduceCoins.DataAccess.Worksites;
//these was not common enough to put into global usings.

namespace Phase05IntroduceCoins;

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