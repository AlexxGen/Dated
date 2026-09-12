using System;
using System.Collections.Generic;

public class EventDate
{
	private readonly int AMOUNT_OF_WEEKDAYS = Enum.GetNames(typeof(Weekdays)).Length;
	public Weekdays weekdaysHappening {get; private set;}
	public HashSet<int> daysHappening {get; private set;}
	
	public EventDate(Weekdays weekdaysHappening, HashSet<int> daysHappening)
	{
		this.weekdaysHappening = weekdaysHappening;
		this.daysHappening = daysHappening;
	}
	
	public bool Matches(int dayNumber)
	{
		if (daysHappening.Contains(dayNumber))
		{
			return true;
		}
		
		
		Weekdays weekdayOfDayNumber = (Weekdays)(dayNumber % AMOUNT_OF_WEEKDAYS);
		if (weekdaysHappening.HasFlag(weekdayOfDayNumber))
		{
			return true;
		}
		
		return false;
	}
	
	
	
}