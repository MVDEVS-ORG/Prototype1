using prototype1.scripts.attacks;
using prototype1.scripts.systems;
using System;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Grunts,
    Ranged,
    Elites,
    Boss
}

public class Enemy : MonoBehaviour
{
    [Header("Enemy Type")]
    public EnemyType enemyType;

    [Header("References")]
    public Transform player;
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    [Header("Settings")]
    public float detectionRange = 15f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public float maxHealth = 100f;

    private NavMeshAgent agent;
    private float _lastAttackTime;
    [SerializeField]
    private bool _isPlayerVisible;
    [SerializeField]
    private bool _followPlayer;
    [SerializeField]
    private bool _isReachedDestination;
    [SerializeField]
    private bool _isFollowiongCommand = false;
    [SerializeField]
    private float _currentHealth;
    private Vector3 _currentDestination;
    private INPCAttack _npcAttack;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _npcAttack = GetComponent<INPCAttack>();
        Debug.LogError($"Enemy npc Attack: {(_npcAttack != null)}");
        _currentHealth = maxHealth;
    }

    private void Update()
    {
        if (_isFollowiongCommand)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    _isFollowiongCommand = false;
                    _followPlayer = true;
                }
            }
        }
        if (_followPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            _isPlayerVisible = IsPlayerVisible(distanceToPlayer);

            if (_isPlayerVisible && distanceToPlayer <= detectionRange)
            {
                if (distanceToPlayer <= attackRange && Time.time > _lastAttackTime + attackCooldown)
                {
                    IHealthSystem playerHealthSystem = player.GetComponent<IHealthSystem>();
                    _npcAttack.Attack(playerHealthSystem);
                    _lastAttackTime = Time.time;
                }
                else
                {
                    agent.SetDestination(player.position);
                }
            }
            else
            {
                agent.ResetPath();
            }
        }
        
    }

    private bool IsPlayerVisible(float distanceToPlayer)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        if(Physics.Raycast(transform.position, direction, out RaycastHit hit, distanceToPlayer, obstacleMask | playerMask))
        {
            return hit.transform.CompareTag("Player");
        }
        return false;
    }

    public void MoveToPostion(Vector3 position)
    {
        _isFollowiongCommand = true;
        _followPlayer = false;
        _currentDestination = position;
        agent.SetDestination(position);
    }

    private void Die()
    {
        Debug.Log("Enemy Died");
        agent.isStopped = true;

        Destroy(gameObject);
    }
}
