using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pallets___Boxes
{
    public static class ConsoleInterface
    {
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
            if (initialCursorPos == -1) // if initial cursor position was not set before, set it now
                SetInitialCursorPosition();
        }

        /// <summary>
        /// Clears console from the end until it reaches initial cursor position
        /// </summary>
        public static void Refresh()
        {
            for (int i = initialCursorPos; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, initialCursorPos);
        }

        /// <summary>
        /// Assigns initial cursor position for later use in Refresh()
        /// </summary>
        public static void SetInitialCursorPosition()
        {
            initialCursorPos = Console.GetCursorPosition().Top;
        }

        /// <summary>
        /// Resets cursor position (useful when you want to print change banner)
        /// </summary>
        public static void ResetInitialCursorPosition()
        {
            initialCursorPos = -1;
        }
        /// <summary>
        /// Print program menu
        /// </summary>
        public static void PrintMenu()
        {

            Console.WriteLine(_menu);
        }
        private static string _banner =
                    "       ╔═════════╗╔═══╗\n" +
                    "       ║ Pallets ║║ & ║\n" +
                    "       ╚═════════╝╚═══╝\n" +
                    "    ╔═══════════════════╗\n" +
                    "    ║       Boxes       ║\n" +
                    "    ╚═══════════════════╝\n" +
                    "   ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒\n\n";
        public static string Banner
        {
            get => _banner;
            set => _banner = value;
        }

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

        private static int initialCursorPos = -1;
    }
}
