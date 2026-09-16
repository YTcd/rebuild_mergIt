using UnityEngine;
using UnityEngine.InputSystem;

public class DragerInputHandler : MonoBehaviour
{
    [SerializeField]
    private InputAction mouseEvent;
    [SerializeField]
    private InputAction clickAction;
    private float cameraDepth;
    private Drager draggingItem;
    private Camera mainCamera;
    private Vector2 inputVec;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void OnEnable()
    {
        cameraDepth = Mathf.Abs(mainCamera.transform.position.z);

        mouseEvent = InputSystem.actions.FindAction("Point");
        clickAction = InputSystem.actions.FindAction("Click");
        clickAction.started += OnDragStart;
        clickAction.canceled += OnDragEnd;
    }

    void Update()
    {
        if (draggingItem == null) return;

        inputVec = mouseEvent.ReadValue<Vector2>();
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(inputVec.x, inputVec.y, cameraDepth));
        Vector3 clampedPos = ClampIconPos(worldPos);
        draggingItem.OnDraged(clampedPos);
    }

    void OnDisable()
    {
        clickAction.started -= OnDragStart;
        clickAction.canceled -= OnDragEnd;
    }

    private void OnDragStart(InputAction.CallbackContext ctx)
    {
        Vector2 inputVec = mouseEvent.ReadValue<Vector2>();
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(inputVec.x, inputVec.y, cameraDepth));

        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        if (hit == null) return;

        if (!hit.TryGetComponent(out Drager drager) || !drager.CanDrag) return;

        draggingItem = drager;
        draggingItem.OnDragStart();
    }

    private void OnDragEnd(InputAction.CallbackContext ctx)
    {
        if (draggingItem == null) return;
        draggingItem.OnDragEnd();
        draggingItem = null;
    }

    private Vector3 ClampIconPos(Vector3 worldPos)
    {
        Rect bounds = GameManager.Instance.BoardBounds;

        float clampedX = Mathf.Clamp(worldPos.x, bounds.xMin, bounds.xMax);
        float clampedY = Mathf.Clamp(worldPos.y, bounds.yMin, bounds.yMax);

        return new Vector3(clampedX, clampedY, worldPos.z);
    }
}