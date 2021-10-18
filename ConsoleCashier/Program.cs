using CashierModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCashier
{
    class Program
    {
        static void Main(string[] args)
        {
            CatalogController catalogController = new CatalogController(MocksFabric.MockProducts);
            CashierController cashierController = new CashierController();

            while (MainMenuInput(catalogController, cashierController));
        }

        static readonly Menu MainMenu = new Menu(new[] { 
            new MenuItem(ConsoleKey.F1, "Новый чек"),
            new MenuItem(ConsoleKey.Escape, "Выход"),
        });

        static bool MainMenuInput(CatalogController catalogController, CashierController cashierController)
        {
            Console.Clear();
            MainMenu.Print();
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.F1:
                    cashierController.FillBill(catalogController.Catalog);
                    break;
                case ConsoleKey.Escape: return false;
            }
            return true;
        }
    }
}
