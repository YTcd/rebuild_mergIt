using UnityEngine;

public class PoolingManger : MonoBehaviour
{
    public static PoolingManger Instance;

    struct IconInfo
    {
        public Icon Icon;
        public bool IsUsing;
    }

    struct FusionEffectInfo
    {
        public FusionEffect Effect;
        public bool IsUsing;
    }

    [SerializeField]
    private Icon iconPrefab;
    [SerializeField]
    private FusionEffect fusionEffect;

    private IconInfo[] iconInfos = new IconInfo[9 * 9 + 1];
    private FusionEffectInfo[] effects = new FusionEffectInfo[10];

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Icon GetItem()
    {
        int firstEmptySlot = -1;
        for (int i = 0; i < iconInfos.Length; i++)
        {
            if (iconInfos[i].Icon == null)
            {
                if (firstEmptySlot == -1)
                    firstEmptySlot = i;
                continue;
            }

            if (!iconInfos[i].IsUsing)
            {
                iconInfos[i].IsUsing = true;
                iconInfos[i].Icon.gameObject.SetActive(true);
                return iconInfos[i].Icon;
            }
        }

        Icon generatedItem = Instantiate(iconPrefab, Vector3.zero, Quaternion.identity);
        iconInfos[firstEmptySlot].Icon = generatedItem;
        iconInfos[firstEmptySlot].IsUsing = true;
        generatedItem.gameObject.SetActive(true);
        return generatedItem;
    }

    public void ReturnItemToPool(Icon icon)
    {
        for (int i = 0; i < iconInfos.Length; i++)
        {
            if (iconInfos[i].Icon == icon)
            {
                iconInfos[i].IsUsing = false;
                iconInfos[i].Icon.gameObject.SetActive(false);
                return;
            }
        }
    }

    public FusionEffect GetFusionEffect()
    {
        int firstEmptySlot = -1;
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].Effect == null)
            {
                if (firstEmptySlot == -1)
                    firstEmptySlot = i;
                continue;
            }

            if (!effects[i].IsUsing)
            {
                effects[i].IsUsing = true;
                effects[i].Effect.gameObject.SetActive(true);
                return effects[i].Effect;
            }
        }

        FusionEffect generatedItem = Instantiate(fusionEffect, Vector3.zero, Quaternion.identity);
        generatedItem.gameObject.SetActive(true);
        effects[firstEmptySlot].Effect = generatedItem;
        effects[firstEmptySlot].IsUsing = true;
        return generatedItem;
    }

    public void ReturnEffectToPool(FusionEffect effect)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].Effect == effect)
            {
                effects[i].IsUsing = false;
                effects[i].Effect.gameObject.SetActive(false);
                return;
            }
        }
    }
}
