namespace Phase04PrepareForMVP1.Models;
public class CropAutoResumeModel
{
    //try to do without id (?)

    //public Guid Id { get; set; } = Guid.NewGuid();  // Unique slot ID
    public string? Crop { get; set; }               // Crop planted (if any)
    public EnumCropState State { get; set; }       // Empty / Growing / Ready
    public DateTime? PlantedAt { get; set; } //when the crop was planted
    public bool Unlocked { get; set; } = true;             // Is this slot unlocked
    public double? RunMultiplier { get; set; }
}