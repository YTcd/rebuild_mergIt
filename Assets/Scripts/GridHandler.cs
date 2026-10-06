using UnityEngine;

public class GridHandler : MonoBehaviour
{
    public static GridHandler Instance;

    private GameObject[,] cells = new GameObject[GameManager.GridSize, GameManager.GridSize];
    private Vector2[,] gridPositions;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gridPositions = new Vector2[GameManager.GridSize, GameManager.GridSize];
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
