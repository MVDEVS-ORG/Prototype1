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
public class Troop : MonoBehaviour
{
    public TroopType troopType;
    public float attackRange;
    public float mobility; // movement speed
    public Color defaultColor = Color.white;
    public Color selectedColor = Color.green;
    public float detectDistance;
    public float attackCooldown;

    private NavMeshAgent _agent;
    private Renderer _rend;
    public bool _isSelected = false;
    private GameObject _targetEnemy;
    private INPCAttack _npcAttack;
    private float _lastAttackTime = 0f;
    private HealthSystem _healthSystem;
    [SerializeField] private LayerMask _enemyLayerMask;

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
        _npcAttack = GetComponent<INPCAttack>();
        _rend.material.color = defaultColor;
        ApplyStatsByType();
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnZeroHealth += Die;
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
        switch (troopType)
        {
            case TroopType.Gunner:
                attackRange = 50f; mobility = 6f;
                break;
            case TroopType.Sniper:
                attackRange = 100f; mobility = 3f;
                break;
            case TroopType.Artillery:
                attackRange = 150f; mobility = 2f;
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

    private void Die()
    {
        _healthSystem.OnZeroHealth -= Die;
        Destroy(gameObject);
    }

    private void AcquireAndAttack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectDistance, LayerMask.GetMask("Enemy"));
        if (hits.Length > 0)
        {
            float minDist = float.MaxValue;
            foreach (var hit in hits)
            {
                float d = Vector3.Distance(transform.position, hit.transform.position);
                if (d < minDist) { minDist = d; _targetEnemy = hit.gameObject; }
            }
            if (_targetEnemy != null && _targetEnemy.TryGetComponent(out IHealthSystem enemy))
            {
                _agent.SetDestination(_targetEnemy.transform.position);
                if (minDist < attackRange)
                {
                    if (IsObjectiveVisible(_targetEnemy.transform.position, Vector3.Distance(_targetEnemy.transform.position, transform.position), CharacterType.EnemyNPC))
                    {
                        _agent.isStopped = true;
                        if (Time.time > (_lastAttackTime + attackCooldown))
                        {
                            _npcAttack.Attack(enemy);
                            _lastAttackTime = Time.time;
                        }
                    }
                }
                else
                {
                    _agent.isStopped = false;
                    _agent.SetDestination(_targetEnemy.transform.position);
                }
                if (minDist <= 1.5f)
                {
                    _npcAttack.Attack(enemy);
                }
            }
        }
    }
    private bool IsObjectiveVisible(Vector3 objective, float distanceToPlayer, CharacterType objectiveType)
    {
        Vector3 direction = (objective - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distanceToPlayer, _enemyLayerMask))
        {
            if (hit.transform.TryGetComponent(out HealthSystem health))
            {
                if ((health as IHealthSystem).CharacterType == objectiveType)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        return false;
    }
}