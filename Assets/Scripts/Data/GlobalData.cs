using System;
using System.Collections.Generic;

public class GlobalData
{
	public static readonly List<CalendarEvent> events = DataParser.readEventsFromFile("Assets/Data/Events.csv");
	public static readonly List<Message> messages = DataParser.readMessagesFromFile("Assets/Data/Messages.csv");
	
	public static Message GetMessage(Character character, EventCategory category, MessageMood mood, int weekNumber)
	{
		foreach (Message message in messages)
		{
			if (
				message.Character == character &&
				message.Category == category &&
				message.Mood == mood &&
				message.WeekNumber == weekNumber
			) {
				return message;
			}
		}
		throw new ArgumentException("Error: No message found for character " + character + " category " + category + " week " + weekNumber);
	}
	
}