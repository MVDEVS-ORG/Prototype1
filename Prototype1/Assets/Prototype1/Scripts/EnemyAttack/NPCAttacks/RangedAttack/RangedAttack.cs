using prototype1.scripts.systems;
using System.Collections;
using UnityEngine;

namespace prototype1.scripts.attacks
{
    [RequireComponent(typeof(HealthSystem))]
    public class RangedAttack : MonoBehaviour, INPCAttack
    {
        [Range(0, 2)][SerializeField] float _animationTime;
        [SerializeField] RangedAttackPrefabScript _attackPrefab;
        Coroutine _attackCoroutine;
        IHealthSystem _healthSystem;

        private void Start()
        {
            _healthSystem = GetComponent<HealthSystem>();
        }

        public void Attack(IHealthSystem enemy)
        {
            if (_attackCoroutine != null)
            {
                return;
            }
            _attackCoroutine = StartCoroutine(AttackUsingRanged(enemy));
        }

        IEnumerator AttackUsingRanged(IHealthSystem enemy)
        {
            RangedAttackPrefabScript attack = Instantiate(_attackPrefab.gameObject, transform).GetComponent<RangedAttackPrefabScript>();
            yield return new WaitForSeconds(_animationTime);
            attack.DamageEnemy(enemy);
            Destroy(attack.gameObject);
            _attackCoroutine = null;
        }

    }
}
