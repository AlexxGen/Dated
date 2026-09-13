using System.Collections.Generic;

public class GlobalData
{
	public static readonly List<CalendarEvent> events = DataParser.readEventsFromFile("Data/Events.csv");
	
}