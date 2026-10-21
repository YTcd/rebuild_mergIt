using UnityEngine;
using UnityEngine.Pool;

public class PoolingManager : MonoBehaviour
{
    private static PoolingManager instance;
    public static PoolingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<PoolingManager>();
                if (instance == null)
                    instance = new GameObject(nameof(PoolingManager)).AddComponent<PoolingManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Icon iconPrefab;
    [SerializeField]
    private FusionEffect fusionEffect;
    [SerializeField]
    private CreateEffect createEffect;


    private IObjectPool<Icon> itemPool;
    private IObjectPool<FusionEffect> effectPool;
    private IObjectPool<CreateEffect> createEffectPool;

    public IObjectPool<Icon> ItemPool =>
        itemPool ??= new ObjectPool<Icon>(CreatePooledItem, OnTakeItemFromPool, OnReturnedItemToPool, OnDestroyPooledItem);
    public IObjectPool<FusionEffect> EffectPool =>
        effectPool ??= new ObjectPool<FusionEffect>(CreatePooledEffect, OnTakeEffectFromPool, OnReturnedEffectToPool, OnDestroyPooledEffect);
    public IObjectPool<CreateEffect> CreateEffectPool =>
        createEffectPool ??= new ObjectPool<CreateEffect>(CreatePooledCreateEffect, OnTakeCreateEffectFromPool, OnReturnedCreateEffectToPool, OnDestroyPooledCreateEffect);


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
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

    private CreateEffect CreatePooledCreateEffect()
    {
        CreateEffect pooledCreateEffect = Instantiate(createEffect);
        return pooledCreateEffect;
    }

    private void OnTakeCreateEffectFromPool(CreateEffect pooledEffect)
    {
        pooledEffect.gameObject.SetActive(true);
    }

    private void OnReturnedCreateEffectToPool(CreateEffect pooledEffect)
    {
        pooledEffect.gameObject.SetActive(false);
    }

    private void OnDestroyPooledCreateEffect(CreateEffect pooledEffect)
    {
        Destroy(pooledEffect.gameObject);
    }
}
