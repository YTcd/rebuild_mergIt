using UnityEngine.InputSystem;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public Vector2 inputVec;
    [SerializeField]
    private float moveSpeed = 5f;
    private InputAction moveAction;

    void OnEnable()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        inputVec = moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * moveSpeed * Time.fixedDeltaTime;
        transform.position += (Vector3)nextVec;
    }
}
