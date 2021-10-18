using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCashier
{
    class Menu
    {
        IEnumerable<MenuItem> Items { get; }

        public Menu(IEnumerable<MenuItem> items)
        {
            Items = items;
        }

        public void Print()
        {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            foreach (var item in Items)
            {
                item.Print();
                Console.Write("  ");
            }
            Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
            Console.ResetColor();
        }
    }
}
