using System.Collections;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    private static MergeManager instance;
    public static MergeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<MergeManager>();
                if (instance == null)
                    instance = new GameObject(nameof(MergeManager)).AddComponent<MergeManager>();
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void MergeItem(GameObject draggedItem, Vector2Int gridIndex)
    {
        GameObject storedItem = GridHandler.Instance.GetItem(gridIndex);
        StartCoroutine(DirectMerging(draggedItem, storedItem));
    }

    private IEnumerator DirectMerging(GameObject draggedItem, GameObject storedItem)
    {
        const float duration = 0.2f;

        Icon storedItemIcon = storedItem.GetComponent<Icon>();
        storedItemIcon.IsMerging = true;

        Vector3 startPos = draggedItem.transform.position;
        Vector3 targetPos = storedItem.transform.position;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            draggedItem.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
        }

        draggedItem.transform.position = targetPos;

        Icon draggedIcon = draggedItem.GetComponent<Icon>();
        PoolingManager.Instance.ItemPool.Release(draggedIcon);

        storedItemIcon.SetVisible(false);
        FusionEffect effect = PoolingManager.Instance.EffectPool.Get();
        yield return StartCoroutine(effect.PlayAndWait(targetPos));

        GameManager.Instance.UiHandler.AddScore(storedItemIcon.GetScore());
        storedItemIcon.SetVisible(true);
        storedItemIcon.UpgradeItem();
    }
}
