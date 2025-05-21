using prototype1.scripts.attacks;
using prototype1.scripts.stateMachine;
using prototype1.scripts.systems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public LayerMask obstacleMask;

    [Header("Settings")]
    public float detectionRange = 15f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public LayerMask buildingLayerMask;
    public LayerMask alliedTroopsLayerMask;
    [Tooltip("This is in number of frames after which update will get triggered")]
    public int enemyDetectionTickRate;
    private int ticks = 0;
    private IStateMachine stateMachine; 
    private NavMeshAgent agent;
    private float _lastAttackTime;
    [SerializeField]
    private bool _isReachedDestination;
    private Vector3 _currentDestination;
    private INPCAttack _npcAttack;

    private HealthSystem _sideObjective = null;
    private HealthSystem _mainObjective;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _npcAttack = GetComponent<INPCAttack>();
        stateMachine = new StateMachine();
        stateMachine.Initialize();
    }

    private void Update()
    {
        ticks++;
        if(ticks>enemyDetectionTickRate)
        {
            ticks = 0;
            if(HasNPCReachedCurrentObjective() && _currentDestination!=null)
            {
                agent.isStopped = false;
                stateMachine.changeState(NPCState.Attack);
            }
            else
            {
                if (stateMachine.CurrentState != NPCState.Move)
                {
                    stateMachine.changeState(NPCState.Move);
                }
            }
            if(_sideObjective==null)
            {
                if (stateMachine.CurrentState != NPCState.Attack)
                {
                    SearchSideObjective();
                }
                if (_sideObjective == null)
                {
                    agent.isStopped = false;
                    _currentDestination = _mainObjective.transform.position;
                    agent.SetDestination(_mainObjective.transform.position);
                    if (stateMachine.CurrentState != NPCState.Move)
                    {
                        stateMachine.changeState(NPCState.Move);
                    }
                }
            }
            if (stateMachine.CurrentState == NPCState.Attack && _sideObjective != null)
            {
                if (Time.time > (_lastAttackTime + attackCooldown))
                {
                    agent.isStopped = true;
                    IHealthSystem playerHealthSystem = _sideObjective;
                    _npcAttack.Attack(playerHealthSystem);
                    _lastAttackTime = Time.time;
                }
            }
            else if (stateMachine.CurrentState == NPCState.Attack && _mainObjective!= null)
            {
                if (Time.time > (_lastAttackTime + attackCooldown))
                {
                    agent.isStopped = true;
                    IHealthSystem playerHealthSystem = _mainObjective;
                    _npcAttack.Attack(playerHealthSystem);
                    _lastAttackTime = Time.time;
                }
            }
            else if(_sideObjective == null && _mainObjective== null)
            {
                stateMachine.changeState(NPCState.Idle);
            }
        } 
    }

    public void SetNPCMainObjective(HealthSystem point)
    {
        Debug.LogError(point == null);
        _mainObjective = point;
        _currentDestination= _mainObjective.transform.position;
        agent.SetDestination(_mainObjective.transform.position);
    }

    private void SearchSideObjective()
    {
        Collider[] buildHits = Physics.OverlapSphere(transform.position, detectionRange, buildingLayerMask);
        if (buildHits.Length == 0 && (stateMachine.CurrentState == NPCState.Move || stateMachine.CurrentState == NPCState.Idle))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, alliedTroopsLayerMask);
            SetPlayerDestination(hits);
        }
        else
        {
            SetPlayerDestination(buildHits);
        }

    }

    private bool HasNPCReachedCurrentObjective()
    {
        return Vector3.Distance(transform.position, _currentDestination)<=attackRange;
    }

    private void SetPlayerDestination(Collider[] hits)
    {
        if (hits.Length > 0)
        {
            float minDistance = 10000;
            Collider closestCollider = null;
            foreach (var item in hits)
            {
                float dist = Vector3.Distance(item.transform.position, transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestCollider = item;
                }
            }
            if (closestCollider != null)
            {
                if (closestCollider.TryGetComponent(out HealthSystem objectiveHealth))
                {
                    _currentDestination = closestCollider.transform.position;
                    agent.isStopped = false;
                    agent.SetDestination(_currentDestination);
                    stateMachine.changeState(NPCState.Move);
                    _sideObjective = objectiveHealth;
                }
            }
        }
    }

    private bool IsObjectiveVisible(float distanceToPlayer, CharacterType objectiveType)
    {
        Vector3 direction = (_currentDestination - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distanceToPlayer, obstacleMask | alliedTroopsLayerMask))
        {
            if (hit.transform.TryGetComponent(out IHealthSystem health))
            {
                if(health.CharacterType == objectiveType)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        return false;
    }

    public void MoveToPostion(Vector3 position)
    {
        _currentDestination = position;
        agent.SetDestination(position);
    }
}
