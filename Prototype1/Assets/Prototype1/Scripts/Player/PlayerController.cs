using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 150f;

    private Rigidbody _rigidbody;
    private float _moveInput;
    private float _rotateInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; 
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput = Input.GetAxis("Vertical");
        _rotateInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = transform.forward * _moveInput * moveSpeed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(_rigidbody.position + moveDirection);

        float rotation = _rotateInput * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnOffset = Quaternion.Euler(0f, rotation, 0f);
        _rigidbody.MoveRotation(_rigidbody.rotation * turnOffset);
    }
}
