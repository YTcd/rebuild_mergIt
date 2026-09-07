using UnityEngine;

public class GridHandler : MonoBehaviour
{
    private bool[,] hasItemInCell = new bool[9, 9];

    void Awake()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                hasItemInCell[i, j] = false;
            }
        }
    }

    public void StoreItem(int x, int y)
    {
        if (!IsValidIndex(x, y)) return;
        hasItemInCell[x, y] = true;
    }

    public void ReleaseItem(int x, int y)
    {
        if (!IsValidIndex(x, y)) return;
        hasItemInCell[x, y] = false;
    }

    public bool HasItem(int x, int y)
    {
        if (!IsValidIndex(x, y)) return false;
        return hasItemInCell[x, y];
    }

    private bool IsValidIndex(int x, int y)
    {
        return x > 0 && x < 9 && y > 0 && y < 9;
    }
}
