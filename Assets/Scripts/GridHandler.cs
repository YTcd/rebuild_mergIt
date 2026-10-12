using UnityEngine;

public class GridHandler : MonoBehaviour
{
    private static GridHandler instance;
    public static GridHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<GridHandler>();
                if (instance == null)
                    instance = new GameObject(nameof(GridHandler)).AddComponent<GridHandler>();
            }
            return instance;
        }
    }

    private GameObject[,] cells = new GameObject[GameManager.GridSize, GameManager.GridSize];
    private Vector2[,] gridPositions = new Vector2[GameManager.GridSize, GameManager.GridSize];
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void SetPosition(int x, int y, Vector2 pos)
    {
        gridPositions[x, y] = pos;
    }

    public Vector2 GetPosition(Vector2Int gridIndex)
    {
        return gridPositions[gridIndex.x, gridIndex.y];
    }

    public void StoreItem(Vector2Int gridIndex, GameObject item)
    {
        if (!IsValidIndex(gridIndex)) return;
        cells[gridIndex.x, gridIndex.y] = item;
    }

    public void ReleaseItem(Vector2Int gridIndex)
    {
        if (!IsValidIndex(gridIndex)) return;
        cells[gridIndex.x, gridIndex.y] = null;
    }

    public bool HasItem(Vector2Int gridIndex)
    {
        if (!IsValidIndex(gridIndex)) return false;
        return cells[gridIndex.x, gridIndex.y] != null;
    }

    public GameObject GetItem(Vector2Int gridIndex)
    {
        return cells[gridIndex.x, gridIndex.y];
    }

    private bool IsValidIndex(Vector2Int gridIndex)
    {
        return gridIndex.x >= 0 && gridIndex.x < GameManager.GridSize && gridIndex.y >= 0 && gridIndex.y < GameManager.GridSize;
    }
}
