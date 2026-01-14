namespace Phase12QuestsBasedOnLevel.Models;
public class AnimalInstanceDocument
{
    required public BasicList<AnimalAutoResumeModel> Animals { get; set; }
    required public FarmKey Farm { get; set; }
}