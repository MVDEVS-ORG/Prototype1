using prototype1.scripts.systems;
using System.Collections;
using UnityEngine;

namespace Assets.Prototype1.Scripts.Buildings
{
    public abstract class Building : MonoBehaviour, IBuildingSystem
    {
        public int BuildCost { get; protected set; }
        public int UpgradeCost { get; protected set; }
        public bool CanBeUpgraded { get; protected set; }
        public BuildingState State { get; protected set; } = BuildingState.Ruined;
        public BuildingTypes BuildingType { get; protected set; }
        public int maxUpgradeLimit { get; protected set; }
        public int currentUpgrade;
        public Renderer m_Renderer;

        public bool isPlayerNearby { get; set; }

        public virtual void Build()
        {
            if (State == BuildingState.Ruined)
            {
                bool costReduced = CurrencyManager.Instance.SpendCurrency(BuildCost);
                if (costReduced)
                {
                    State = BuildingState.Completed;
                    currentUpgrade = 1;
                    if (m_Renderer != null)
                        m_Renderer.material.color = Color.blue;
                    GetComponent<IHealthSystem>().ResetHealth();
                    Debug.Log($"{name} built successfully!");
                }
            }
        }

        protected virtual void Update()
        {
            HandlePlayerInput();
        }

        private void HandlePlayerInput()
        {
            if (!isPlayerNearby) return;

            if (Input.GetKeyDown(KeyCode.B))
            {
                if (State == BuildingState.Ruined)
                {
                    Build();
                }
                else if (State == BuildingState.Completed)
                {
                    UpgradeBuilding();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerNearby = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerNearby = false;
            }
        }

        public abstract void UpgradeBuilding();
    }

}