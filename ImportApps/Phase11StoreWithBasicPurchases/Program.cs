mm1.Prep();
await ii1.ImportStartClass.ImportStartAsync(); //done
await ii1.ImportCatalogOfferClass.ImportCatalogAsync();
await ii1.ImportInventoryStorageUpgradesClass.ImportInventoryStorageUpgradesAsync(); //this must be first now.  like the old recipes.
await ii1.ImportInventoryStorageProfileClass.ImportInventoryProfilesAsync();
await ii1.ImportLevelProgressionClass.ImportProgressionAsync();
await ii1.ImportProgressionProfileClass.ImportProgressionAsync();
await ii1.ImportCropRecipeClass.ImportCropsAsync();
await ii1.ImportCropProgressionClass.ImportCropsAsync();
await ii1.ImportCropInstanceClass.ImportCropsAsync(); //done
await ii1.ImportInventoryStockClass.ImportBeginningInventoryAmountsAsync(); //done
await ii1.ImportTreeRecipeClass.ImportTreesAsync();
await ii1.ImportTreeInstanceClass.ImportTreesAsync(); //done
await ii1.ImportAnimalRecipeClass.ImportAnimalsAsync();
await ii1.ImportAnimalProgressionClass.ImportAnimalsAsync();
await ii1.ImportAnimalInstanceClass.ImportAnimalsAsync(); //done
await ii1.ImportWorkshopRecipeClass.ImportWorkshopsAsync();
await ii1.ImportWorkshopCapacityUpgradesClass.ImportWorkshopsAsync();
await ii1.ImportWorkshopProgressionClass.ImportWorkshopsAsync();
await ii1.ImportWorkshopInstanceClass.ImportWorkshopsAsync(); //done
await ii1.ImportWorksiteRecipeClass.ImportWorksitesAsync();
await ii1.ImportWorksiteInstanceClass.ImportWorksitesAsync(); //done
await ii1.ImportWorkerRecipeClass.ImportWorkersAsync(); //done
await ii1.ImportWorkerInstanceClass.ImportWorkersAsync();
await ii1.ImportBalanceMultiplierClass.ImportBalanceMultiplierAsync(); //done

await ii1.ImportQuestInstancesClass.ImportQuestsAsync(); //iffy
Console.WriteLine("Completed");