using prototype1.scripts.attacks;
using prototype1.scripts.systems;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ArtilliaryPrefabScript : MonoBehaviour
{
    private Vector3 _startPos;
    private Vector3 _endPos;
    [SerializeField] private float _time;
    [SerializeField] private int _damage;
    private float _timer=0f;
    private Vector3 _upperCoordinate;
    private SphereCollider _collider;
    private bool _endOfMovement = false;
    private CharacterType _selfCharacterType;

    public void SetParameters(Vector3 pos, CharacterType selfType)
    {
        _startPos = transform.position;
        _endPos = pos;
        _upperCoordinate = Vector3.Lerp(_startPos, _endPos, 0.5f);
        _upperCoordinate.y = _upperCoordinate.y + Vector3.Distance(_startPos, _endPos)*2;
        Debug.DrawLine(_startPos, (_endPos - _startPos)/2, Color.green, 10f);
        Debug.DrawLine(_startPos, _upperCoordinate, Color.red, 10f);
        Debug.DrawLine(_upperCoordinate, _endPos, Color.red, 10f);
        gameObject.SetActive(true);
        _collider = GetComponent<SphereCollider>();
        _selfCharacterType = selfType;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while(_timer<=1)
        {
            Vector3 pointA = Vector3.Lerp(_startPos, _upperCoordinate, _timer);
            Vector3 pointB = Vector3.Lerp(_upperCoordinate, _endPos, _timer);
            transform.position = Vector3.Lerp(pointA, pointB, _timer);
            _timer += Time.deltaTime/_time;
            yield return new WaitForEndOfFrame();
        }
        _endOfMovement = true;
        _collider.enabled = true;
        Destroy(gameObject,0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HealthSystem enemy))
        {
            IHealthSystem enemyHealth = enemy;
            switch (_selfCharacterType)
            {
                case CharacterType.EnemyNPC:
                    if (enemyHealth.CharacterType == CharacterType.Player || enemyHealth.CharacterType == CharacterType.AlliedNPC)
                    {
                        enemyHealth?.TakeDamage(_damage);
                    }
                    break;

                case CharacterType.AlliedNPC:
                    if (enemyHealth.CharacterType == CharacterType.EnemyNPC)
                    {
                        enemyHealth?.TakeDamage(_damage);
                    }
                    break;

                case CharacterType.Player:
                    if (enemyHealth.CharacterType == CharacterType.EnemyNPC)
                    {
                        enemyHealth?.TakeDamage(_damage);
                    }
                    break;
            }
        }
    }
}
