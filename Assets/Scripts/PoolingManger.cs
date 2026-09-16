using UnityEngine;
using UnityEngine.Pool;

public class PoolingManger : MonoBehaviour
{
    public static PoolingManger Instance;

    [SerializeField]
    private Icon iconPrefab;
    [SerializeField]
    private FusionEffect fusionEffect;

    public IObjectPool<Icon> ItemPool { get; private set; }
    public IObjectPool<FusionEffect> EffectPool { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        ItemPool = new ObjectPool<Icon>(CreatePooledItem, OnTakeItemFromPool, OnReturnedItemToPool, OnDestroyPooledItem);
        EffectPool = new ObjectPool<FusionEffect>(CreatePooledEffect, OnTakeEffectFromPool, OnReturnedEffectToPool, OnDestroyPooledEffect);
    }

    private Icon CreatePooledItem()
    {
        Icon poolItem = Instantiate(iconPrefab);
        return poolItem;
    }

    private void OnTakeItemFromPool(Icon icon)
    {
        icon.gameObject.SetActive(true);
    }

    private void OnReturnedItemToPool(Icon icon)
    {
        icon.gameObject.SetActive(false);
    }

    private void OnDestroyPooledItem(Icon pooledItem)
    {
        Destroy(pooledItem.gameObject);
    }

    private FusionEffect CreatePooledEffect()
    {
        FusionEffect pooledEffect = Instantiate(fusionEffect);
        return pooledEffect;
    }

    private void OnTakeEffectFromPool(FusionEffect pooledEffect)
    {
        pooledEffect.gameObject.SetActive(true);
    }

    private void OnReturnedEffectToPool(FusionEffect pooledEffect)
    {
        pooledEffect.gameObject.SetActive(false);
    }

    private void OnDestroyPooledEffect(FusionEffect pooledEffect)
    {
        Destroy(pooledEffect.gameObject);
    }
}
