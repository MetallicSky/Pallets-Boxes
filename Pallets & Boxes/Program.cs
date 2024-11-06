using Pallets___Boxes;
using System.CodeDom.Compiler;

class Program
{
    public static List<Pallet> GeneratePalletList(in int num = 1000)
    {
        if (num < 0)
        {
            throw new ArgumentException("number of generated pallets can't be negative", nameof(num));
        }
        Random rnd = new();
        var pallets = new List<Pallet>();

        for (int i = 0; i < num; i++)
        {
            
            double width = rnd.NextDouble() * (1.0 - 0.5) + 0.5;
            double length = rnd.NextDouble() * (1.0 - 0.5) + 0.5;
            double height = rnd.NextDouble() * (0.1 - 0.05) + 0.05;
            double weight = rnd.Next(15, 31);
            Pallet pallet = new Pallet(height, width, length, weight);
            int boxAmount = rnd.Next(0, 6);
            for (int j = 0; j < boxAmount; j++)
            {
                new Box(pallet);
            }
            pallets.Add(pallet);

        }
        return pallets;
    }

    public static void PrintPallets(in List<Pallet> pallets)
    {
        Console.WriteLine("Total pallets: " + pallets.Count);
        for (int i = 0; i < pallets.Count; i++)
        {
            pallets[i].PrintPallet();
        }
    }

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
                bool isCorrect;
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
                        PrintPallets(GeneratePalletList());
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
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
