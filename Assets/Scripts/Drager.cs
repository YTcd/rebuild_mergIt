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
    private Camera MainCamera;
    private SpriteRenderer spriteRenderer;
    private Icon MainScript;

    [SerializeField]
    bool isClicked;

    [SerializeField]
    Vector3 originScale;
    [SerializeField]
    Vector3 clickedScale;

    void Awake()
    {
        MainScript = gameObject.GetComponent<Icon>();
        MainCamera = Camera.main;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        isClicked = false;
        cameraDepth = Mathf.Abs(MainCamera.transform.position.z);

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
            worldPos = MainCamera.ScreenToWorldPoint(new Vector3(inputVec.x, inputVec.y, cameraDepth));
        }
    }

    void FixedUpdate()
    {
        if (isClicked == true)
        {
            Vector3 ClampedPos = ClampIconPos(worldPos);
            transform.position = ClampedPos;
        }
    }

    private Vector3 ClampIconPos(Vector3 worldPos)
    {
        Rect bounds = GameManager.instance.BoardBounds;

        float clampedX = Mathf.Clamp(worldPos.x, bounds.xMin, bounds.xMax);
        float clampedY = Mathf.Clamp(worldPos.y, bounds.yMin, bounds.yMax);

        return new Vector3(clampedX, clampedY, worldPos.z);
    }

    private void OnDragStart(InputAction.CallbackContext ctx)
    {
        Vector2 pos = mouseEvent.ReadValue<Vector2>();
        Vector3 world = MainCamera.ScreenToWorldPoint(new Vector3(pos.x, pos.y, cameraDepth));

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
            MainScript.SnapToNearestCell();
        }
    }
}
