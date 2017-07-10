<Query Kind="Program" />

/*
* Author: Marcelino Jorge Romero 
* Date: 10-Jul-2017
*
* This Script takes a huge .sql file that generates tables and inserts data and split's it into various smaller ones.
* Each command should terminate with ";".
*/
void Main()
{
	const int splitAtNumberOfLines = 200_000;
	
	var file = new System.IO.StreamReader(getFileName());
	
	uint fileCounter = 0;
	uint lineCounter = 0;
	System.IO.StreamWriter newFile = null;
	
	var firstLineFounded = false;
	var generateNewFile = false;
	
	string line;
	while ((line = file.ReadLine()) != null)
	{
		lineCounter++;
		var lineContainsNewTable = line.Contains("-- Table"); // or "CREATE TABLE" for a more generic script
		
		if (!firstLineFounded && !lineContainsNewTable)
		{
			continue;
		}
		firstLineFounded = true;
		
		if (lineContainsNewTable || generateNewFile)
		{
			lineCounter = 0;
			generateNewFile = false;
			
			if (newFile != null) 
			{
				newFile.Close();
				newFile.Dispose();
			}
			
			newFile = File.AppendText(getFileName(++fileCounter, lineContainsNewTable));
			newFile.WriteLine(getHeaders());
			newFile.WriteLine(line);
		}
		else
		{
			newFile.WriteLine(line);
			
			if (line.EndsWith(";") && lineCounter >= splitAtNumberOfLines)// Every <splitAtNumberOfLines> lines generates new file
			{
				generateNewFile = true;
			}
		}
	}

	newFile.Close();
	newFile.Dispose();
	file.Close();
}

string getFileName(uint? counter = null, bool isNewTable = false)
{
	const string fileTitle = "worldindicators";
	var fileName = counter == null ? $@"C:\Dev\AcidLabs\DB\{fileTitle}{(isNewTable ? "_NewTable" : string.Empty)}" : $@"C:\Dev\AcidLabs\DB\{fileTitle}{(isNewTable ? "_NewTable" : string.Empty)}{counter}";
	return fileName + ".sql";
}

string getHeaders()
{
	return @"USE WorldIndicatorsDb";
}
