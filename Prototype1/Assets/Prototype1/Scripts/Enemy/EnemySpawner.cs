using prototype1.scripts.systems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] GameObject _grunts;
    [SerializeField] GameObject _gunners;
    [SerializeField] GameObject _elites;

    [Header("Spawner dimensions")]
    [SerializeField] int _spawnRadius;

    [SerializeField] private List<Groups> _groups;
    private Dictionary<EnemyType, GameObject> _enemyPref = new();

    [Header("Reference required by enemies")]
    [SerializeField] HealthSystem _playerBase;

    // revamp based on groups 
    // create a group class that can spawn say 5 enemies first then 10 after some delay the 15 and so on after delays and the groups can be mixed
    public void Start()
    {
        _enemyPref[EnemyType.Grunts] = _grunts;
        _enemyPref[EnemyType.Ranged] = _gunners;
        _enemyPref[EnemyType.Elites] = _elites;
        _enemyPref[EnemyType.Boss] = null;
        foreach (var group in _groups)
        {
            group.SetValues();
        }
        DayNightManager.instance.AddEnemySpawners();
    }

    public void StartSpawn()
    {
        StartCoroutine(spawnEnemies());
    }

    IEnumerator spawnEnemies()
    {
        foreach(var enemyGroup in _groups)
        {
            yield return new WaitForSeconds(enemyGroup.delayBeforeSpawn);
            foreach(var enemyType in enemyGroup.spawnValues)
            {
                try
                {
                    int numberOfEnemies = enemyType.Value;
                    int NumberOfAttempts = 0;
                    while (numberOfEnemies > 0)
                    {
                        if (NumberOfAttempts > 20)
                        {
                            break;
                        }
                        else
                        {
                            NumberOfAttempts++;
                        }
                        Vector2 rngInCircle = UnityEngine.Random.insideUnitCircle * _spawnRadius;
                        Vector3 randomPoint = transform.position + new Vector3(rngInCircle.x, transform.position.y, rngInCircle.y);
                        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit pointFound, 2f, NavMesh.AllAreas))
                        {
                            GameObject enemyObj = Instantiate(_enemyPref[enemyType.Key], pointFound.position, Quaternion.identity);
                            Enemy enemies = enemyObj.GetComponent<Enemy>();
                            DayNightManager.instance?.enemies.Add(enemies);
                            enemies.SetNPCMainObjective(_playerBase);
                            numberOfEnemies--;
                        }
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception);
                }
            }
        }
        DayNightManager.instance.AllEnemiesSpawned();
    }
}

[Serializable]
public class Groups
{
    public float delayBeforeSpawn;
    public int numberOfGrunts;
    public int numberOfGunners;
    public int numberOfElites;
    public int numberOfBosses;

    public Dictionary<EnemyType, int> spawnValues = new();

    public void SetValues()
    {
        spawnValues[EnemyType.Grunts] = numberOfGrunts;
        spawnValues[EnemyType.Ranged] = numberOfGunners;
        spawnValues[EnemyType.Elites] = numberOfElites;
        spawnValues[EnemyType.Boss] = numberOfBosses;
    }
}
