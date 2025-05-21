using prototype1.scripts.attacks;
using UnityEngine;

namespace prototype1.scripts.systems
{
    public interface IHealthSystem
    {
        void TakeDamage(int damage, GameObject damager);
        void RestoreHealth(int healing);
        void ResetHealth();
        CharacterType CharacterType { get; }
    }
}
