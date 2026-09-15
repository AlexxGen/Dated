using UnityEngine;

public class PhoneRenderingScript : MonoBehaviour
{
	
	[SerializeField] private NotificationRenderingScript topNotification;
	[SerializeField] private NotificationRenderingScript bottomNotification;
	
	
	
	void Start()
	{
		
	}

	void Update()
	{
		
	}
	
	public void RenderNotifications(Message first, Message second)
	{
		if (first.LessThan(second))
		{
			Message temp = second;
			second = first;
			first = temp;
		}
		
		topNotification.ShowMessage(first);
		bottomNotification.ShowMessage(second);
		
	}
}