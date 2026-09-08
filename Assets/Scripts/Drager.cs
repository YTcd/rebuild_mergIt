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

    private ImageChanger imageChanger;

    [SerializeField]
    bool isClicked;

    [SerializeField]
    Vector3 originScale;
    [SerializeField]
    Vector3 clickedScale;

    void Awake()
    {
        MainCamera = Camera.main;
    }

    void OnEnable()
    {
        isClicked = false;
        cameraDepth = Mathf.Abs(MainCamera.transform.position.z);

        imageChanger = GetComponent<ImageChanger>();

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
            SnapToNearestCell();
        }
    }

    private void SnapToNearestCell()
    {
        Vector2 CurrentPos = (Vector2)transform.position;
        Vector2 NearestPos = CurrentPos;
        float minValue = float.PositiveInfinity;
        Vector2[,] CellCenterPoints = GameManager.instance.GridPositions;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Vector2 GridPos = CellCenterPoints[i, j];
                float distance = Vector2.Distance(CurrentPos, GridPos);
                if (distance <= minValue)
                {
                    NearestPos = GridPos;
                    minValue = distance;
                }
            }
        }

        transform.position = (Vector3)NearestPos;
    }
}
