using CashierModel;
using ConsoleCashier.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCashier
{
    class Program
    {
        readonly static CatalogController catalogController;
        readonly static CashierController cashierController;
        const bool useMocks = false;

        static Program()
        {
            if (useMocks)
            {
                catalogController = new CatalogController(MocksFabric.MockProducts);
                cashierController = new CashierController(catalogController.Catalog, MocksFabric.GetMockBills(3));
            }
            else
            {
                catalogController = new CatalogController(DbManager.GetProducts());
                cashierController = new CashierController(catalogController.Catalog, DbManager.GetBills());
            }
        }

        static void Main(string[] args)
        {
            while (MainMenuInput());
            if (!useMocks)
            {
                DbManager.UpdateProducts(catalogController.Catalog.ProductsList);
                DbManager.UpdateBills(cashierController.Cashier.Bills);
            }
        }

        static readonly Menu mainMenu = new Menu(new MenuItem[] {
            new MenuAction(ConsoleKey.Tab, "Перейти в каталог",
                () => { while (CatalogMenuInput()); }),
            new MenuClose(ConsoleKey.Escape, "Выход"),
        });

        static bool MainMenuInput()
        {
            Console.Clear();
            cashierController.Menu.Print();
            mainMenu.Print();
            cashierController.PrintAllBills();
            var key = Console.ReadKey().Key;
            return mainMenu.Action(key) ? cashierController.Menu.Action(key) : false;
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
