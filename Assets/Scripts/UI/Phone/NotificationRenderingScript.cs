using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class NotificationRenderingScript : MonoBehaviour
{
	[SerializeField] private Image MessageImage;
	[SerializeField] private TMP_Text HeaderText;
	[SerializeField] private TMP_Text BodyText;
	
	
	[SerializeField] private Sprite JESSIE_TEXT_BACKGROUND;
	[SerializeField] private Sprite PETER_TEXT_BACKGROUND;
	[SerializeField] private Sprite DEFAULT_TEXT_BACKGROUND;
	[SerializeField] private Sprite EMAIL_BACKGROUND;
	
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	private Sprite CorrectBackgroundImage(Message message)
	{
		switch (message.Medium)
		{
			case MessageMedium.TEXT:
				switch (message.Sender.ToLower())
				{
					case "Jessiebear:)":
						return JESSIE_TEXT_BACKGROUND;
					case "peter <3":
						return PETER_TEXT_BACKGROUND;
					default:
						return DEFAULT_TEXT_BACKGROUND;
				}
			case MessageMedium.EMAIL:
				return EMAIL_BACKGROUND;
		}
		throw new ArgumentException("Somehow we got here");
	}
	
	
	public void ShowMessage(Message message)
	{
		MessageImage.sprite = CorrectBackgroundImage(message);
		
		HeaderText.text = message.Sender;
		BodyText.text = message.Text;
		
		Debug.Log("Updated " + message.Sender + ": " + message.Text);
	}
	
}