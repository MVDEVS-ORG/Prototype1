using prototype1.scripts.attacks;

namespace prototype1.scripts.systems
{
    public interface IHealthSystem
    {
        void TakeDamage(int damage);
        void RestoreHealth(int healing);
        void ResetHealth();
        CharacterType CharacterType { get; }
    }
}
