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
            Console.WriteLine(
                "Нажмите любую клавишу, чтобы продолжить..."
            );
            Console.ReadKey();
        }

        public void PrintAllBills()
        {
            Console.WriteLine("{0,-10} {1,-20} {2,-7} {3,-12} {4,-8}",
                   "№", "Дата создания", "Кол-во", "Сумма", "Статус");
            foreach (var bill in Cashier.Bills)
            {
                Console.WriteLine("{0,10:0000000000} {1,20} {2,7} {3,12:#,##0.00} {4,8}",
                    bill.Number, bill.Created, 
                    bill.ItemsCount, bill.Sum, 
                    BillStatusRus.Names[bill.Status]);
            }
        }
    }
}
