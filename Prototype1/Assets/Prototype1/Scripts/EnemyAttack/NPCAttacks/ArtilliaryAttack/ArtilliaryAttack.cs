using prototype1.scripts.attacks;
using System.Collections;
using UnityEngine;

public class ArtilliaryAttack : MonoBehaviour, INPCAttack
{
    [SerializeField] GameObject _attackPrefab;
    [SerializeField] 
    public void Attack(Vector3 pos)
    {
        StartCoroutine(AttackUsingShell(pos));
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AttackUsingShell(Vector3 pos)
    {
        GameObject attack = Instantiate(_attackPrefab, transform);
        yield return null;
    }
}
