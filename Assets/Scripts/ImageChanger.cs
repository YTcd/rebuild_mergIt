using UnityEngine;

public class ImageChanger : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite[] iconSprites = new Sprite[5];

    public int IconIndex;

    public void init()
    {
        IconIndex = 0;
        spriteRenderer.sortingOrder = 1;
        spriteRenderer.sprite = iconSprites[0];
    }

    public void SetIcon(int index)
    {
        if (index < 0 || index >= iconSprites.Length) return;

        IconIndex = index;
        spriteRenderer.sprite = iconSprites[index];
    }
}
