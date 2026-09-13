
using System;

[Flags]
public enum Weekdays
{
	NONE = 0,
	MONDAY,
	TUESDAY,
	WEDNESDAY,
	THURSDAY,
	FRIDAY
	
}

public class WeekdaysFunctions
{
	public static Weekdays From(String inputString)
	{
		Weekdays toReturn = Weekdays.NONE;
		foreach (char c in inputString) {
			switch (c)
			{
				case 'M':
					toReturn |= Weekdays.MONDAY;
					break;
				case 'T':
					toReturn |= Weekdays.TUESDAY;
					break;
				case 'W':
					toReturn |= Weekdays.WEDNESDAY;
					break;
				case 'R':
					toReturn |= Weekdays.THURSDAY;
					break;
				case 'F':
					toReturn |= Weekdays.FRIDAY;
					break;
			}
		}
		
		return toReturn;
		
	}
}