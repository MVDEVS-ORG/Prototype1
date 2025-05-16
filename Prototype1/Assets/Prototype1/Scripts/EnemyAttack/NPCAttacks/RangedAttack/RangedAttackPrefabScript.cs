using prototype1.scripts.attacks;
using prototype1.scripts.systems;
using UnityEngine;

public class RangedAttackPrefabScript : MonoBehaviour
{
    private CharacterType _selfCharacterType;
    [SerializeField] private int _damage;

    public void DamageEnemy(IHealthSystem enemy)
    {
        switch (_selfCharacterType)
        {
            case CharacterType.EnemyNPC:
                if (enemy.CharacterType == CharacterType.Player || enemy.CharacterType == CharacterType.AlliedNPC)
                {
                    enemy.TakeDamage(_damage);
                }
                break;

            case CharacterType.AlliedNPC:
                if (enemy.CharacterType == CharacterType.EnemyNPC)
                {
                    enemy.TakeDamage(_damage);
                }
                break;

            case CharacterType.Player:
                if (enemy.CharacterType == CharacterType.EnemyNPC)
                {
                    enemy.TakeDamage(_damage);
                }
                break;
        }
    }
}
