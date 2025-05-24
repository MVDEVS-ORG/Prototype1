using prototype1.scripts.attacks;
using prototype1.scripts.systems;
using UnityEngine;

public class RangedAttackPrefabScript : MonoBehaviour
{
    [SerializeField] private int _damage;
    private GameObject attacker;

    public void SetParameters(GameObject sender)
    {
        attacker = sender;
    }

    public void DamageEnemy(IHealthSystem enemy, CharacterType selfType)
    {
        switch (selfType)
        {
            case CharacterType.EnemyNPC:
                if (enemy.CharacterType == CharacterType.Player || enemy.CharacterType == CharacterType.AlliedNPC || enemy.CharacterType == CharacterType.AliedBuildings)
                {
                    enemy?.TakeDamage(_damage,attacker);
                }
                break;

            case CharacterType.AlliedNPC:
                if (enemy.CharacterType == CharacterType.EnemyNPC)
                {
                    enemy?.TakeDamage(_damage,attacker);
                }
                break;

            case CharacterType.Player:
                if (enemy.CharacterType == CharacterType.EnemyNPC)
                {
                    enemy?.TakeDamage(_damage, attacker);
                }
                break;
        }
    }
}
