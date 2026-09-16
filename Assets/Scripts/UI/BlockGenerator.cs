using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private DayColumn[] targetColumns;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Dictionary<EventCategory, Sprite> blockTypes;

    [SerializeField] private DayColumn[] columns; 

    private GlobalData data;
    public void SpawnBlock()
    {
        foreach (DayColumn col in columns)
            col.ClearColumn();

        List<BlockData> blocksJ = new List<BlockData>();
        List<BlockData> blocksP = new List<BlockData>();

        if (blockPrefab == null || targetColumns[0] == null || targetColumns[1] == null)
        {
            Debug.Log("Missing block prefab or DayColumn target");
            return;
        }

        DayColumn curColumn;

        foreach (CalendarEvent timeBlock in GlobalData.events)
        {
            // Debug.Log("Current Week: " +  gameManager.curWeek);
            if(timeBlock.Week != gameManager.curWeek)
            {
                continue;
            }
            if (timeBlock.OwningCharacter == Character.JESSIE)
            {
                curColumn = targetColumns[0];
            }
            else if (timeBlock.OwningCharacter == Character.PETER)
            {
                curColumn = targetColumns[1];
            }
            else
            {
                Debug.Log($"Owning Character not Jessie or Peter!");
                continue;
            }

            Debug.Log($"Current block being spawned: It's {timeBlock.OwningCharacter} \n in week {timeBlock.Week}");

            RectTransform blockRect = blockPrefab.GetComponent<RectTransform>();
            float blockHeight = timeBlock.LengthInMinutes();
            blockRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, blockHeight);
            blockPrefab.GetComponent<Image>().sprite = blockTypes[timeBlock.Category];
            if (blockPrefab.GetComponentInChildren<TextMeshProUGUI>()){
                blockPrefab.GetComponentInChildren<TextMeshProUGUI>().text = timeBlock.Name;
                blockPrefab.GetComponentInChildren<TextMeshProUGUI>().fontSize = Mathf.Clamp(11 * (blockHeight/60), 11, 15);
            }

            if (!curColumn.CanFitBlock(blockHeight))
            {
                Debug.Log($"Column {curColumn.name} is out of space");
                return;
            }

            GameObject newBlock = Instantiate(blockPrefab, curColumn.transform, false);
            BlockData blockData = newBlock.GetComponent<BlockData>();
            blockData.OwningCharacter = timeBlock.OwningCharacter;
            blockData.Name = timeBlock.Name;
            blockData.Category = timeBlock.Category;
            blockData.Week = timeBlock.Week;
            blockData.Days = timeBlock.Days;
            blockData.Length = blockHeight;
            blockData.Dialogue = timeBlock.Dialogue;

            switch (blockData.OwningCharacter)
            {
                case Character.JESSIE:
                    blocksJ.Add(blockData);
                    break;
                case Character.PETER:
                    blocksP.Add(blockData);
                    break;
            }

            curColumn.RefreshLayout();
        }
        gameManager.newBlocksJ = blocksJ;
        gameManager.newBlocksP = blocksP;

    }

    private void Start()
    {
        SpawnBlock();
        Debug.Log("data found first here:" +  GlobalData.events[0].Name);
    }

    public void clearCols()
    {
        foreach (DayColumn col in columns)
            col.ClearColumn();
    }
}
