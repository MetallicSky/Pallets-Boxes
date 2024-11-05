using Pallets___Boxes;

class Program
{


    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            ConsoleInterface.PrintLogo();
            ConsoleInterface.PrintMenu();
            while (true)
            {
                string line = Console.ReadLine();
                string[] input = line.Split(' ');
                bool isCorrect = true;
                isCorrect = int.TryParse(input[0], out int mode);
                ConsoleInterface.Refresh();
                ConsoleInterface.PrintMenu();
                if (isCorrect == false)
                    mode = -1;
                switch (mode)
                {
                    case 0:
                        return;
                    case 1:
                        // TODO: implement generate random entries
                        break;
                    case 2:
                        // TODO: implement json data input
                        break;
                    case 3:
                        // TODO: implement db data input
                        break;
                    case 4:
                        // TODO: implement all data group, sort and print
                        break;
                    case 5:
                        // TODO: implement top 3 pallets print
                        break;
                    default:
                        Console.WriteLine("Unknown option number entered: " + input[0] + "\n\n");
                        break;
                }
            }
            return;
        }
        string fileName = args[0];
        if (Path.HasExtension(fileName))
        {
            Console.WriteLine("ERROR: specified file name doesn't contain have any extension");
            return;
        }
        string fileExtension = Path.GetExtension(fileName);
        if (fileExtension == ".json") // check if specified file is json
        {
            Console.WriteLine("JSON file detected, attempting to read data");
            // TODO: implement json data input
            return;
        }
        if (fileExtension == ".db") // check if specified file is db
        {
            Console.WriteLine("db file detected, attempting to read data");
            // TODO: implement db data input
            return;
        }

    }
}
