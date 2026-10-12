using UnityEngine;

public class Dragger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Icon iconScript;

    private Vector3 originScale;

    public bool CanDrag => !iconScript.IsMerging;

    void Awake()
    {
        iconScript = GetComponent<Icon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnDragged(Vector3 clampedPos)
    {
        transform.position = clampedPos;
    }

    public void OnDragStart()
    {
        spriteRenderer.sortingOrder = 3;
        originScale = transform.localScale;
        transform.localScale = originScale * 1.2f;
    }

    public void OnDragEnd()
    {
        transform.localScale = originScale;
        iconScript.SnapToNearestCell();
    }

    public void OnDoubleClicked()
    {
        if (iconScript.GetSpriteIndex() != 4) return;

        GameManager.Instance.UiHandler.AddScore(iconScript.GetScore());
        PoolingManager.Instance.ItemPool.Release(iconScript);
    }
}
