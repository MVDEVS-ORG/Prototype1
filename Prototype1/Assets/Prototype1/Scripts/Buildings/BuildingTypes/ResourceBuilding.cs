using System.Collections;
using UnityEngine;

namespace Assets.Prototype1.Scripts.Buildings
{
    public enum ResourceBuildingType
    {
        House,
        Farm,
        Mine
    }

    public class ResourceBuilding : Building
    {
        public ResourceBuildingType ResourceType;
        private int resourcePerDay;
        
        private void Start()
        {
            BuildingType = BuildingTypes.Resource;
            SetInitialValues();
        }

        void SetInitialValues()
        {
            switch (ResourceType)
            {
                case ResourceBuildingType.House:
                    BuildCost = 5;
                    UpgradeCost = 7;
                    resourcePerDay = 2;
                    break;
                case ResourceBuildingType.Farm:
                    BuildCost = 8;
                    UpgradeCost = 9;
                    resourcePerDay = 3;
                    break;
                case ResourceBuildingType.Mine:
                    BuildCost = 6;
                    UpgradeCost = 10;
                    resourcePerDay = 5;
                    break;
            }
            CanBeUpgraded = true;
        }

        public override void UpgradeBuilding()
        {
            if (CanBeUpgraded && State == BuildingState.Completed)
            {
                currentUpgrade++;
                UpgradeCost += 10;
                resourcePerDay += 2;
                Debug.Log($"{ResourceType} upgraded! Now generates {resourcePerDay} per Day.");
                if (currentUpgrade == 3) CanBeUpgraded = false;
            }
            else
            {
                Debug.LogError("Can not Upgrade this building anymore (Reached Max Level)");
                return;
            }
        }

        public int GenerateResources()
        {
            return resourcePerDay;
        }

        public override void TakeDamage(int damage)
        {
            if (State != BuildingState.Completed) return;

            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                State = BuildingState.Ruined;
                m_Renderer.material.color = Color.gray;
                Debug.Log($"{ResourceType} destroyed!");
            }
        }
    }
}