using prototype1.scripts.systems;

namespace prototype1.scripts.attacks
{
    public interface INPCAttack
    {
        void Attack(IHealthSystem enemy);
    }

    public enum CharacterType
    {
        Player,
        AlliedNPC,
        EnemyNPC
    }
}
