
using System;
using System.Data;


public class DataParser
{
	
	void thing()
	{
		
		// GetDataTableFromCSVFile("table.csv");
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
