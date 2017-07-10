<Query Kind="Program" />

void Main()
{
	const string path = @"C:\Dev\AcidLabs\DB\";
	const string fileTitle = "worldindicators";
	const string fileExtension = ".sql";
	
	for (int i = 9; i <= 34; i++)
	{
		var command = $"sqlcmd -S MATRIXCODE -U admin -P admin -i \"{path}{fileTitle}{i}{fileExtension}\"";
		command.Dump($"Executing file: {fileTitle}{i}{fileExtension}...");
		
		ExecuteInConsole(command);
	}

}

void ExecuteInConsole(string cmd)
{
	var console = new Process();
	console.StartInfo.FileName = "cmd.exe";
	console.StartInfo.RedirectStandardInput = true;
	console.StartInfo.RedirectStandardOutput = true;
	console.StartInfo.CreateNoWindow = true;
	console.StartInfo.UseShellExecute = false;
	console.Start();
	console.StandardInput.WriteLine(cmd);
	console.StandardInput.Flush();
	console.StandardInput.Close();
	Console.WriteLine(console.StandardOutput.ReadToEnd());
	console.WaitForExit();
}