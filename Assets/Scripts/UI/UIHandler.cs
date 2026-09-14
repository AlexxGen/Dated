using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Button nextWeekBtn;
    [SerializeField] private Button swapCharBtn;
    [SerializeField] private int curCharacter;
    [SerializeField] private GameObject[] planners; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapCharacters()
    {
        if (curCharacter == 0)
        {
            planners[0].gameObject.SetActive(false);
            planners[1].gameObject.SetActive(true);
            curCharacter = 1;
        }
        else
        {
            planners[1].gameObject.SetActive(false);
            planners[0].gameObject.SetActive(true);
            curCharacter = 0;
        }
    }

    public void NextWeek()
    {
        gameManager.StatCheck();
    }
}
