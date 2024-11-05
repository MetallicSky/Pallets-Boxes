// See https://aka.ms/new-console-template for more information

class Program
{
    static void Main(string[] args)
    {
        const string logo =
        "       ╔═════════╗╔═══╗\n" +
        "       ║ Pallets ║║ & ║\n" +
        "       ╚═════════╝╚═══╝\n" +
        "    ╔═══════════════════╗\n" +
        "    ║       Boxes       ║\n" +
        "    ╚═══════════════════╝\n" +
        "   ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n\n";
        Console.WriteLine(logo);
        Console.WriteLine("Number of arguments: " + args.Length);
    }
}
