using UnityEngine;
using UnityEngine.InputSystem;

public class DraggerInputHandler : MonoBehaviour
{
    private InputAction mouseEvent;
    private InputAction clickAction;
    private float cameraDepth;
    private Dragger draggingItem;
    private Camera mainCamera;

    private const float TapMaxDuration = 0.25f;
    private const float TapMaxDistance = 0.2f;
    private const float DoubleClickInterval = 0.3f;

    private float pressTime;
    private Vector3 pressWorldPos;
    private float lastClickTime = -1f;
    private Dragger lastClickedItem;

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

        Vector3 inputVec = mouseEvent.ReadValue<Vector2>();
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(inputVec.x, inputVec.y, cameraDepth));
        Vector3 clampedPos = ClampIconPos(worldPos);
        draggingItem.OnDragged(clampedPos);
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

        if (!hit.TryGetComponent(out Dragger dragger) || !dragger.CanDrag) return;

        draggingItem = dragger;
        draggingItem.OnDragStart();

        pressTime = Time.time;
        pressWorldPos = worldPos;
    }

    private void OnDragEnd(InputAction.CallbackContext ctx)
    {
        if (draggingItem == null) return;

        bool isTap = Time.time - pressTime <= TapMaxDuration && Vector3.Distance(draggingItem.transform.position, pressWorldPos) <= TapMaxDistance;

        draggingItem.OnDragEnd();

        if (isTap)
        {
            if (draggingItem == lastClickedItem && Time.time - lastClickTime <= DoubleClickInterval)
            {
                draggingItem.OnDoubleClicked();
                lastClickedItem = null;
            }
            else
            {
                lastClickedItem = draggingItem;
                lastClickTime = Time.time;
            }
        }
        else
        {
            lastClickedItem = null;
        }

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