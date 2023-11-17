namespace ConsoleApp1
{
	public class Program
	{
		static void Main(string[] args)
		{
            Console.WriteLine("Enter Source Code: ");
			Console.WriteLine("Write exec to start the code");
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

			foreach (var item in scanner.Tokens)
			{
				Console.WriteLine(item);
			}

            if (Errors.Error_List.Count > 0)
            {
                foreach (var error in Errors.Error_List)
                {
                    Console.WriteLine(error);
                }
            }

            Console.WriteLine("Press Any Key to Exit");
			Console.ReadKey();
        }
	}
}