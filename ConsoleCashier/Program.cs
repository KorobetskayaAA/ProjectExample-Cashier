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

        static readonly Menu mainMenu = new Menu(new MenuItem[] { 
            new MenuAction(ConsoleKey.F1, "Новый чек",
                () => cashierController.FillBill(catalogController.Catalog)),
            new MenuAction(ConsoleKey.Tab, "Перейти в каталог",
                () => { while (CatalogMenuInput()); }),
            new MenuClose(ConsoleKey.Escape, "Выход"),
        });

        static bool MainMenuInput()
        {
            Console.Clear();
            mainMenu.Print();
            cashierController.PrintAllBills();
            return mainMenu.Action(Console.ReadKey().Key);
        }

        static bool CatalogMenuInput()
        {
            Console.Clear();
            catalogController.Menu.Print();
            catalogController.PrintAllProducts();
            return catalogController.Menu.Action(Console.ReadKey().Key);
        }
    }
}
