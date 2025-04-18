public class BuildingContext : IBuildingContext
{
    public BuildingData Data { get; }
    public System.Action OnCancel { get; }

    public BuildingContext(BuildingData data, System.Action onCancel)
    {
        Data = data;
        OnCancel = onCancel;
    }
}