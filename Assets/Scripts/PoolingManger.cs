using UnityEngine;

public class PoolingManger : MonoBehaviour
{
    public static PoolingManger instance;

    struct IconInfo
    {
        public Icon icon;
        public bool isUsing;
    }

    [SerializeField]
    Icon IconPrefab;

    IconInfo[] IconInfos = new IconInfo[9 * 9 + 1];

    void Awake()
    {
        instance = this;
    }

    public Icon GetItem()
    {
        int firstEmptySlot = -1;
        for (int i = 0; i < IconInfos.Length; i++)
        {
            if (IconInfos[i].icon == null)
            {
                if (firstEmptySlot == -1)
                    firstEmptySlot = i;
                continue;
            }

            if (!IconInfos[i].isUsing)
            {
                IconInfos[i].isUsing = true;
                IconInfos[i].icon.gameObject.SetActive(true);
                return IconInfos[i].icon;
            }
        }

        Icon GeneratedItem = Instantiate(IconPrefab, Vector3.zero, Quaternion.identity);
        IconInfos[firstEmptySlot].icon = GeneratedItem;
        IconInfos[firstEmptySlot].isUsing = true;
        GeneratedItem.gameObject.SetActive(true);
        return GeneratedItem;
    }

    public void returnItemToPool(Icon icon)
    {
        for (int i = 0; i < IconInfos.Length; i++)
        {
            if (IconInfos[i].icon == icon)
            {
                IconInfos[i].isUsing = false;
                IconInfos[i].icon.gameObject.SetActive(false);
                return;
            }
        }
    }
}
