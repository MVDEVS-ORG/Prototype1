using prototype1.scripts.systems;
using System.Collections;
using UnityEngine;

namespace prototype1.scripts.attacks
{
    [RequireComponent(typeof(HealthSystem))]
    public class MeleeAttack : MonoBehaviour, INPCAttack
    {
        [Range(0, 2)][SerializeField] float _animationTime;
        [SerializeField] AttackPrefabScript _attackPrefab;
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
            _attackCoroutine = StartCoroutine(AttackUsingMelee());
        }

        IEnumerator AttackUsingMelee()
        {
            AttackPrefabScript attack = Instantiate(_attackPrefab.gameObject, transform).GetComponent<AttackPrefabScript>();
            attack.SetCharacterType(_healthSystem.CharacterType, gameObject);
            yield return new WaitForSeconds(_animationTime);
            Destroy(attack.gameObject);
            _attackCoroutine = null;
        }
    }
}
