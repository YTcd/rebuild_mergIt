using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject iconContainer;
    [SerializeField]
    private PoolingManager poolingManager;
    [SerializeField]
    private int[] levelThresholds = { 100, 500 };
    private Vector2Int[] emptyCells;

    void Awake()
    {
        emptyCells = new Vector2Int[GameManager.GridSize * GameManager.GridSize];
    }

    public void GenerateIcon()
    {
        Vector2Int validPos = GetEmptyGrid();
        if (validPos.x == -1) return;
        Icon item = poolingManager.ItemPool.Get();
        item.transform.SetParent(iconContainer.transform);
        GridHandler.Instance.StoreItem(validPos, item.gameObject);
        item.SetReady(false);
        StartCoroutine(SpawnRoutine(item, validPos));
    }

    private IEnumerator SpawnRoutine(Icon item, Vector2Int validPos)
    {
        CreateEffect effect = PoolingManager.Instance.CreateEffectPool.Get();
        yield return effect.PlayAndWait(GridHandler.Instance.GetPosition(validPos));
        item.Init(validPos, GetSpawnLevel());
        item.SetReady(true);
    }

    private int GetSpawnLevel()
    {
        int score = GameManager.Instance.UiHandler.Current;
        int level = 0;
        foreach (int threshold in levelThresholds)
            if (score >= threshold) level++;

        return Mathf.Min(level, 3);
    }

    private Vector2Int GetEmptyGrid()
    {
        int emptyCellsCount = 0;
        for (int i = 0; i < GameManager.GridSize; i++)
        {
            for (int j = 0; j < GameManager.GridSize; j++)
            {
                Vector2Int newIndex = new Vector2Int(i, j);
                if (GridHandler.Instance.HasItem(newIndex) == false)
                {
                    emptyCells[emptyCellsCount] = newIndex;
                    emptyCellsCount++;
                }
            }
        }

        if (emptyCellsCount == 0) return new Vector2Int(-1, -1);

        int randomIndex = Random.Range(0, emptyCellsCount);
        return emptyCells[randomIndex];
    }
}
