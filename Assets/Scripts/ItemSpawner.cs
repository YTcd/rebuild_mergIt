using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject IconPrefab;
    [SerializeField]
    private GridHandler GridHandler;
    [SerializeField]
    private Sprite[] Sprites = new Sprite[5];
    [SerializeField]
    private GameObject IconContainer;

    public void GenerateIcon()
    {
        Vector2 ValidPos = GetEmptyGrid();
        GameObject icon = Instantiate(IconPrefab, ValidPos, Quaternion.identity, IconContainer.transform);

        Vector2 coodPos = GameManager.instance.GridPositions[(int)ValidPos.x, (int)ValidPos.y];
        icon.transform.position = coodPos;

        int randIndex = Random.Range(0, 5);
        SpriteRenderer sr = icon.GetComponent<SpriteRenderer>();
        sr.sortingOrder = 2;
        sr.sprite = Sprites[randIndex];

        float spriteWorldSize = sr.sprite.bounds.size.x;
        float scale = GameManager.instance.TileSpriteSize / spriteWorldSize;
        icon.transform.localScale = new Vector3(scale, scale, 1f);
    }

    private Vector2 GetEmptyGrid()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (GridHandler.HasItem(i, j) == false)
                {
                    GridHandler.StoreItem(i, j);
                    return new Vector2(i, j);
                }
            }
        }

        return Vector2.zero;
    }
}
