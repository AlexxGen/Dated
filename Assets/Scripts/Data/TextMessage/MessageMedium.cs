using System;

public enum MessageMedium
{
	TEXT,
	EMAIL
}

public class MessageMediumFunctions
{
	public static MessageMedium From(String originString)
	{
		switch (originString.ToLower())
		{
			case "text":
				return MessageMedium.TEXT;
			case "email":
				return MessageMedium.EMAIL;
		}
		
		throw new ArgumentException("Invalid message mood \"" + originString + "\"");
	}
}