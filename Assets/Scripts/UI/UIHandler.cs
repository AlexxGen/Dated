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
	
	[SerializeField] private PhoneRenderingScript JessiePhone;
	[SerializeField] private PhoneRenderingScript PeterPhone;
	
	private PhoneRenderingScript Phone {get
		{
			switch (getCurrentCharacter()) {
				case Character.JESSIE:
					return JessiePhone;
				case Character.PETER:
					return PeterPhone;
			}
			throw new ArgumentException("No valid char");
		}
	}
	
	
	[SerializeField] private Image fadeScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeScreen.material.SetFloat("_Fade", 0.0f);
		renderMessagesOnPhones();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapCharacters()
    {
        StartCoroutine(fadeAnim());
    }
	
	private List<Message> messagesToRenderOnPhone(Character character)
	{
		List<Message> toReturn = new List<Message>();
		
		foreach (EventCategory category in Enum.GetValues(typeof(EventCategory))) {
			Message messageToRender = GlobalData.GetMessage(
				character,
				category,
				gameManager.MoodOfCategory(category, character),
				gameManager.curWeek
			);
			
			toReturn.Add(messageToRender);
		}
		
		
		
		
		return toReturn;
		
		
		
	}
	
	private void renderMessagesOnPhones()
	{
		Debug.Log(Phone);
		Debug.Log(JessiePhone);
		Debug.Log(PeterPhone);
		
		JessiePhone.RenderNotifications(messagesToRenderOnPhone(Character.JESSIE));
		PeterPhone.RenderNotifications(messagesToRenderOnPhone(Character.PETER));
	}
	
    public void NextWeek()
    {
		
		Debug.Log("Next Week function running");
		
        pageFlips[0].gameObject.SetActive(true);
        pageFlips[1].gameObject.SetActive(true);
        pageFlips[0].GetComponent<Animator>().Play("notebook", 0, 0.0f);
        pageFlips[1].GetComponent<Animator>().Play("notebook", 0, 0.0f);
        StartCoroutine(waitForPageFlip());
		
		
		renderMessagesOnPhones();
		
    }

    IEnumerator waitForPageFlip()
    {
        gameManager.StatCheck();
        gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.583f);

        pageFlips[0].gameObject.SetActive(false);
        pageFlips[1].gameObject.SetActive(false);

        if (gameManager.curWeek == 4)
        {
            swapCharBtn.gameObject.SetActive(false);
            nextWeekBtn.gameObject.SetActive(false);
        }
		
		
    }

    IEnumerator fadeAnim()
    {

        float timeElapsed = 0.0f;
        float startValue = 0.0f;

        while (timeElapsed < 1.5f)
        {
            float t = timeElapsed / 1.5f;

            float curValue = Mathf.Clamp(Mathf.Lerp(startValue, 1.0f, t), 0.0f, 1.0f);
            Debug.Log("Cur Value: " + curValue);

            fadeScreen.material.SetFloat("_Fade", curValue);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        fadeScreen.material.SetFloat("_Fade", 1.0f);

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
        timeElapsed = 0.0f;

        startValue = 1.0f;

        while (timeElapsed < 3.0f)
        {
            float t = timeElapsed / 3.0f;

            float curValue = Mathf.Clamp(Mathf.Lerp(startValue, 0.0f, t), 0.0f, 1.0f);

            fadeScreen.material.SetFloat("_Fade", curValue);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        fadeScreen.material.SetFloat("_Fade", 0.0f);
    }
}
