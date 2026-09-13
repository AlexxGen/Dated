using System.Collections.Generic;

public class GlobalData
{
	public static readonly List<CalendarEvent> events = DataParser.readEventsFromFile("Data/Events.csv");
	public static readonly List<Message> messages = DataParser.readMessagesFromFile("Data/Messages.csv");
}