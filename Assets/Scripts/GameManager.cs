using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<GameManager>();
                if (instance == null)
                    instance = new GameObject(nameof(GameManager)).AddComponent<GameManager>();
            }
            return instance;
        }
    }
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Sprite tileSprite;
    [SerializeField]
    private GameObject board;
    [SerializeField]
    private RectTransform mergeButton;
    [SerializeField]
    private float buttonMargin = 24f;
    [SerializeField, Range(0.1f, 0.4f)]
    private float maxStripFraction = 0.22f;
    public UIHandler UiHandler;

    public const int GridSize = 9;

    private float boardSize;
    private int lastWidth, lastHeight;

    public float TileSpriteSize = 74f;
    public Rect BoardBounds;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        boardSize = TileSpriteSize * GridSize;
        Fit();
        GenerateBoard();
    }

    void Update()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight)
            Fit();
    }

    private void Fit()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;

        float w = Screen.width;
        float h = Screen.height;
        bool landscape = w >= h;
        float longAxis = Mathf.Max(w, h);
        float shortAxis = Mathf.Min(w, h);

        float freePixels = longAxis - shortAxis;

        Vector2 btnSize = mergeButton != null ? mergeButton.sizeDelta : new Vector2(160f, 31f);
        float needed = (landscape ? btnSize.x : btnSize.y) + 2f * buttonMargin;

        float boardScreenSize;
        float buttonScale;
        if (freePixels >= needed)
        {
            boardScreenSize = shortAxis;
            buttonScale = 1f;
        }
        else
        {
            float stripPixels = Mathf.Min(needed, longAxis * maxStripFraction);
            boardScreenSize = longAxis - stripPixels;
            buttonScale = Mathf.Min(1f, stripPixels / needed);
        }

        float worldPerPixel = boardSize / boardScreenSize;
        mainCamera.orthographicSize = worldPerPixel * h / 2f;

        Vector3 camPos = mainCamera.transform.position;
        if (landscape)
        {
            camPos.x = (w / 2f - boardScreenSize / 2f) * worldPerPixel;
            camPos.y = 0f;
        }
        else
        {
            camPos.x = 0f;
            camPos.y = -(h / 2f - boardScreenSize / 2f) * worldPerPixel;
        }
        mainCamera.transform.position = camPos;

        LayoutButton(landscape, w, h, boardScreenSize, buttonScale);
    }

    private void LayoutButton(bool landscape, float w, float h, float boardScreenSize, float scale)
    {
        if (mergeButton == null) return;

        mergeButton.anchorMin = Vector2.zero;
        mergeButton.anchorMax = Vector2.zero;
        mergeButton.pivot = new Vector2(0.5f, 0.5f);

        mergeButton.anchoredPosition = landscape
            ? new Vector2((boardScreenSize + w) / 2f, h / 2f)
            : new Vector2(w / 2f, (h - boardScreenSize) / 2f);
        mergeButton.localScale = new Vector3(scale, scale, 1f);
    }

    private void GenerateBoard()
    {
        float spriteWorldSize = tileSprite.bounds.size.x;
        float scale = TileSpriteSize / spriteWorldSize;
        float offset = (GridSize - 1) / 2f;

        float halfBoardSize = boardSize / 2f;
        BoardBounds = new Rect(-halfBoardSize, -halfBoardSize, boardSize, boardSize);

        for (int x = 0; x < GridSize; x++)
        {
            for (int y = 0; y < GridSize; y++)
            {
                Vector2 pos = new Vector2((x - offset) * TileSpriteSize, (y - offset) * TileSpriteSize);
                GridHandler.Instance.SetPosition(x, y, pos);

                GameObject tile = new GameObject($"Tile_{x}_{y}");
                tile.transform.SetParent(board.transform);
                tile.transform.position = pos;
                tile.transform.localScale = new Vector3(scale, scale, 1f);

                SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
                sr.sprite = tileSprite;
                sr.sortingOrder = 0;
            }
        }
    }
}
