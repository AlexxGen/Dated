using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Button nextWeekBtn;
    [SerializeField] private Button swapCharBtn;
    [SerializeField] private GameObject[] pageFlips;
    [SerializeField] private int curCharacter;
	private Character getCurrentCharacter()
	{
		switch (curCharacter)
		{
			case 0:
				return Character.JESSIE;
			case 1:
				return Character.PETER;
		}
		throw new ArgumentException("Current character is neither 0 nor 1");
	}
    [SerializeField] private GameObject[] planners; 
	
	[SerializeField] private PhoneRenderingScript phone;
	
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
	
	private List<Message> messagesToRenderOnPhone()
	{
		List<Message> toReturn = new List<Message>();
		
		foreach (EventCategory category in Enum.GetValues(typeof(EventCategory))) {
			Character currentCharacter = Character.JESSIE;
			GlobalData.GetMessage(
				getCurrentCharacter(),
				category,
				gameManager.MoodOfCategory(category, getCurrentCharacter()),
				gameManager.curWeek
			);
		}
		
		
		
		
		return toReturn;
		
		
		
	}
	
	private void renderMessagesOnPhone()
	{
		phone.RenderNotifications(messagesToRenderOnPhone());
	}
	
    public void NextWeek()
    {
        pageFlips[0].gameObject.SetActive(true);
        pageFlips[1].gameObject.SetActive(true);
        pageFlips[0].GetComponent<Animator>().Play("notebook");
        pageFlips[0].GetComponent<Animator>().Play("notebook");
        StartCoroutine(waitForPageFlip());
		
		
		
    }

    IEnumerator waitForPageFlip()
    {
        yield return new WaitForSeconds(0.75f);

        gameManager.StatCheck();
        pageFlips[0].gameObject.SetActive(false);
        pageFlips[1].gameObject.SetActive(false);
    }
}
