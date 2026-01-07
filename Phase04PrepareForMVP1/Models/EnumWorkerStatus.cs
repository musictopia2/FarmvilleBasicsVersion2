namespace Phase04PrepareForMVP1.Models;
public enum EnumWorkerStatus
{
    None,
    Selected,
    Working //if they are working, they can't be on another site.
}