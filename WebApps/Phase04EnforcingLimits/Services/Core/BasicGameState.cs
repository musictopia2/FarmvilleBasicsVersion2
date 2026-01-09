namespace Phase04EnforcingLimits.Services.Core;
public class BasicGameState : IGameTimer
{
    public BasicGameState(InventoryManager inventory,
    IInventoryFactory startFactory,
    ICropFactory cropFactory,
    ITreeFactory treeFactory,
    IAnimalFactory animalFactory,
    IWorkshopFactory workshopFactory,
    IWorksiteFactory worksiteFactory,
    IWorkerFactory workerFactory,
    IQuestFactory questFactory,
    CropManager cropManager,
    TreeManager treeManager,
    AnimalManager animalManager,
    WorkshopManager workshopManager,
    WorksiteManager worksiteManager,
    QuestManager questManager
)
    {
        _inventory = inventory;
        _startFactory = startFactory;
        _cropFactory = cropFactory;
        _treeFactory = treeFactory;
        _animalFactory = animalFactory;
        _workshopFactory = workshopFactory;
        _worksiteFactory = worksiteFactory;
        _workerFactory = workerFactory;
        _questFactory = questFactory;
        _cropManager = cropManager;
        _treeManager = treeManager;
        _animalManager = animalManager;
        _workshopManager = workshopManager;
        _worksiteManager = worksiteManager;
        _questManager = questManager;
        _container = new MainFarmContainer
        {
            InventoryManager = inventory,
            CropManager = cropManager,
            TreeManager = treeManager,
            AnimalManager = animalManager,
            WorkshopManager = workshopManager,
            WorksiteManager = worksiteManager,
            QuestManager = questManager
        };
    }
    readonly MainFarmContainer _container;
    private readonly InventoryManager _inventory;
    private readonly IInventoryFactory _startFactory;
    private readonly ICropFactory _cropFactory;
    private readonly ITreeFactory _treeFactory;
    private readonly IAnimalFactory _animalFactory;
    private readonly IWorkshopFactory _workshopFactory;
    private readonly IWorksiteFactory _worksiteFactory;
    private readonly IWorkerFactory _workerFactory;
    private readonly IQuestFactory _questFactory;
    private readonly CropManager _cropManager;
    private readonly TreeManager _treeManager;
    private readonly AnimalManager _animalManager;
    private readonly WorkshopManager _workshopManager;
    private readonly WorksiteManager _worksiteManager;
    private readonly QuestManager _questManager;
    private FarmKey? _farm;
    FarmKey? IGameTimer.FarmKey => _farm;
    MainFarmContainer IGameTimer.FarmContainer
    {
        get
        {
            return _container;
        }
    }
    private bool _init = false;
    async Task IGameTimer.SetThemeContextAsync(FarmKey farm)
    {
        if (string.IsNullOrWhiteSpace(farm.PlayerName) || string.IsNullOrWhiteSpace(farm.Theme))
        {
            throw new CustomBasicException("Must specify player and farm themes now");
        }
        _farm = farm;
        IInventoryRepository init = _startFactory.GetInventoryServices(farm);
        IInventoryProfile inventoryProfileService = _startFactory.GetInventoryProfile(farm);
        Dictionary<string, int> starts = await init.LoadAsync(farm);
        InventoryStorageProfileModel inventoryStorageProfileModel = await inventoryProfileService.LoadAsync(farm);
        _inventory.LoadStartingInventory(starts, inventoryStorageProfileModel);
        CropServicesContext cropContext = _cropFactory.GetCropServices(farm);
        await _cropManager.SetStyleContextAsync(cropContext, farm);
        TreeServicesContext treeContext = _treeFactory.GetTreeServices(farm);
        await _treeManager.SetStyleContextAsync(treeContext, farm);
        AnimalServicesContext animalContext = _animalFactory.GetAnimalServices(farm);
        await _animalManager.SetStyleContextAsync(animalContext, farm);
        WorkshopServicesContext workshopContext = _workshopFactory.GetWorkshopServices(farm);
        await _workshopManager.SetStyleContextAsync(workshopContext, farm);
        WorksiteServicesContext worksiteContext = _worksiteFactory.GetWorksiteServices(farm);
        WorkerServicesContext workerContext = _workerFactory.GetWorkerServices(farm);
        await _worksiteManager.SetStyleContextAsync(worksiteContext, workerContext, farm);
        QuestServicesContext questContext = _questFactory.GetQuestServices(farm);
        await _questManager.SetStyleContextAsync(questContext);
        _init = true;
    }

    async Task IGameTimer.TickAsync()
    {
        if (_init == false)
        {
            return;
        }
        await _treeManager.UpdateTickAsync();
        await _cropManager.UpdateTickAsync();
        await _animalManager.UpdateTickAsync();
        await _workshopManager.UpdateTickAsync();
        await _worksiteManager.UpdateTickAsync();
    }
}