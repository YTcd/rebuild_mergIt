using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Sprite tileSprite;
    [SerializeField]
    private GameObject board;
    [SerializeField]
    private PoolingManger poolingManger;

    private const int GridSize = 9;

    private float boardSize;
    private int lastWidth, lastHeight;

    public float TileSpriteSize = 74f;
    public Rect BoardBounds;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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

        float aspect = (float)Screen.width / Screen.height;
        mainCamera.orthographicSize = aspect >= 1f ? boardSize / 2f
            : boardSize / (2f * aspect);
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
