using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject IconContainer;
    [SerializeField]
    private PoolingManger PoolingManager;

    public void GenerateIcon()
    {
        Vector2Int ValidPos = GetEmptyGrid();
        Icon item = PoolingManager.GetItem();
        item.gameObject.transform.parent = IconContainer.transform;
        GridHandler.instance.StoreItem(ValidPos, item.gameObject);
        Icon icon = item.GetComponent<Icon>();
        icon.Init(ValidPos);
    }

    private Vector2Int GetEmptyGrid()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Vector2Int newIndex = new Vector2Int(i, j);
                if (GridHandler.instance.HasItem(newIndex) == false)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return Vector2Int.zero;
    }
}
