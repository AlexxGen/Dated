using System;
using UnityEngine;

public class CalendarEvent
{
	public Character OwningCharacter {get; private set;}
	public String Name {get; private set;}
	public EventCategory Category {get; private set;}
	public EventDate Date {get; private set;}
	public TimeSpan Length {get; private set;}
	public String Dialogue {get; private set;}
	
	
	
    public CalendarEvent(
		Character OwningCharacter,
		String Name,
		EventCategory Category,
		EventDate Date,
		TimeSpan Length,
		String Dialogue
	) { 
		this.OwningCharacter = OwningCharacter;
		this.Name = Name;
		this.Category = Category;
		this.Date = Date;
		this.Length = Length;
		this.Dialogue = Dialogue;
	}
}
