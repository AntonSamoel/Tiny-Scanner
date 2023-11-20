namespace ConsoleApp1
{
	public class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Write 'exec' to start generating tokens");
			Console.WriteLine("Enter Source Code: ");
			List<string> lines = new();

			string line= Console.ReadLine()!;
			while (line!="exec")
			{
				lines.Add(line);
				line = Console.ReadLine()!.ToLower();
			}
			//lines.Add("end");
            string sourceCode = "";
			foreach (var item in lines)
			{
				sourceCode += item + "\n";	
			}
			Scanner scanner = new Scanner();
			scanner.StartScanning(sourceCode);

			string output = "";
            Console.WriteLine("======== Output in Cmd ========");
            Console.WriteLine();
            foreach (var item in scanner.Tokens)
			{
				output+= item.ToString() + "\n";
				Console.WriteLine(item);
			}

            if (Errors.Error_List.Count > 0)
            {
                foreach (var error in Errors.Error_List)
                {
                    Console.WriteLine(error);
                }
            }

			// Create a file with output 
			string directory = AppDomain.CurrentDomain.BaseDirectory;
			string fileName = "output.txt";
			string filePath = Path.Combine(directory, fileName);
			File.WriteAllText(filePath,output);
			Console.WriteLine();
			Console.WriteLine("======== Output in File ========");

			Console.WriteLine($"Output File created at: {filePath}");
            Console.WriteLine();
            Console.WriteLine("Press Any Key to Exit");
			Console.ReadKey();
        }
	}
}