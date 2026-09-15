using UnityEngine;

public class PoolingManger : MonoBehaviour
{
    public static PoolingManger instance;

    struct IconInfo
    {
        public Icon icon;
        public bool isUsing;
    }

    struct FusionEffectInfo
    {
        public FusionEffect effect;
        public bool isUsing;
    }

    [SerializeField]
    private Icon IconPrefab;
    [SerializeField]
    private FusionEffect FusionEffect;

    IconInfo[] IconInfos = new IconInfo[9 * 9 + 1];
    FusionEffectInfo[] Effects = new FusionEffectInfo[10];

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
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

    public FusionEffect getFusionEffect()
    {
        int firstEmptySlot = -1;
        for (int i = 0; i < Effects.Length; i++)
        {
            if (Effects[i].effect == null)
            {
                if (firstEmptySlot == -1)
                    firstEmptySlot = i;
                continue;
            }

            if (!Effects[i].isUsing)
            {
                Effects[i].isUsing = true;
                Effects[i].effect.gameObject.SetActive(true);
                return Effects[i].effect;
            }
        }

        FusionEffect GeneratedItem = Instantiate(FusionEffect, Vector3.zero, Quaternion.identity);
        GeneratedItem.gameObject.SetActive(true);
        Effects[firstEmptySlot].effect = GeneratedItem;
        Effects[firstEmptySlot].isUsing = true;
        return GeneratedItem;
    }

    public void returnEffectToPool(FusionEffect effect)
    {
        for (int i = 0; i < Effects.Length; i++)
        {
            if (Effects[i].effect == effect)
            {
                Effects[i].isUsing = false;
                Effects[i].effect.gameObject.SetActive(false);
                return;
            }
        }
    }
}
