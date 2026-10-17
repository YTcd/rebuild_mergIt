using UnityEngine;

public class AutoMerger : MonoBehaviour
{
    public void AutoMerge()
    {
        GameObject item = null;
        bool hasValidItems = false;
        Vector2Int storedIndex = new Vector2Int(-1, -1);
        int gridSize = GameManager.GridSize;
        for (int i = 0; i < gridSize * gridSize - 1; i++)
        {
            int row = i / gridSize;
            int column = i % gridSize;
            Vector2Int gridIndex = new Vector2Int(row, column);
            item = GridHandler.Instance.GetItem(gridIndex);

            if (item == null) continue;

            Icon icon = item.GetComponent<Icon>();
            if (icon == null || icon.GetSpriteIndex() == 4) continue;

            for (int j = i + 1; j < gridSize * gridSize; j++)
            {
                int c_row = j / gridSize;
                int c_column = j % gridSize;
                Vector2Int c_gridIndex = new Vector2Int(c_row, c_column);
                GameObject compareItem = GridHandler.Instance.GetItem(c_gridIndex);
                if (compareItem == null) continue;
                Icon compareIcon = compareItem.GetComponent<Icon>();

                if (compareIcon == null) continue;
                if (compareIcon.GetSpriteIndex() != icon.GetSpriteIndex()) continue;
                if (compareIcon.IsMerging || icon.IsMerging) continue;


                storedIndex.x = c_row;
                storedIndex.y = c_column;
                hasValidItems = true;
                break;
            }

            if (hasValidItems == true) break;
        }

        if (storedIndex.x == -1 || storedIndex.y == -1) return;
        if (hasValidItems == false) return;
        if (item == null) return;

        item.GetComponent<Icon>().PrepareMerge();
        MergeManager.Instance.MergeItem(item, storedIndex);
    }
}
