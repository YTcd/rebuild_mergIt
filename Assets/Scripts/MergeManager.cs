using System.Collections;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager instance;

    void Awake()
    {
        instance = this;
    }

    public void TryMerge(GameObject dragedItem, Vector2Int GridIndex)
    {
        if (!GridHandler.instance.HasItem(GridIndex)) return;

        int dragedItemIndex = dragedItem.GetComponent<ImageChanger>().IconIndex;

        GameObject storedItem = GridHandler.instance.getItem(GridIndex);
        int storedItemIndex = storedItem.GetComponent<ImageChanger>().IconIndex;

        if (dragedItemIndex != storedItemIndex) return;

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
        Destroy(dragedItem);

        ImageChanger storedImageChanger = storedItem.GetComponent<ImageChanger>();
        storedImageChanger.SetIcon(storedImageChanger.IconIndex + 1);
    }
}
