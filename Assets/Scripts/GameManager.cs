using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float TileSpriteSize = 74f;
    [SerializeField]
    private Camera MainCamera;
    [SerializeField]
    private Sprite tileSprite;
    [SerializeField]
    private GameObject Board;

    private const int GridSize = 9;

    private float BoardSize;
    private int lastWidth, lastHeight;

    public Vector2[,] GridPositions;

    void Awake()
    {
        BoardSize = TileSpriteSize * GridSize;
        GridPositions = new Vector2[9, 9];
        Fit();
        GenerateBoard();
        GenerateButton();
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
        MainCamera.orthographicSize = aspect >= 1f ? BoardSize / 2f
            : BoardSize / (2f * aspect);
    }

    private void GenerateBoard()
    {
        float spriteWorldSize = tileSprite.bounds.size.x;
        float scale = TileSpriteSize / spriteWorldSize;
        float offset = (GridSize - 1) / 2f;

        for (int x = 0; x < GridSize; x++)
        {
            for (int y = 0; y < GridSize; y++)
            {
                Vector2 pos = new Vector2((x - offset) * TileSpriteSize, (y - offset) * TileSpriteSize);
                GridPositions[x, y] = pos;

                GameObject tile = new GameObject($"Tile_{x}_{y}");
                tile.transform.SetParent(Board.transform);
                tile.transform.position = pos;
                tile.transform.localScale = new Vector3(scale, scale, 1f);

                SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
                sr.sprite = tileSprite;
                sr.sortingOrder = 0;
            }
        }
    }

    private void GenerateButton()
    {
        float aspect = (float)Screen.width / Screen.height;
        if (aspect > 1)
        {

        }
        else
        {

        }
    }
}
