namespace Phase01IntroduceBarnAndSilo.Services.Crops;
public interface ICropHarvestPolicy
{
    //can come from a database.  means one may choose to be automatic but another one manual (factories will return the proper ones).
    //if i do a database lookup, can later, choose that based on the values returned from a database, can be automatic vs manual
    Task<bool> IsAutomaticAsync(); //can't be property because possible async
    //for now, i am not ready to figure out if you have silo space. that is after mvp1.


    //bool CanHarvest(string output, Inventory inventory);
}