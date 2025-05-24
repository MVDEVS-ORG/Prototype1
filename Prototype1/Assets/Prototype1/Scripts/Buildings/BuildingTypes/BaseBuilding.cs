using prototype1.scripts.systems;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Prototype1.Scripts.Buildings
{
    public class BaseBuilding : Building
    {
        private HealthSystem _selfHealthSystem;
        private void Start()
        {
            _selfHealthSystem = GetComponent<HealthSystem>();
            SetInitialValues();
        }

        private void SetInitialValues()
        {
            BuildCost = 0;
            UpgradeCost = 10;
            maxUpgradeLimit = 3;
            currentUpgrade = 1;
            CanBeUpgraded = true;
            State = BuildingState.Completed;

            if (m_Renderer != null)
                m_Renderer.material.color = Color.green;
            Debug.Log("Base Building initialized in completes State");
            (_selfHealthSystem as IHealthSystem).ResetHealth();
            _selfHealthSystem.OnZeroHealth += Die;
        }

        void Die()
        {
            State = BuildingState.Ruined;

            m_Renderer.material.color = Color.gray;

            Debug.Log("Base destroyed!");
            _selfHealthSystem.OnZeroHealth -= Die;
        }

        public override void Build()
        {
            Debug.Log("Base is already Built. No need to build again");
        }

        public override void UpgradeBuilding()
        {
            if (!CanBeUpgraded)
            {
                Debug.LogError("Base is at max Upgrade Level");
                return;
            }

            if(currentUpgrade < maxUpgradeLimit)
            {
                currentUpgrade++;
                if(m_Renderer != null)
                {
                    switch (currentUpgrade)
                    {
                        case 2: m_Renderer.material.color = Color.cyan; break;
                        case 3: m_Renderer.material.color = Color.magenta; break;
                    }
                }

                Debug.Log($"Base Upgraded to level {currentUpgrade}");
            }

            if(currentUpgrade >= maxUpgradeLimit)
            {
                CanBeUpgraded = false;
                Debug.LogError("Base reached max Upgrade Level");
            }
        }
    }
}