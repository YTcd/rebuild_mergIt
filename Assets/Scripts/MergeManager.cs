using System.Collections;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager instance;

    void Awake()
    {
        instance = this;
    }

    public void MergeItem(GameObject dragedItem, Vector2Int GridIndex)
    {
        GameObject storedItem = GridHandler.instance.getItem(GridIndex);
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
        PoolingManger.instance.returnItemToPool(dragedIcon);

        storedItem.GetComponent<Icon>().UpgradeItem();
    }
}
