namespace Phase09LevelProgression.DataAccess.Workers;
public class BasicWorkerInstances(IWorkerRegistry registry) : IWorkerInstances
{
    async Task<BasicList<WorkerDataModel>> IWorkerInstances.GetWorkerInstancesAsync()
    {
        var list = await registry.GetWorkersAsync();
        BasicList<WorkerDataModel> output = [];
        foreach (var item in list)
        {
            output.Add(new()
            {
                Name = item.WorkerName,
                Unlocked = true
            });
        }
        return output;
    }
}