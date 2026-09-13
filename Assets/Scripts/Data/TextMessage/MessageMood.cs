using System;

public enum MessageMood
{
	POSITIVE,
	NEGATIVE
}

public class MessageMoodFunctions
{
	public static MessageMood From(String originString)
	{
		switch (originString.ToLower())
		{
			case "positive":
				return MessageMood.POSITIVE;
			case "negative":
				return MessageMood.NEGATIVE;
		}
		
		throw new ArgumentException("Invalid message mood \"" + originString + "\"");
	}
}