using CashierModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCashier
{
    class CashierController
    {
        public Cashier Cashier { get; } = new Cashier();


        public void FillBill(Catalog catalog)
        {
            Bill bill = Cashier.OpenNewBill();
            Console.Clear();
            Console.WriteLine("Заполнение чека №{0:0000000000}", bill.Number);
            do
            {
                Console.WriteLine("\n{0}.", bill.ItemsCount + 1);
                string barcode = InputValidator.ReadBarcode(catalog.Barcodes);
                Product product = catalog[barcode];
                Console.WriteLine("{0} - {1:C2}", product.Name, product.Price);
                double amount = InputValidator.ReadAmount();
                bill.AddItem(product, amount);
                Console.WriteLine(
                    "Нажмите Enter, чтобы завершить, или любую другую клавишу, чтобы продолжить."
                );
            } while (Console.ReadKey().Key != ConsoleKey.Enter);
            bill.FinishEditing();
            PrintBill(bill);
        }

        public void PrintBill(Bill bill)
        {
            Console.Clear();
            Console.WriteLine(bill);
        }
    }
}
