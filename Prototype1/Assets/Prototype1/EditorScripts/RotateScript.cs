using UnityEngine;

public class RotateScript : MonoBehaviour
{
    void Update()
    {
        transform.RotateAround(transform.position, transform.up, 1);
    }
}
