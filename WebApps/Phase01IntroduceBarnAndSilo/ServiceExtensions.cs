using Phase01IntroduceBarnAndSilo.Components.Custom; //for now.
using Phase01IntroduceBarnAndSilo.DataAccess.Animals;
using Phase01IntroduceBarnAndSilo.DataAccess.Balance;
using Phase01IntroduceBarnAndSilo.DataAccess.Core;
using Phase01IntroduceBarnAndSilo.DataAccess.Crops;
using Phase01IntroduceBarnAndSilo.DataAccess.Quests; //not common enough.
using Phase01IntroduceBarnAndSilo.DataAccess.Trees;
using Phase01IntroduceBarnAndSilo.DataAccess.Workers;
using Phase01IntroduceBarnAndSilo.DataAccess.Workshops;
using Phase01IntroduceBarnAndSilo.DataAccess.Worksites;
//these was not common enough to put into global usings.

namespace Phase01IntroduceBarnAndSilo;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection RegisterFarmServices()
        {
            services.AddHostedService<GameTimerService>()
                .AddSingleton<GameRegistry>()
                .AddSingleton<IInventoryRepository, InventoryDatabase>()
                .AddSingleton<IStartFarmRegistry, StartFarmDatabase>()
                .AddSingleton<IStartingFactory, StartingFactory>()
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