using CashierModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCashier
{
    class Program
    {
        readonly static CatalogController catalogController = new CatalogController(MocksFabric.MockProducts);
        readonly static CashierController cashierController = new CashierController(MocksFabric.GetMockBills(3));

        static void Main(string[] args)
        {
            while (MainMenuInput());
        }

        static readonly Menu mainMenu = new Menu(new[] { 
            new MenuItem(ConsoleKey.F1, "Новый чек"),
            new MenuItem(ConsoleKey.Tab, "Перейти в каталог"),
            new MenuItem(ConsoleKey.Escape, "Выход"),
        });

        static bool MainMenuInput()
        {
            Console.Clear();
            mainMenu.Print();
            cashierController.PrintAllBills();
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.F1:
                    cashierController.FillBill(catalogController.Catalog);
                    break;
                case ConsoleKey.Tab:
                    while (CatalogMenuInput());
                    break;
                case ConsoleKey.Escape: return false;
            }
            return true;
        }

        static bool CatalogMenuInput()
        {
            Console.Clear();
            CatalogController.Menu.Print();
            catalogController.PrintAllProducts();
            var key = Console.ReadKey().Key;
            return catalogController.MenuAction(key);
        }
    }
}
