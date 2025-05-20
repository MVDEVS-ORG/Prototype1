using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] GameObject _grunts;
    [SerializeField] GameObject _gunners;
    [SerializeField] GameObject _elites;
    [SerializeField] int _numberOfGrunts;
    [SerializeField] int _numberOfGunners;
    [SerializeField] int _numberOfElites;

    [SerializeField] float _spawnDelay;

    [Header("Spawner dimensions")]
    [SerializeField] int _spawnRadius;
    
    private bool _spawn = true;
    private Dictionary<GameObject, int> _spawnRecords = new();

    [Header("Reference required by enemies")]
    [SerializeField] Transform _player;
    public void Start()
    {
        _spawnRecords[_grunts] = _numberOfGrunts;
        _spawnRecords[_gunners] = _numberOfGunners;
        _spawnRecords[_elites] = _numberOfElites;
    }

    public void StartSpawn()
    {
        StartCoroutine(spawnEnemies());
    }

    IEnumerator spawnEnemies()
    {
        foreach(var enemy in _spawnRecords)
        {
            int numberOfEnemies = enemy.Value;
            int NumberOfAttempts = 0;
            while(numberOfEnemies>0)
            {
                if(NumberOfAttempts>5)
                {
                    break;
                }
                else
                {
                    NumberOfAttempts++;
                }
                Vector2 rngInCircle = Random.insideUnitCircle * _spawnRadius;
                Vector3 randomPoint = transform.position + new Vector3(rngInCircle.x,transform.position.y, rngInCircle.y);
                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit pointFound, 2f, NavMesh.AllAreas))
                {
                    Enemy enemies = Instantiate(enemy.Key, pointFound.position, Quaternion.identity).GetComponent<Enemy>();
                    enemies.player = _player;
                    numberOfEnemies--;
                    yield return new WaitForSeconds(_spawnDelay);
                }
            }
        }
    }
}
