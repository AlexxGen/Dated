using UnityEngine;
using UnityEngine.UI;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private DayColumn[] targetColumns;

    public GameObject SpawnBlock(DayColumn column)
    {
        if (blockPrefab == null || column == null)
        {
            Debug.Log("Missing block prefab or DayColumn target");
            return null;
        }

        RectTransform blockRect = blockPrefab.GetComponent<RectTransform>();
        float blockHeight = blockRect != null ? blockRect.rect.height : 0f;

        if (!column.CanFitBlock(blockHeight))
        {
            Debug.Log($"Column {column.name} is out of space");
            return null;
        }

        GameObject newBlock = Instantiate(blockPrefab, column.transform, false);

        column.RefreshLayout();

        return newBlock;
        
    }

    private void Start()
    {
        SpawnBlock(targetColumns[0]);
    }
}
