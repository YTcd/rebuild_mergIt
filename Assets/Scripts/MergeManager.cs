using System.Collections;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void MergeItem(GameObject dragedItem, Vector2Int gridIndex)
    {
        GameObject storedItem = GridHandler.Instance.GetItem(gridIndex);
        StartCoroutine(MergeItem(dragedItem, storedItem));
    }

    private IEnumerator MergeItem(GameObject dragedItem, GameObject storedItem)
    {
        const float duration = 0.2f;

        Vector3 startPos = dragedItem.transform.position;
        Vector3 targetPos = storedItem.transform.position;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            dragedItem.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
        }

        dragedItem.transform.position = targetPos;

        Icon dragedIcon = dragedItem.GetComponent<Icon>();
        PoolingManger.Instance.ReturnItemToPool(dragedIcon);

        Icon storedItemIcon = storedItem.GetComponent<Icon>();
        storedItemIcon.IsMerging = true;
        storedItemIcon.SetVisible(false);
        FusionEffect effect = PoolingManger.Instance.GetFusionEffect();
        yield return StartCoroutine(effect.PlayAndWait(targetPos));

        storedItemIcon.SetVisible(true);
        storedItemIcon.UpgradeItem();
    }
}
