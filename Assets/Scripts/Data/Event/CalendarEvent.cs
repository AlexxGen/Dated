using System;
using UnityEngine;

public class CalendarEvent
{
	public Character OwningCharacter {get; private set;}
	public String Name {get; private set;}
	public EventCategory Category {get; private set;}
	public int Week {get; private set;}
	public Weekdays Days {get; private set;}
	public TimeSpan Length {get; private set;}
	public String Dialogue {get; private set;}
	
	
	
    public CalendarEvent(
		Character OwningCharacter,
		String Name,
		EventCategory Category,
		int Week,
		Weekdays Days,
		TimeSpan Length,
		String Dialogue
	) { 
		this.OwningCharacter = OwningCharacter;
		this.Name = Name;
		this.Category = Category;
		this.Week = Week;
		this.Days = Days;
		this.Length = Length;
		this.Dialogue = Dialogue;
	}
}
