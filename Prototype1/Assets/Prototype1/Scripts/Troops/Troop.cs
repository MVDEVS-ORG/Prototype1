using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using prototype1.scripts.systems;
using prototype1.scripts.attacks;

public enum TroopType
{
    Gunner,
    Sniper,
    Artillery
}

[RequireComponent(typeof(NavMeshAgent))]
public class Troop : MonoBehaviour, IHealthSystem
{
    public TroopType type;
    public float attackRange;
    public float damage;
    public float mobility; // movement speed
    public Color defaultColor = Color.white;
    public Color selectedColor = Color.green;
    public int maxHealth = 100;

    private NavMeshAgent _agent;
    private Renderer _rend;
    private bool _isSelected = false;
    private GameObject _targetEnemy;
    private int _currentHealth;

    public CharacterType CharacterType => CharacterType.AlliedNPC;

    void OnEnable()
    {
        SelectionManager.RegisterTroop(this);
    }
    void OnDisable()
    {
        SelectionManager.UnregisterTroop(this);
    }
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _rend = GetComponent<Renderer>();
        _rend.material.color = defaultColor;
        _currentHealth = maxHealth;
        ApplyStatsByType();
    }

    void Update()
    {
        if (!_isSelected)
        {
            AcquireAndAttack();
        }
    }

    void ApplyStatsByType()
    {
        switch (type)
        {
            case TroopType.Gunner:
                attackRange = 5f; damage = 10f; mobility = 6f;
                break;
            case TroopType.Sniper:
                attackRange = 15f; damage = 15f; mobility = 3f;
                break;
            case TroopType.Artillery:
                attackRange = 12f; damage = 30f; mobility = 2f;
                break;
        }
        _agent.speed = mobility;
    }

    public void Select()
    {
        _isSelected = true;
        _rend.material.color = selectedColor;
        _agent.isStopped = true;
    }

    public void Deselect()
    {
        _isSelected = false;
        _rend.material.color = defaultColor;
        _agent.isStopped = false;
    }

    public void MoveTo(Vector3 position)
    {
        _agent.isStopped = false;
        _agent.SetDestination(position);
    }

    private void AcquireAndAttack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Enemy"));
        if (hits.Length > 0)
        {
            float minDist = float.MaxValue;
            foreach (var hit in hits)
            {
                float d = Vector3.Distance(transform.position, hit.transform.position);
                if (d < minDist) { minDist = d; _targetEnemy = hit.gameObject; }
            }
            if (_targetEnemy != null)
            {
                _agent.SetDestination(_targetEnemy.transform.position);
                if (minDist <= 1.5f) Attack(_targetEnemy.GetComponent<IHealthSystem>());
            }
        }
    }

    void Attack(IHealthSystem enemyHealthSystem)
    {
        Debug.LogError("attacking");
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if(_currentHealth <= 0)
        {
            Die();
        }
    }

    public void RestoreHealth(int healing)
    {
        _currentHealth += healing;
        if(_currentHealth > maxHealth) _currentHealth = maxHealth;
    }

    public void ResetHealth()
    {
        _currentHealth = maxHealth;
    }
    private void Die()
    {
        Debug.Log("Troop Died");
        _agent.isStopped = true;

        Destroy(gameObject);
    }

}