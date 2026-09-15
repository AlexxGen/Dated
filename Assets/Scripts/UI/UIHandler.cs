using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Button nextWeekBtn;
    [SerializeField] private Button swapCharBtn;
    [SerializeField] private GameObject[] pageFlips;
    [SerializeField] private int curCharacter;
    [SerializeField] private GameObject[] planners;
    [SerializeField] private Image fadeScreen;

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
        StartCoroutine(fadeAnim());
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

        pageFlips[0].gameObject.SetActive(true);
        pageFlips[1].gameObject.SetActive(true);
        pageFlips[0].GetComponent<Animator>().Play("notebook");
        pageFlips[0].GetComponent<Animator>().Play("notebook");
        StartCoroutine(waitForPageFlip());
    }

    IEnumerator waitForPageFlip()
    {
        gameManager.StatCheck();
        yield return new WaitForSeconds(0.583f);

        pageFlips[0].gameObject.SetActive(false);
        pageFlips[1].gameObject.SetActive(false);
    }

    IEnumerator fadeAnim()
    {

        float timeElapsed = 0.0f;

        float oldValue = 0.0f;

        while (timeElapsed < 1)
        {
            float t = timeElapsed / 1;

            float curValue = Mathf.Lerp(oldValue, 1.0f, t);

            fadeScreen.material.SetFloat("_Fade", curValue);

            oldValue = curValue;
        }

        timeElapsed = 0.0f;

        while (timeElapsed < 1)
        {
            float t = timeElapsed / 1;

            float curValue = Mathf.Lerp(oldValue, 0.0f, t);

            fadeScreen.material.SetFloat("_Fade", curValue);

            oldValue = curValue;
        }

        yield return null;
    }
}
