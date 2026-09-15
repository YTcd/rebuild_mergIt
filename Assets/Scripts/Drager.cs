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
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private Icon iconScript;

    [SerializeField]
    bool isClicked;

    [SerializeField]
    Vector3 originScale;
    [SerializeField]
    Vector3 clickedScale;

    void Awake()
    {
        iconScript = gameObject.GetComponent<Icon>();
        mainCamera = Camera.main;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        isClicked = false;
        cameraDepth = Mathf.Abs(mainCamera.transform.position.z);

        mouseEvent = InputSystem.actions.FindAction("Point");
        clickAction = InputSystem.actions.FindAction("Click");
        clickAction.started += OnDragStart;
        clickAction.canceled += OnDragEnd;
    }

    void OnDisable()
    {
        clickAction.started -= OnDragStart;
        clickAction.canceled -= OnDragEnd;
    }

    void Update()
    {
        if (isClicked == true)
        {
            inputVec = mouseEvent.ReadValue<Vector2>();
            worldPos = mainCamera.ScreenToWorldPoint(new Vector3(inputVec.x, inputVec.y, cameraDepth));
        }
    }

    void FixedUpdate()
    {
        if (isClicked == true)
        {
            Vector3 clampedPos = ClampIconPos(worldPos);
            transform.position = clampedPos;
        }
    }

    private Vector3 ClampIconPos(Vector3 worldPos)
    {
        Rect bounds = GameManager.Instance.BoardBounds;

        float clampedX = Mathf.Clamp(worldPos.x, bounds.xMin, bounds.xMax);
        float clampedY = Mathf.Clamp(worldPos.y, bounds.yMin, bounds.yMax);

        return new Vector3(clampedX, clampedY, worldPos.z);
    }

    private void OnDragStart(InputAction.CallbackContext ctx)
    {
        if (iconScript.IsMerging == true) return;

        Vector2 pos = mouseEvent.ReadValue<Vector2>();
        Vector3 world = mainCamera.ScreenToWorldPoint(new Vector3(pos.x, pos.y, cameraDepth));

        Collider2D hit = Physics2D.OverlapPoint(world);
        if (hit == null || hit.gameObject != gameObject) return;

        spriteRenderer.sortingOrder = 2;
        originScale = transform.localScale;
        transform.localScale = originScale * 1.2f;
        isClicked = true;
    }

    private void OnDragEnd(InputAction.CallbackContext ctx)
    {
        if (isClicked == true)
        {
            isClicked = false;
            transform.localScale = originScale;
            iconScript.SnapToNearestCell();
        }
    }
}
