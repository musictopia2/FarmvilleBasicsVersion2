mm1.Prep();
await ii1.ImportInventoryClass.ImportBeginningInventoryAmountsAsync(); //done
await ii1.ImportCropRecipeClass.ImportCropsAsync();
await ii1.ImportCropInstanceClass.ImportCropsAsync(); //done
await ii1.ImportTreeRecipeClass.ImportTreesAsync();
await ii1.ImportTreeInstanceClass.ImportTreesAsync(); //done
await ii1.ImportAnimalRecipeClass.ImportAnimalsAsync();
await ii1.ImportAnimalInstanceClass.ImportAnimalsAsync(); //done
await ii1.ImportWorkshopRecipeClass.ImportWorkshopsAsync();
await ii1.ImportWorkshopInstanceClass.ImportWorkshopsAsync(); //done
await ii1.ImportWorksiteRecipeClass.ImportWorksitesAsync();
await ii1.ImportWorksiteInstancesClass.ImportWorksitesAsync(); //done
await ii1.ImportWorkerRecipeClass.ImportWorkersAsync(); //done
await ii1.ImportStartClass.ImportStartAsync(); //done
await ii1.ImportBalanceMultiplierClass.ImportBalanceMultiplierAsync(); //done
await ii1.ImportQuestInstancesClass.ImportQuestsAsync(); //iffy
Console.WriteLine("Completed");