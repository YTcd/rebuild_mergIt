using UnityEngine;
using UnityEngine.InputSystem;

public class Drager : MonoBehaviour
{
    [SerializeField]
    private InputAction mouseEvent;
    [SerializeField]
    private InputAction clickAction;
    private Vector2 inputVec;
    private Vector3 worldPos;
    private float cameraDepth;
    private Vector2 clickOffset;

    [SerializeField]
    bool isClicked;

    void OnEnable()
    {
        clickOffset = new Vector2(0, 0);
        isClicked = false;
        cameraDepth = Mathf.Abs(Camera.main.transform.position.z);
        mouseEvent = InputSystem.actions.FindAction("Point");
        clickAction = InputSystem.actions.FindAction("Click");
        clickAction.started += OnDragStart;
        clickAction.canceled += OnDragEnd;
    }

    void Update()
    {
        if (isClicked == true)
        {
            inputVec = mouseEvent.ReadValue<Vector2>();
            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(inputVec.x, inputVec.y, cameraDepth));
        }
    }

    void FixedUpdate()
    {
        if (isClicked == true)
        {
            transform.position = worldPos - (Vector3)clickOffset;
        }
    }

    private void OnDragStart(InputAction.CallbackContext ctx)
    {
        Vector2 pos = mouseEvent.ReadValue<Vector2>();
        Vector3 world = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, cameraDepth));

        clickOffset = world - transform.position;

        Collider2D hit = Physics2D.OverlapPoint(world);
        if (hit == null) return;

        isClicked = true;
    }

    private void OnDragEnd(InputAction.CallbackContext ctx)
    {
        isClicked = false;
    }
}
