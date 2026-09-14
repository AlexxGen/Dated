using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int academicStatP = 100;
    //[SerializeField] private int socialStatP;
    [SerializeField] private int extracurricularStatP = 100;
    [SerializeField] private int academicStatJ = 100;
    [SerializeField] private int socialStat = 100;
    [SerializeField] private int extracurricularStatJ = 100;

    public int curWeek;

    //Take the type and count how many of a certain type are still in the new blocks to determine what changes are needed for the stats
    public List<BlockData> newBlocksJ = new List<BlockData>();
    public List<BlockData> newBlocksP = new List<BlockData>();

    [SerializeField] private int freeHoursP;
    [SerializeField] private int freeHoursJ;

    [SerializeField] private DayColumn[] spawnColumns;

    [SerializeField] private BlockGenerator blockGenerator;

    private void OnEnable()
    {
        BlockDrag.onBlockDropped += HandleBlockDropped;
    }

    private void OnDisable()
    {
        BlockDrag.onBlockDropped -= HandleBlockDropped;
    }

    private void HandleBlockDropped(DayColumn targetColumn, BlockData droppedBlock)
    {
        if (targetColumn != spawnColumns[0]  && targetColumn != spawnColumns[1] && (newBlocksJ.Contains(droppedBlock) || newBlocksP.Contains(droppedBlock)))
        {
            switch (droppedBlock.OwningCharacter)
            {
                case Character.PETER:
                    newBlocksP.Remove(droppedBlock);
                    break;
                case Character.JESSIE:
                    newBlocksJ.Remove(droppedBlock);
                    break;
            }
        }

        else if (spawnColumns != null)
        {
            //If does not work try doing it based on target column instead
            switch (droppedBlock.OwningCharacter)
            {
                case Character.PETER:
                    newBlocksP.Add(droppedBlock);
                    break;
                case Character.JESSIE:
                    newBlocksJ.Add(droppedBlock);
                    break;
            }
        }
    }

    public void StatCheck()
    {
        if (curWeek == 4)
        {
            Endings();
            return;
        }
        foreach (BlockData block in newBlocksJ)
        {
            switch (block.Category)
            {
                case EventCategory.ACADEMIC:
                    academicStatJ -= Mathf.FloorToInt(block.Length / 60);
                    break;
                case EventCategory.SOCIAL:
                    socialStat -= Mathf.FloorToInt(block.Length / 60);
                    break;
                case EventCategory.EXTRACURRICULAR:
                    extracurricularStatJ -= Mathf.FloorToInt(block.Length / 60);
                    break;
            }
        }

        foreach (BlockData block in newBlocksP)
        {
            switch (block.Category)
            {
                case EventCategory.ACADEMIC:
                    academicStatP -= Mathf.FloorToInt(block.Length / 60);
                    break;
                case EventCategory.SOCIAL:
                    socialStat -= Mathf.FloorToInt(block.Length / 60);
                    break;
                case EventCategory.EXTRACURRICULAR:
                    extracurricularStatP -= Mathf.FloorToInt(block.Length / 60);
                    break;
            }
        }

        curWeek++;
        blockGenerator.SpawnBlock();

    }

    public void Endings()
    {

    }
}
