using prototype1.scripts.attacks;
using UnityEngine;

namespace prototype1.scripts.systems
{
    public class HealthSystem : MonoBehaviour, IHealthSystem
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;
        [SerializeField] private CharacterType _characterType;
        CharacterType IHealthSystem.CharacterType => _characterType;

        void IHealthSystem.ResetHealth()
        {
            _currentHealth = _maxHealth;
        }

        void IHealthSystem.RestoreHealth(int healing)
        {
            _currentHealth = Mathf.Min(_currentHealth + healing, _maxHealth);
        }

        void IHealthSystem.TakeDamage(int damage)
        {
            _currentHealth = Mathf.Max(_currentHealth - damage, 0);
        }
    }
}
