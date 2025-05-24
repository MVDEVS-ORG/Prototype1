using prototype1.scripts.attacks;
using System;
using UnityEngine;

namespace prototype1.scripts.systems
{
    public class HealthSystem : MonoBehaviour, IHealthSystem
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;
        [SerializeField] private CharacterType _characterType;
        public Action<GameObject> OnDamaged;
        public Action OnZeroHealth;
        CharacterType IHealthSystem.CharacterType => _characterType;

        public int CurrentHealth => _currentHealth;

        void IHealthSystem.ResetHealth()
        {
            _currentHealth = _maxHealth;
        }

        void IHealthSystem.RestoreHealth(int healing)
        {
            _currentHealth = Mathf.Min(_currentHealth + healing, _maxHealth);
        }

        void IHealthSystem.TakeDamage(int damage, GameObject damager)
        {
            if(OnDamaged!=null)
            {
                OnDamaged.Invoke(damager);
            }
            _currentHealth = Mathf.Max(_currentHealth - damage, 0);
            if(_currentHealth==0)
            {
                if(OnZeroHealth!=null)
                {
                    OnZeroHealth.Invoke();
                }
            }
        }
    }
}
