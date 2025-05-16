using prototype1.scripts.attacks;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, INPCAttack
{
    [Range(0,2)][SerializeField] float _animationTime;
    [SerializeField] GameObject _attackPrefab;
    Coroutine _attackCoroutine;
    public void Attack(Vector3 pos)
    {
        if(_attackCoroutine!=null)
        {
            return;
        }
        _attackCoroutine = StartCoroutine(AttackUsingMelee());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack(new Vector3(0,0,0));
        }
    }

    IEnumerator AttackUsingMelee()
    {
        GameObject attack = Instantiate(_attackPrefab, transform);
        yield return new WaitForSeconds(_animationTime);
        Destroy(attack);
        _attackCoroutine = null;
    }
}
