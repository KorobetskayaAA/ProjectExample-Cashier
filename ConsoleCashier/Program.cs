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
            Catalog catalog = new Catalog();
            foreach(var product in MockProducts)
            {
                catalog.AddProduct(product);
            }

            Cashier cashier = new Cashier();
            Bill bill = cashier.OpenNewBill();
            FillBill(catalog, bill);

            PrintBill(bill);
        }

        static IEnumerable<Product> MockProducts => new List<Product>()
        {
            new Product("4634567890098", "Батон горчичный", 37.12m ),
            new Product("8001234567891", "Спагетти Италия 450г", 89.9m),
            new Product("4609876541212", "Вода негаз. 0,5л", 15.6m),
            new Product("5345738573637", "Томаты вес.", 90.5m),
            new Product("1234567891234", "Огурцы вес.", 75.25m),
         };

        static void FillBill(Catalog catalog, Bill bill)
        {
            do
            {
                Console.WriteLine("\n{0}.", bill.ItemsCount + 1);
                string barcode = ReadBarcode(catalog.Barcodes);
                Product product = catalog[barcode];
                Console.WriteLine("{0} - {1:C2}", product.Name, product.Price);
                double amount = ReadAmount();
                bill.AddItem(product, amount);
                Console.WriteLine(
                    "Нажмите Enter, чтобы завершить, или любую другую клавишу, чтобы продолжить."
                );
            } while (Console.ReadKey().Key != ConsoleKey.Enter);
        }

        static void PrintBill(Bill bill)
        {
            Console.Clear();
            Console.WriteLine("       КАССОВЫЙ ЧЕК №{0:0000000000}", bill.Number);
            Console.WriteLine(bill.Created.ToLocalTime());
            Console.WriteLine("*************************************");
            for (int i = 0; i < bill.ItemsCount; i++)
            {
                PrintItem(bill[i]);
            }
            Console.WriteLine("*************************************");
            var sum = bill.Sum;
            decimal vat = 0.2m;
            Console.WriteLine("ИТОГ ={0,31:#,##0.00}", sum);
            Console.WriteLine("СУММА БЕЗ НДС ={0,22:#,##0.00}", sum / (1 + vat));
        }

        static void PrintItem(Item item)
        {
            Console.WriteLine("{0} {1}", item.Barcode, item.Name);
            Console.WriteLine("{0,11:#,##0.00} * {1,7:0.###} = {2,13:#,##0.00}",
                item.Price, item.Amount, item.Cost);
        }

        static uint ReadBillNumber()
        {
            Console.Write("Введите номер чека: ");
            uint number;
            string input = Console.ReadLine();
            while (!uint.TryParse(input, out number))
            {
                Console.Error.WriteLine("Номер чека должен состоять из цифр.");
                Console.Write("Введите номер чека: ");
                input = Console.ReadLine();
            }
            return number;
        }

        static string ReadBarcode(IEnumerable<string> knownBarcodes)
        {
            Console.Write("Введите штрих-код товара: ");
            string barcode;
            barcode = Console.ReadLine();
            while (barcode.Length != 13 || !ulong.TryParse(barcode, out _)
                || !knownBarcodes.Contains(barcode))
            {
                Console.Error.WriteLine("Штрих-код должен состоять из 13 цифр. Указанный штрих-код не найден.");
                Console.Write("Введите штрих-код: ");
                barcode = Console.ReadLine();
            }
            return barcode;
        }

        static double ReadAmount()
        {
            double amount;
            Console.Write("Введите количество: ");
            while (!double.TryParse(Console.ReadLine(), out amount)
                || amount < 0)
            {
                Console.Error.WriteLine("Количество должно быть положительным числом.");
                Console.Write("Введите количество: ");
            }
            return amount;
        }
    }
}
