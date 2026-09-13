using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int gradeStatP;
    [SerializeField] private int socialStatP;
    [SerializeField] private int extraStatP;
    [SerializeField] private int gradeStatJ;
    [SerializeField] private int socialStatJ;
    [SerializeField] private int extraStatJ;

    [SerializeField] private int curWeek;

    //Take the type and count how many of a certain type are still in the new blocks to determine what changes are needed for the stats
    //[SerializeField] private Block []newBlocksP;
    //[SerializeField] private Block []newBlocksJ;

    [SerializeField] private int freeHoursP;
    [SerializeField] private int freeHoursJ;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
