namespace ConsoleApp1
{
	internal class Program
	{
		static void Main(string[] args)
		{
            Console.WriteLine("Enter Source Code: ");
			
			List<string> lines = new List<string>();

			string line= Console.ReadLine()!;
			while (line!="end")
			{
				lines.Add(line);
				line = Console.ReadLine()!.ToLower();
			}
			lines.Add("end");
            string sourceCode = "";
			foreach (var item in lines)
			{
				sourceCode += item + "\n";	
			}
			Scanner scanner = new Scanner();
			scanner.StartScanning(sourceCode);

			foreach (var item in scanner.Tokens)
			{
				Console.WriteLine(item);
			}

			Console.WriteLine("Press Any Key to Exit");
			Console.ReadKey();
        }
	}
}