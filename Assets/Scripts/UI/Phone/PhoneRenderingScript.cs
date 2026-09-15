using System.Collections.Generic;
using UnityEngine;

public class PhoneRenderingScript : MonoBehaviour
{
	
	[SerializeField] private NotificationRenderingScript[] notificationSlots;	
	
	
	void Start()
	{
		
	}

	void Update()
	{
		
	}
	
	public void RenderNotifications(List<Message> toShow)
	{
		toShow.Sort(delegate(Message m1, Message m2)
		{
			if (m1.LessThan(m2))
			{
				return -1;
			}
			else
			{
				return 1;
			}
		});
		
		for (int i = 0; i < toShow.Count && i < notificationSlots.Length; ++i)
		{
			notificationSlots[i].ShowMessage(toShow[i]);
		}
		
		
		
	}
}
