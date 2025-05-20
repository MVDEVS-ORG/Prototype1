using UnityEngine;

public enum BuildingTypes
{
    Resource,
    Barracks,
    Defense
}

public enum BuildingState
{
    Ruined,
    Completed
}
public interface IBuildingSystem
{
    int BuildCost { get; }
    int UpgradeCost { get; }
    bool CanBeUpgraded {  get; }
    BuildingState State { get; }
    BuildingTypes BuildingType { get; }
    void UpgradeBuilding();
}
