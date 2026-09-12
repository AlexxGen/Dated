public class TimeOfDay
{
	public int Hour {get; private set;}
	public int Minute {get; private set;}
	
	public TimeOfDay(int Minute, int Hour)
	{
		this.Minute = Minute;
		this.Hour = Hour;
	}
}