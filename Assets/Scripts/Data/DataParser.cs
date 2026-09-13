
using System;
using UniCSV;
using System.Collections.Generic;
using UnityEditor;

public class DataParser
{
	public static readonly List<CalendarEvent> events = readEventsFromFile("Data/Events.csv");
	
	
	public static TimeSpan timespanFromString(String originString)
	{
		float totalTimeInHours = float.Parse(originString);
		
		int hoursToReturn = (int)totalTimeInHours;
		float totalRemainingMinutes = (totalTimeInHours - hoursToReturn) * 60;
		int minutesToReturn = (int)totalRemainingMinutes;
		float totalRemainingSeconds = (totalRemainingMinutes - minutesToReturn) * 60;
		int secondsToReturn = (int)totalRemainingSeconds;
		
		
		return new TimeSpan(hoursToReturn, minutesToReturn, secondsToReturn);
		
	}
	public static List<CalendarEvent> readEventsFromFile(String fileName)
	{
		List<CalendarEvent> toReturn = new List<CalendarEvent>();
		
		
		List<List<String>> data = CsvParser.ParseFromPath(fileName, true);
		
		foreach (List<String> line in data)
		{
			Character eventCharacter = CharacterFunctions.From(line[0]);
			String eventName = line[1];
			EventCategory eventCategory = EventCategoryFunctions.From(line[2]);
			int eventWeek = int.Parse(line[3]);
			Weekdays daysOfEvent = WeekdaysFunctions.From(line[4]);
			TimeSpan eventLength = timespanFromString(line[5]);
			String eventDialogue = line[6];
			
			CalendarEvent toAdd = new CalendarEvent(eventCharacter, eventName, eventCategory, eventWeek, daysOfEvent, eventLength, eventDialogue);
			
			toReturn.Add(toAdd);
		}
		
		
		return toReturn;
	}
	
	/*
	private static DataTable GetDataTableFromCSVFile(string csv_file_path)
	{
	    DataTable csvData = new DataTable();

	    try
	    {
	        using(var csvReader = new Microsoft.VisualBasic.FileIO.TextFieldParser(csv_file_path))
	        {
	            csvReader.SetDelimiters(new string[] { "," });
	            csvReader.HasFieldsEnclosedInQuotes = true;
	            string[] colFields = csvReader.ReadFields();

	            foreach (string column in colFields)
	            {
	                DataColumn datacolumn = new DataColumn(column);
	                datacolumn.AllowDBNull = true;
	                csvData.Columns.Add(datacolumn);
	            }

	            while (!csvReader.EndOfData)
	            {
	                string[] fieldData = csvReader.ReadFields();
	                //Making empty value as null
	                for (int i = 0; i < fieldData.Length; i++)
	                {
	                    if (fieldData[i] == "")
	                    {
	                        fieldData[i] = null;
	                    }
	                }

	                csvData.Rows.Add(fieldData);
	             }
	         }
	     }
	     catch (Exception ex)
	     {
	     }

	     return csvData;

	}
	
	*/
}
