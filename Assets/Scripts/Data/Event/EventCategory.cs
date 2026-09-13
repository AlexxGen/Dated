using System;

public enum EventCategory
{
	ACADEMIC,
	EXTRACURRICULAR,
	SOCIAL
}

public class EventCategoryFunctions
{
	public static EventCategory From(String originString)
	{
		switch (originString.ToLower())
		{
			case "academic":
				return EventCategory.ACADEMIC;
			case "extracurricular":
				return EventCategory.EXTRACURRICULAR;
			case "social":
				return EventCategory.SOCIAL;
		}
		throw new ArgumentException("Invalid character name \"" + originString + "\"");
	}
}