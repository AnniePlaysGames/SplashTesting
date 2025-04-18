public interface IBuildingContext
{
    BuildingData Data { get; }
    System.Action OnCancel { get; }
}