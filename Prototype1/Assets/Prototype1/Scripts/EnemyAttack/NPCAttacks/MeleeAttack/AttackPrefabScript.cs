using Assets.Prototype1.Scripts.Buildings;
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
        Debug.LogError(other.gameObject.name + "this is what we are looking for");
        if (other.TryGetComponent(out HealthSystem enemy))
        {
            Debug.LogError($"allied collider {other.transform.name} was hit and the collider had {(enemy as IHealthSystem).CharacterType}");
            IHealthSystem enemyHealth = enemy;
            switch (_selfCharacterType)
            {
                case CharacterType.EnemyNPC:
                    if(enemyHealth.CharacterType==CharacterType.Player || enemyHealth.CharacterType == CharacterType.AlliedNPC || enemyHealth.CharacterType == CharacterType.AliedBuildings)
                    {
                        if(enemyHealth.CharacterType == CharacterType.AliedBuildings)
                        {
                            var state = other.GetComponent<Building>().State;
                            if(state == BuildingState.Ruined)
                            {
                                return;
                            }
                            else
                            {
                                enemyHealth?.TakeDamage(_damage, sender);
                            }
                        }
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
