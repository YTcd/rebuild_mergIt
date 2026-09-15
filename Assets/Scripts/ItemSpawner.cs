using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject iconContainer;
    [SerializeField]
    private PoolingManger poolingManager;

    public void GenerateIcon()
    {
        Vector2Int validPos = GetEmptyGrid();
        if (validPos.x == -1) return;
        Icon item = poolingManager.GetItem();
        item.gameObject.transform.parent = iconContainer.transform;
        GridHandler.Instance.StoreItem(validPos, item.gameObject);
        Icon icon = item.GetComponent<Icon>();
        icon.Init(validPos);
    }

    private Vector2Int GetEmptyGrid()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Vector2Int newIndex = new Vector2Int(i, j);
                if (GridHandler.Instance.HasItem(newIndex) == false)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }
}
