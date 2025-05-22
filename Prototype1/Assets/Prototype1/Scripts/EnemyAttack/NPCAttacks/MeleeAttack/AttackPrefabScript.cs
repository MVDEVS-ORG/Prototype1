using prototype1.scripts.attacks;
using prototype1.scripts.systems;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AttackPrefabScript : MonoBehaviour
{
    private CharacterType _selfCharacterType;
    [SerializeField] private int _damage;
    private GameObject sender;

    public void SetCharacterType(CharacterType selfType, GameObject sender)
    {
        this.sender = sender;
        _selfCharacterType = selfType;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HealthSystem enemy))
        {
            IHealthSystem enemyHealth = enemy;
            switch (_selfCharacterType)
            {
                case CharacterType.EnemyNPC:
                    if(enemyHealth.CharacterType==CharacterType.Player || enemyHealth.CharacterType == CharacterType.AlliedNPC)
                    {
                        enemyHealth?.TakeDamage(_damage, sender );
                    }
                    break;

                case CharacterType.AlliedNPC:
                    if(enemyHealth.CharacterType == CharacterType.EnemyNPC)
                    {
                        enemyHealth?.TakeDamage(_damage, sender);
                    }
                    break;

                case CharacterType.Player:
                    if(enemyHealth.CharacterType == CharacterType.EnemyNPC)
                    {
                        enemyHealth?.TakeDamage(_damage, sender);
                    }
                    break;
            }
        }
    }
}
