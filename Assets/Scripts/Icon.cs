using UnityEngine;

public class Icon : MonoBehaviour
{
    [SerializeField]
    private Sprite[] iconSprites = new Sprite[5];

    private SpriteRenderer spriteRenderer;
    private int spriteIndex;
    private Vector2Int gridIndex;
    private GridHandler gridHandler;
    public bool IsMerging;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gridHandler = GridHandler.Instance;
    }

    void OnDisable()
    {
        gridHandler.ReleaseItem(gridIndex);
    }

    public void Init(Vector2Int position)
    {
        gridIndex = position;
        SetIconSprite(0);

        Vector2 coodPos = gridHandler.GetPosition(position);
        transform.position = coodPos;


        spriteRenderer.sortingOrder = 1;
        float spriteWorldSize = spriteRenderer.sprite.bounds.size.x;
        float scale = GameManager.Instance.TileSpriteSize / spriteWorldSize;
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    public void SetIconSprite(int index)
    {
        if (index < 0 || index >= iconSprites.Length) return;

        spriteIndex = index;
        spriteRenderer.sprite = iconSprites[index];
    }

    public int GetSpriteIndex()
    {
        return spriteIndex;
    }

    public void UpgradeItem()
    {
        IsMerging = false;
        spriteIndex++;
        SetIconSprite(spriteIndex);
    }

    public void ReturnToOriginPos()
    {
        transform.position = gridHandler.GetPosition(gridIndex);
    }

    public void SnapToNearestCell()
    {
        Vector2 currentPos = (Vector2)transform.position;
        Vector2 nearestPos = currentPos;
        Vector2Int nearestIndex = new Vector2Int();
        float minValue = float.PositiveInfinity;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Vector2 gridPos = gridHandler.GetPosition(new Vector2Int(i, j));
                float distance = Vector2.Distance(currentPos, gridPos);
                if (distance <= minValue)
                {
                    nearestPos = gridPos;
                    minValue = distance;
                    nearestIndex.x = i;
                    nearestIndex.y = j;
                }
            }
        }

        spriteRenderer.sortingOrder = 1;
        if (nearestIndex == gridIndex)
        {
            ReturnToOriginPos();
        }
        else if (!gridHandler.HasItem(nearestIndex))
        {
            gridHandler.ReleaseItem(gridIndex);
            gridHandler.StoreItem(nearestIndex, gameObject);
            gridIndex = nearestIndex;
            transform.position = (Vector3)nearestPos;
        }
        else if (gridHandler.GetItem(nearestIndex).GetComponent<Icon>().GetSpriteIndex() != spriteIndex
        || spriteIndex == 4 || gridHandler.GetItem(nearestIndex).GetComponent<Icon>().IsMerging == true)
        {
            ReturnToOriginPos();
        }
        else
        {
            gridHandler.ReleaseItem(gridIndex);
            MergeManager.Instance.MergeItem(gameObject, nearestIndex);
        }

    }

    public void SetVisible(bool visible)
    {
        spriteRenderer.enabled = visible;
    }
}
