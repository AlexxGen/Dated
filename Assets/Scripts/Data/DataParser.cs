
using System;
using UniCSV;
using System.Collections.Generic;


public class DataParser
{
	// public static readonly List<CalendarEvent> events = readEventsFromFile("Data/Events.csv");
	
	// public static List<CalendarEvent> readEventsFromFile(String fileName)
	// {
	// 	List<CalendarEvent> toReturn = new List<CalendarEvent>();
		
		
	// 	List<List<String>> data = CsvParser.ParseFromPath(fileName, true);
		
	// 	foreach (List<String> line in data)
	// 	{
	// 		Character eventCharacter = CharacterFunctions.From(line[0]);
	// 		String eventName = line[1];
			
	// 		/*
	// 		- Category
	// 		- Week number
	// 		- Days
	// 		- Start
	// 		- End
	// 		- Dialogue
			
			
	// 		*/
			
			
	// 		CalendarEvent toAdd = new CalendarEvent();
			
	// 		toReturn.Add(toAdd);
	// 	}
		
		
	// 	return toReturn;
	// 	// GetDataTableFromCSVFile("table.csv");
	// }
	
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
