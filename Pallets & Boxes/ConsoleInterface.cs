using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pallets___Boxes
{
    public static class ConsoleInterface
    {
        private static string _banner =
                    "       ╔═════════╗╔═══╗\n" +
                    "       ║ Pallets ║║ & ║\n" +
                    "       ╚═════════╝╚═══╝\n" +
                    "    ╔═══════════════════╗\n" +
                    "    ║       Boxes       ║\n" +
                    "    ╚═══════════════════╝\n" +
                    "   ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n\n";
        private static string _menu =
                    "Choose one of the following options (enter option number):\n" +
                    "  1 - Generate data with 1000 random entries\n" +
                    "  2 - Read data from specified .json file\n" +
                    "  3 - Read data from specified .db file\n" +
                    "\n" +
                    "  4 - Group and sort all pallets by expiration date and all sort all associated boxes by weight\n" +
                    "  5 - Get 3 pallets and its content with longest expiration date, sorted by volume\n" +
                    "\n" +
                    "  0 - Exit program\n\n";
        public static string Menu
        {
            get => _menu;
            set => _menu = value;
        }
        public static string Banner
        {
            get => _banner;
            set => _banner = value;
        }
        /// <summary>
        /// Prints banner in console. If cursor position was not initialized before (==1) then assigns it after banner print
        /// </summary>
        /// <param name="banner">String representing banner. If not specified, will print default banner</param>
        public static void PrintLogo(string banner = "")
        {
            if (banner == "") // default banner if nothing or empty string was received
            {
                banner = _banner;
            }

            Console.WriteLine(banner);
        }
        /// <summary>
        /// Print program menu
        /// </summary>
        public static void PrintMenu()
        {
            Console.WriteLine(_menu);
        }
        /// <summary>
        /// Clears console and prints banner with menu
        /// </summary>
        public static void Refresh()
        {
            Console.Clear();
            PrintLogo();
            PrintMenu();
        }
        /// <summary>
        /// Pauses Program and prompts user to press any key to continue
        /// </summary>
        /// <param name="message">String representing prompt message. If not specified, will print default text</param>
        public static void PressAnyKey(string message = "")
        {
            if (message == "")
            {
                message = "Press any key to continue...";
            }
            Console.WriteLine(message);
            Console.ReadKey();
        }
    }
}
