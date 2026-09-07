using UnityEngine;

public class ImageChanger : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite[] iconSprites = new Sprite[5];

    public void SetIcon(int index)
    {
        if (index < 0 || index >= iconSprites.Length) return;

        spriteRenderer.sprite = iconSprites[index];
    }
}
