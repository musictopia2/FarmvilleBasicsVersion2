namespace Phase04EnforcingLimits.Components.Custom;
public partial class CropsComponent
{
    private string? _selectedItem;
    private BasicList<string> _crops = [];

    // modal state
    private bool _showProgressModal;
    private Guid _progressCropId;

    private void SelectCrop(string id) => _selectedItem = id;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        _crops = CropManager.UnlockedRecipes;
    }
    private int InventoryAmount(string crop) => Inventory.GetInventoryCount(crop);
    private EnumCropState GetState(Guid id) => CropManager.GetCropState(id);

    private bool CanPlant(Guid id)
    {
        if (_selectedItem is null)
        {
            return false;
        }
        return CropManager.CanPlant(id, _selectedItem);
    }

    private void PlantSingle(Guid id)
    {
        if (_selectedItem is null)
        {
            return;
        }
        CropManager.Plant(id, _selectedItem);
    }

    private void PlantMax()
    {
        var list = CropManager.GetUnlockedCrops;
        foreach (var item in list)
        {
            var state = GetState(item);
            if (state != EnumCropState.Empty)
            {
                continue; //can only plant if its not empty.
            }
            if (CanPlant(item) == false)
            {
                continue;
            }
            PlantSingle(item);
        }
    }

    private void HarvestMax()
    {
        var list = CropManager.GetUnlockedCrops;
        foreach (var item in list)
        {
            var state = GetState(item);
            if (state !=  EnumCropState.Ready)
            {
                continue;
            }
            HarvestSingle(item); //for now, go ahead and harvest.  once i start getting into limits, then rethink.
        }
    }



    private void HarvestSingle(Guid id) => CropManager.Harvest(id);

    // The main UX router: card click always does the "right thing"
    private async Task OnCardClick(Guid id)
    {
        var state = GetState(id);

        if (state == EnumCropState.Empty)
        {
            if (CanPlant(id))
            {
                PlantSingle(id);
            }
            else
            {
                // Optional: could flash/select UI or show a tooltip-like message
                // If you have a toast system, trigger it here.
            }
            return;
        }

        if (state == EnumCropState.Ready)
        {
            HarvestSingle(id);
            return;
        }

        // Growing -> show modal for 5 seconds
        _progressCropId = id;
        _showProgressModal = true;
        StateHasChanged();

        await Task.Delay(5000);

        _showProgressModal = false;
        StateHasChanged();
    }

    // Image mapping: “root place with the same name as the name given”
    // Example assumes wwwroot/images/crops/{name}.png
    private string GetCropImageSrc(Guid id)
    {
        var name = CropManager.GetCropName(id);

        // If your filenames include spaces, you probably want to normalize:
        // e.g. "Green Beans" => "GreenBeans.png" or "green-beans.png"
        // If your real files match exactly, keep it simple:
        return $"/{name}.png";
    }
    private static string GetCropImageSrc(string name) => $"/{name}.png";
    private static string GetBadgeTitle(EnumCropState state) => state switch
    {
        EnumCropState.Empty => "Plant",
        EnumCropState.Growing => "In progress (click to view)",
        EnumCropState.Ready => "Ready (click to harvest)",
        _ => ""
    };

    private static string GetBackground(EnumCropState state) => state switch
    {
        EnumCropState.Empty => cc1.White.ToWebColor,
        EnumCropState.Growing => cc1.DarkGray.ToWebColor,
        EnumCropState.Ready => cc1.Lime.ToWebColor,
        _ => cc1.DarkGray.ToWebColor
    };

    private static RenderFragment BadgeSvg(EnumCropState state) => builder =>
    {
        var svg = state switch
        {
            EnumCropState.Empty =>
                @"<svg viewBox='0 0 24 24' width='36' height='36' aria-hidden='true'>
                <path d='M11 5h2v14h-2zM5 11h14v2H5z'/>
              </svg>",

            EnumCropState.Growing =>
                @"<svg viewBox='0 0 24 24' width='36' height='36' aria-hidden='true'>
                <path d='M6 2h12v4c0 2-2 4-4 6 2 2 4 4 4 6v4H6v-4c0-2 2-4 4-6-2-2-4-4-4-6V2zm2 2v2c0 1 2 3 4 5 2-2 4-4 4-5V4H8zm8 16v-2c0-1-2-3-4-5-2 2-4 4-4 5v2h8z'/>
              </svg>",

            EnumCropState.Ready =>
                @"<svg viewBox='0 0 24 24' width='36' height='36' aria-hidden='true'>
                <path d='M9.2 16.6 4.9 12.3l1.4-1.4 2.9 2.9 8.6-8.6 1.4 1.4z'/>
              </svg>",

            _ => ""
        };

        builder.AddMarkupContent(0, svg);
    };
}