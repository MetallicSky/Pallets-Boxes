using Pallets___Boxes;

class Program
{
    public static List<Pallet> GeneratePalletDictionary( in int num = 1000)
    {
        if (num < 0)
        {
            throw new ArgumentException("number of generated pallets can't be negative", nameof(num));
        }
        Random rnd = new();
        List<Pallet> PalletList = [];

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

            pallet.SortBoxesBy(box => box.Weight);
            PalletList.Add(pallet);

        }
        return PalletList;
    }

    public static void SortPalletsBy<T>(Func<Pallet, T> keySelector, List<Pallet> pallets, bool descending = false) where T : IComparable<T>
    {
        pallets.Sort((pallet1, pallet2) =>
        {
            int result = keySelector(pallet1).CompareTo(keySelector(pallet2));
            return descending ? -result : result;
        });
    }

    public static SortedDictionary<DateOnly, List<Pallet>> GroupSort(List<Pallet> input)
    {
        SortedDictionary<DateOnly, List<Pallet>> output = [];

        SortPalletsBy(pallet => pallet.TotalWeight, input);
        for (int i = 0; i < input.Count; i++)
        {
            DateOnly date = input[i].ExpDate.GetValueOrDefault();
            if (!output.TryGetValue(date, out List<Pallet>? value))
            {
                value = [];
                output[date] = value;
            }

            value.Add(input[i]);
        }
        return output;
    }

    public static List<Pallet> GetFreshestPallets(in List<Pallet> input, in int amount = 3)
    {
        List<Pallet> output = [];
        if (input.Count < amount)
        {
            return output;
        }
        SortPalletsBy(pallet => pallet.ExpDate.GetValueOrDefault(), input, true);

        for (int i = 0; i < amount; i++)
        {
            output.Add(input[i]);
            output[i].SortBoxesBy(box => box.Weight);
        }
        return output;
    }

    public static void PrintPallets(in SortedDictionary<DateOnly, List<Pallet>> PalletGroups)
    {
        foreach (var group in PalletGroups)
        {
            Console.WriteLine("============================");
            Console.WriteLine("Expiration Date: " + group.Key);
            Console.WriteLine("============================");
            PrintPallets(group.Value);
        }
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
            List<Pallet> pallets = [];
            ConsoleInterface.Refresh();
            while (true)
            {
                string line = Console.ReadLine();
                string[] input = line.Split(' ');
                bool isCorrect;
                isCorrect = int.TryParse(input[0], out int mode);
                ConsoleInterface.Refresh();
                if (isCorrect == false)
                    mode = -1;
                switch (mode)
                {
                    case 0:
                        return;
                    case 1:
                        pallets = GeneratePalletDictionary();
                        Console.WriteLine("Data has been generated");
                        ConsoleInterface.PressAnyKey();
                        break;
                    case 2:
                        // TODO: implement json data input
                        break;
                    case 3:
                        // TODO: implement db data input
                        break;
                    case 4:
                        if (pallets.Count == 0)
                        {
                            Console.WriteLine("No data has been generated so far!");
                            continue;
                        }
                        SortedDictionary<DateOnly, List<Pallet>> palletGroups = GroupSort(pallets);
                        PrintPallets(palletGroups);
                        ConsoleInterface.PressAnyKey();
                        break;
                    case 5:
                        int amount = 3;
                        List<Pallet> freshest = GetFreshestPallets(pallets, amount);
                        if (freshest.Count == 0)
                        {
                            Console.WriteLine("There was less than " + amount + " pallets generated!");
                            continue;
                        }
                        PrintPallets(freshest);
                        ConsoleInterface.PressAnyKey();
                        break;
                    default:
                        Console.WriteLine("Unknown option number entered: " + input[0] + "\n\n");
                        continue;
                }
                ConsoleInterface.Refresh();
            }
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
