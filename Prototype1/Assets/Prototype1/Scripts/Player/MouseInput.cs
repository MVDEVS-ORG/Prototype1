using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public Camera mainCamera;
    public Enemy[] enemies;
    public LayerMask groundMask;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray  = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
            {
                Vector3 targetPostion = hit.point;
                foreach(Enemy enemy in enemies)
                {
                    enemy.MoveToPostion(targetPostion);
                }           
            }
        }
    }
}
