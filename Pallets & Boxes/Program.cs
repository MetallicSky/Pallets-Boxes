using Pallets___Boxes;
using System.CodeDom.Compiler;
using System.Collections.Generic;

class Program
{
    public static void SortPalletsBy<T>(Func<Pallet, T> keySelector, List<Pallet> pallets, bool descending = false) where T : IComparable<T>
    {
        pallets.Sort((pallet1, pallet2) =>
        {
            int result = keySelector(pallet1).CompareTo(keySelector(pallet2));
            return descending ? -result : result;
        });
    }
    public static SortedDictionary<DateOnly, List<Pallet>> GeneratePalletDictionary(out List<Pallet> PalletList, in int num = 1000)
    {
        if (num < 0)
        {
            throw new ArgumentException("number of generated pallets can't be negative", nameof(num));
        }
        Random rnd = new();
        PalletList = [];

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
        SortPalletsBy(pallet => pallet.TotalWeight, PalletList);
        SortedDictionary<DateOnly, List<Pallet>> PalletGroups = [];
        for (int i = 0; i < PalletList.Count; i++)
        {
            DateOnly date = PalletList[i].ExpDate.GetValueOrDefault();
            if (!PalletGroups.ContainsKey(date))
            {
                // If not, initialize the key with an empty list
                PalletGroups[date] = new List<Pallet>();
            }
            PalletGroups[date].Add(PalletList[i]);
        }
        return PalletGroups;
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
            SortedDictionary<DateOnly, List<Pallet>>? palletGroups = [];
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
                        palletGroups = GeneratePalletDictionary(out pallets);
                        Console.WriteLine("Data has been generated ");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case 2:
                        // TODO: implement json data input
                        break;
                    case 3:
                        // TODO: implement db data input
                        break;
                    case 4:
                        PrintPallets(palletGroups); // in current implementation, all data is sorted and grouped right after generation
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case 5:
                        if (pallets.Count < 3)
                        {
                            Console.WriteLine("There was less than 3 pallets generated!");
                            continue;
                        }
                        SortPalletsBy(pallet => pallet.ExpDate.GetValueOrDefault(), pallets, true);
                        List<Pallet> top3Pallets = [];
                        top3Pallets.Add(pallets[0]);
                        top3Pallets.Add(pallets[1]);
                        top3Pallets.Add(pallets[2]);
                        pallets[0].SortBoxesBy(box => box.Weight);
                        pallets[1].SortBoxesBy(box => box.Weight);
                        pallets[2].SortBoxesBy(box => box.Weight);
                        PrintPallets(top3Pallets);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Unknown option number entered: " + input[0] + "\n\n");
                        continue;
                }
                ConsoleInterface.Refresh();
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
