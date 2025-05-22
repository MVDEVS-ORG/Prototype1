using prototype1.scripts.systems;
using System.Collections;
using UnityEngine;

namespace prototype1.scripts.attacks
{
    [RequireComponent(typeof(HealthSystem))]
    public class ArtilliaryAttack : MonoBehaviour, INPCAttack
    {
        [SerializeField] ArtilliaryPrefabScript _attackPrefab;
        IHealthSystem _healthSystem;    
        public void Attack(IHealthSystem enemy)
        {
            StartCoroutine(AttackUsingShell((enemy as HealthSystem).transform.position));
        }

        void Start()
        {
            _healthSystem = GetComponent<HealthSystem>();
        }

        IEnumerator AttackUsingShell(Vector3 pos)
        {
            ArtilliaryPrefabScript attack = Instantiate(_attackPrefab.gameObject, transform.position, Quaternion.identity).GetComponent<ArtilliaryPrefabScript>();
            attack.SetParameters(pos, _healthSystem.CharacterType, gameObject);
            yield return null;
        }
    }
}
