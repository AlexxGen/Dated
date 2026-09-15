using System;

public class Message
{
	public Character Character {get; private set;}
	public MessageMedium Medium {get; private set;}
	public EventCategory Category {get; private set;}
	public int WeekNumber {get; private set;}
	public MessageMood Mood {get; private set;}
	public String Text {get; private set;}
	public String Sender {get; private set;}
	
	public Message(
		Character Character,
		MessageMedium Medium,
		EventCategory Category,
		int WeekNumber,
		MessageMood Mood,
		String Text,
		String Sender
	) {
		this.Character = Character;
		this.Medium = Medium;
		this.Category = Category;
		this.WeekNumber = WeekNumber;
		this.Mood = Mood;
		this.Text = Text;
		this.Sender = Sender;
	}
	
	
	public bool LessThan(Message other)
	{
		if (Medium < other.Medium)
		{
			return true;
		}
		else if (other.Medium < Medium)
		{
			return false;
		}
		else if (Sender.CompareTo(other.Sender) < 0)
		{
			return true;
		}
		else if (Sender.CompareTo(other.Sender) > 0)
		{
			return false;
		}
		return true;
	}
	
}