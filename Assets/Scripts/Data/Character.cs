using System;


public enum Character
{
	JESSIE,
	PETER
}


public class CharacterFunctions
{
	public static Character From(String originString)
	{
		switch (originString.ToLower())
		{
			case "jessie":
				return Character.JESSIE;
			case "peter":
				return Character.PETER;
		}
		
		throw new ArgumentException("Error: Invalid character name \"" + originString + "\"");
	}
}