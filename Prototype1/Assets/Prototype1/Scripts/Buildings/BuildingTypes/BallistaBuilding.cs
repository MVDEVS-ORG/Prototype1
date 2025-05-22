using prototype1.scripts.attacks;
using prototype1.scripts.systems;
using UnityEngine;

namespace Assets.Prototype1.Scripts.Buildings
{
    public enum BallistaType
    {
        Basic,
        Advanced,
        Elite
    }

    public class BallistaBuilding : Building
    {
        public BallistaType CurrentBallista = BallistaType.Basic;
        public float attackRange = 10f;
        public int damage = 10;
        public float attackCooldown = 1.5f;
        private float lastAttackTime;
        private GameObject _targetEnemy;
        private HealthSystem _selfHealthSystem;

        private void Start()
        {
            _selfHealthSystem = GetComponent<HealthSystem>();
            _selfHealthSystem.OnZeroHealth += Die;
            SetInitialValues();
        }

        void SetInitialValues()
        {
            BuildCost = 10;
            UpgradeCost = 15;
            CanBeUpgraded = true;
            maxUpgradeLimit = 3; // Fix: properly initialized
            (_selfHealthSystem as IHealthSystem).ResetHealth();
        }

        protected override void Update()
        {
            base.Update();
            if (State == BuildingState.Completed)
            {
                AttackEnemyInRange();
            }
        }

        void AttackEnemyInRange()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Enemy"));
            if (hits.Length > 0)
            {
                float minDist = float.MaxValue;
                GameObject closestEnemy = null;

                foreach (var hit in hits)
                {
                    float d = Vector3.Distance(transform.position, hit.transform.position);
                    if (d < minDist)
                    {
                        minDist = d;
                        closestEnemy = hit.gameObject;
                    }
                }

                _targetEnemy = closestEnemy;

                if (_targetEnemy != null && _targetEnemy.activeInHierarchy &&
                    _targetEnemy.TryGetComponent(out IHealthSystem enemy))
                {
                    if (Time.time > (lastAttackTime + attackCooldown))
                    {
                        Debug.Log("Ballista attacked enemy.");
                        enemy.TakeDamage(damage,null);
                        lastAttackTime = Time.time;
                    }
                }
            }
            else
            {
                _targetEnemy = null;
            }
        }

        public override void UpgradeBuilding()
        {
            if ((currentUpgrade >= maxUpgradeLimit || CurrentBallista == BallistaType.Elite) && !CanBeUpgraded)
            {
                Debug.Log("Ballista is already at max upgrade.");
                return;
            }

            currentUpgrade++;
            UpgradeCost += 10;

            if (CurrentBallista == BallistaType.Basic)
            {
                CurrentBallista = BallistaType.Advanced;
                damage += 5;
                attackRange += 2;
            }
            else if (CurrentBallista == BallistaType.Advanced)
            {
                CurrentBallista = BallistaType.Elite;
                damage += 10;
                attackRange += 3;
                CanBeUpgraded = false;
            }
            Debug.Log($"Ballista upgraded to {CurrentBallista}.");
        }


        void Die()
        {
            State = BuildingState.Ruined;
            if (m_Renderer != null)
                m_Renderer.material.color = Color.gray;

            Debug.Log("Ballista destroyed!");
            _selfHealthSystem.OnZeroHealth -= Die;
            
        }
    }
}
