using prototype1.scripts.systems;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Prototype1.Scripts.Buildings
{
    public enum BarracksUpgradeType
    {
        Gunner,
        Sniper,
        Artillery
    }

    public class BarracksBuilding : Building
    {
        public BarracksUpgradeType CurrentType;

        [Header("Unit Prefabs")]
        public GameObject gunnerPrefab;
        public GameObject sniperPrefab;
        public GameObject artilleryPrefab;

        [Header("Spawn Settings")]
        public Transform spawnPoint;
        public float spawnRadius = 2f;

        private List<GameObject> spawnedUnits = new();

        private float troopCheckInterval = 5f;
        private float lastCheckTime;
        private HealthSystem _selfHealthSystem;

        private void Start()
        {
            _selfHealthSystem = GetComponent<HealthSystem>();
            _selfHealthSystem.OnZeroHealth += Die;
            BuildingType = BuildingTypes.Barracks;
            maxUpgradeLimit = 3;
            SetInitialValues();
        }

        void SetInitialValues()
        {
            BuildCost = 10;
            UpgradeCost = 15;
            CurrentType = BarracksUpgradeType.Gunner;
            CanBeUpgraded = true;
            (_selfHealthSystem as IHealthSystem).ResetHealth();
        }

        public override void Build()
        {
            base.Build();
            SpawnUnits(CurrentType); // Spawn 10 gunners on initial build
        }

        public override void UpgradeBuilding()
        {
            if (CanBeUpgraded && State == BuildingState.Completed)
            {
                currentUpgrade++;
                UpgradeCost += 10;

                if (CurrentType == BarracksUpgradeType.Gunner)
                {
                    CurrentType = BarracksUpgradeType.Sniper;
                    SpawnUnits(CurrentType); // Spawn 6 snipers
                }
                else if (CurrentType == BarracksUpgradeType.Sniper)
                {
                    CurrentType = BarracksUpgradeType.Artillery;
                    SpawnUnits(CurrentType); // Spawn 3 artillery
                }

                Debug.Log($"Barracks upgraded to {CurrentType}");

                if (currentUpgrade >= maxUpgradeLimit)
                {
                    CanBeUpgraded = false;
                }
            }
            else
            {
                Debug.LogError("Cannot upgrade Barracks anymore (Reached Max Level)");
                return;
            }
        }

        void SpawnUnits(BarracksUpgradeType type, int count = -1)
        {
            if (count == -1)
                count = GetDesiredUnitCount(type);

            GameObject unitPrefab = GetUnitPrefab(type);

            if (unitPrefab == null)
            {
                Debug.LogWarning("No prefab assigned for unit troopType: " + type);
                return;
            }

            for (int i = 0; i < count; i++)
            {
                Vector3 spawnPos = (spawnPoint != null) ? spawnPoint.position : transform.position;
                spawnPos += Random.insideUnitSphere * spawnRadius;
                spawnPos.y = transform.position.y;

                GameObject newUnit = Instantiate(unitPrefab, spawnPos, Quaternion.identity);
                newUnit.name = type.ToString() + "_Unit"; // Helpful for identifying troopType
                spawnedUnits.Add(newUnit);
            }

            Debug.Log($"{count} {type} units spawned.");
        }

        public void CheckAndMaintainTroops()
        {
            CleanupDeadUnits();


            foreach (BarracksUpgradeType type in System.Enum.GetValues(typeof(BarracksUpgradeType)))
            {
                int currentCount = GetActiveUnitCountForType(type);
                int desiredCount = GetDesiredUnitCount(type);
                int unitsToSpawn = desiredCount - currentCount;

                if (unitsToSpawn > 0)
                {
                    Debug.Log($"Maintaining {type} troops. Spawning {unitsToSpawn} more.");
                    SpawnUnits(type, unitsToSpawn);
                }
            }
        }

        void CleanupDeadUnits()
        {
            spawnedUnits.RemoveAll(unit => unit == null);
        }

        int GetActiveUnitCountForType(BarracksUpgradeType type)
        {
            int count = 0;
            foreach (var unit in spawnedUnits)
            {
                if (unit == null) continue;
                var troopType = unit.GetComponent<Troop>().troopType;
                if ((type == BarracksUpgradeType.Gunner && troopType == TroopType.Gunner) ||
                    (type == BarracksUpgradeType.Sniper && troopType == TroopType.Sniper) ||
                    (type == BarracksUpgradeType.Artillery && troopType == TroopType.Artillery))
                {
                    count++;
                }
            }
            return count;
        }

        int GetDesiredUnitCount(BarracksUpgradeType type)
        {
            switch (type)
            {
                case BarracksUpgradeType.Gunner: return 10;
                case BarracksUpgradeType.Sniper: return 6;
                case BarracksUpgradeType.Artillery: return 3;
                default: return 0;
            }
        }

        GameObject GetUnitPrefab(BarracksUpgradeType type)
        {
            switch (type)
            {
                case BarracksUpgradeType.Gunner: return gunnerPrefab;
                case BarracksUpgradeType.Sniper: return sniperPrefab;
                case BarracksUpgradeType.Artillery: return artilleryPrefab;
                default: return null;
            }
        }

        void Die()
        {
            State = BuildingState.Ruined;
            if (m_Renderer != null)
                m_Renderer.material.color = Color.gray;

            Debug.Log("Barracks destroyed!");
        }
    }
}
