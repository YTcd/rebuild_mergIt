using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject IconPrefab;
    [SerializeField]
    private GameObject IconContainer;

    public void GenerateIcon()
    {
        Vector2Int ValidPos = GetEmptyGrid();
        GameObject icon = Instantiate(IconPrefab, (Vector3)(Vector2)ValidPos, Quaternion.identity, IconContainer.transform);
        GridHandler.instance.StoreItem(ValidPos, icon);

        Vector2 coodPos = GridHandler.instance.getPosition(ValidPos);
        icon.transform.position = coodPos;

        ImageChanger imageChanger = icon.GetComponent<ImageChanger>();
        imageChanger.init();

        SpriteRenderer sr = icon.GetComponent<SpriteRenderer>();

        float spriteWorldSize = sr.sprite.bounds.size.x;
        float scale = GameManager.instance.TileSpriteSize / spriteWorldSize;
        icon.transform.localScale = new Vector3(scale, scale, 1f);
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
