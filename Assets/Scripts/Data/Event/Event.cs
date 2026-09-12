using System;
using UnityEngine;

public class CalendarEvent
{
	public Character OwningCharacter {get; private set;}
	public String Name {get; private set;}
	public EventCategory Category {get; private set;}
	public EventDate Date {get; private set;}
	public TimeOfDay StartTime {get; private set;}
	public TimeOfDay EndTime {get; private set;}
	public String Dialogue {get; private set;}
	
	
	
    public CalendarEvent(
		Character OwningCharacter,
		String Name,
		EventCategory Category,
		EventDate Date,
		TimeOfDay StartTime,
		TimeOfDay EndTime,
		String Dialogue
	) { 
		this.OwningCharacter = OwningCharacter;
		this.Name = Name;
		this.Category = Category;
		this.Date = Date;
		this.StartTime = StartTime;
		this.EndTime = EndTime;
		this.Dialogue = Dialogue;
	}
}
