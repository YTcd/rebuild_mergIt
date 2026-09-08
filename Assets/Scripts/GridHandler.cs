using UnityEngine;

public class GridHandler : MonoBehaviour
{
    public static GridHandler instance;

    private GameObject[,] Cells = new GameObject[9, 9];
    private Vector2[,] GridPositions;

    void Awake()
    {
        instance = this;
        GridPositions = new Vector2[9, 9];
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Cells[i, j] = null;
            }
        }
    }

    public void setPosition(int x, int y, Vector2 pos)
    {
        GridPositions[x, y] = pos;
    }

    public Vector2 getPosition(Vector2Int GridIndex)
    {
        return GridPositions[GridIndex.x, GridIndex.y];
    }

    public void StoreItem(Vector2Int GridIndex, GameObject gameObject)
    {
        if (!IsValidIndex(GridIndex)) return;
        Cells[GridIndex.x, GridIndex.y] = gameObject;
    }

    public void ReleaseItem(Vector2Int GridIndex)
    {
        if (!IsValidIndex(GridIndex)) return;
        Cells[GridIndex.x, GridIndex.y] = null;
    }

    public bool HasItem(Vector2Int GridIndex)
    {
        if (!IsValidIndex(GridIndex)) return false;
        return Cells[GridIndex.x, GridIndex.y] != null;
    }

    public GameObject getItem(Vector2Int GridIndex)
    {
        return Cells[GridIndex.x, GridIndex.y];
    }

    private bool IsValidIndex(Vector2Int GridIndex)
    {
        return GridIndex.x >= 0 && GridIndex.x < 9 && GridIndex.y >= 0 && GridIndex.y < 9;
    }
}
