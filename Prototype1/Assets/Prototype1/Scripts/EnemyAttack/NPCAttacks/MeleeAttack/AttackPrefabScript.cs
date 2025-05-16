using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AttackPrefabScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //here we try get component npc and deal the respective damage
    }
}
