using UnityEngine;

public class Icon : MonoBehaviour
{
    [SerializeField]
    private Sprite[] iconSprites = new Sprite[5];

    private SpriteRenderer SpriteRenderer;
    private int spriteIndex;
    private Vector2Int GridIndex;
    private GridHandler gridHandler;

    void Awake()
    {
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gridHandler = GridHandler.instance;
    }

    void OnDisable()
    {
        gridHandler.ReleaseItem(GridIndex);
    }

    public void Init(Vector2Int position)
    {
        GridIndex = position;
        SetIconSprite(0);

        Vector2 coodPos = gridHandler.getPosition(position);
        transform.position = coodPos;


        SpriteRenderer.sortingOrder = 1;
        float spriteWorldSize = SpriteRenderer.sprite.bounds.size.x;
        float scale = GameManager.instance.TileSpriteSize / spriteWorldSize;
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    public void SetIconSprite(int index)
    {
        if (index < 0 || index >= iconSprites.Length) return;

        spriteIndex = index;
        SpriteRenderer.sprite = iconSprites[index];
    }

    public int GetSpriteIndex()
    {
        return spriteIndex;
    }

    public void UpgradeItem()
    {
        spriteIndex++;
        SetIconSprite(spriteIndex);
    }

    public void returnToOriginPos()
    {
        transform.position = gridHandler.getPosition(GridIndex);
    }

    public void SnapToNearestCell()
    {
        Vector2 CurrentPos = (Vector2)transform.position;
        Vector2 NearestPos = CurrentPos;
        Vector2Int NearestIndex = new Vector2Int();
        float minValue = float.PositiveInfinity;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Vector2 GridPos = gridHandler.getPosition(new Vector2Int(i, j));
                float distance = Vector2.Distance(CurrentPos, GridPos);
                if (distance <= minValue)
                {
                    NearestPos = GridPos;
                    minValue = distance;
                    NearestIndex.x = i;
                    NearestIndex.y = j;
                }
            }
        }

        transform.position = (Vector3)NearestPos;
        SpriteRenderer.sortingOrder = 1;
        MergeManager.instance.TryMerge(gameObject, NearestIndex);
    }
}
