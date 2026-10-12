using UnityEngine;

public class Drager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Icon iconScript;

    [SerializeField]
    Vector3 originScale;
    [SerializeField]
    Vector3 clickedScale;

    public bool CanDrag => !iconScript.IsMerging;

    void Awake()
    {
        iconScript = GetComponent<Icon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnDraged(Vector3 clampedPos)
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

        GameManager.Instance.UIHandler.AddScore(iconScript.GetScore());
        PoolingManger.Instance.ItemPool.Release(iconScript);
    }
}
