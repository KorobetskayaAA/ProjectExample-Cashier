using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCashier
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateBill();
        }

        static void CreateBill()
        {
            uint number = ReadBillNumber();
            DateTime date = DateTime.Now;
            var itemsBarcodes = new List<string>();
            var itemsNames = new List<string>();
            var itemsPrices = new List<decimal>();
            var itemsAmounts = new List<double>();
            var itemsCosts = new List<decimal>();
            do
            {
                Console.WriteLine("\n{0}.", itemsBarcodes.Count + 1);
                string itemBarcode = ReadBarcode();
                string itemName = ReadItemName();
                decimal itemPrice = ReadPrice();
                double itemAmount = ReadAmount();
                decimal itemCost = Math.Truncate(itemPrice * (decimal)itemAmount);
                itemsBarcodes.Add(itemBarcode);
                itemsNames.Add(itemName);
                itemsPrices.Add(itemPrice);
                itemsAmounts.Add(itemAmount);
                itemsCosts.Add(itemCost);

                Console.WriteLine(
                    "Нажмите Enter, чтобы завершить, или любую другую клавишу, чтобы продолжить."
                );
            } while (Console.ReadKey().Key != ConsoleKey.Enter);

            PrintBill(number, date, itemsBarcodes, itemsNames, 
                itemsPrices, itemsAmounts, itemsCosts);
        }

        static void PrintBill(uint number, DateTime date, List<string> itemsBarcodes,
           List<string> itemsNames, List<decimal> itemsPrices, List<double> itemsAmounts,
           List<decimal> itemsCosts)
        {
            Console.Clear();
            Console.WriteLine("       КАССОВЫЙ ЧЕК №{0:0000000000}", number);
            Console.WriteLine(date.ToLocalTime());
            Console.WriteLine("*************************************");
            for (int i = 0; i < itemsBarcodes.Count; i++)
            {
                PrintItem(itemsBarcodes[i], itemsNames[i], itemsPrices[i], itemsAmounts[i]
                    , itemsCosts[i]);
            }
            Console.WriteLine("*************************************");
            var sum = itemsCosts.Sum();
            decimal vat = 0.2m;
            Console.WriteLine("ИТОГ ={0,31:#,##0.00}", sum);
            Console.WriteLine("СУММА БЕЗ НДС ={0,22:#,##0.00}", sum / (1 + vat));
        }

        static void PrintItem(string barcode, string name, decimal price,
            double amount, decimal cost)
        {
            Console.WriteLine("{0} {1}", barcode, name);
            Console.WriteLine("{0,10:#,##0.00} * {1,8:0.000} = {2,13:#,##0.00}", price, amount, cost);
        }

        static string ReadItemName()
        {
            Console.Write("Введите наименование товара: ");
            return Console.ReadLine();
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

        static string ReadBarcode()
        {
            Console.Write("Введите штрих-код товара: ");
            string barcode;
            barcode = Console.ReadLine();
            while (barcode.Length != 13 || !ulong.TryParse(barcode, out _))
            {
                Console.Error.WriteLine("Штрих-код должен состоять из 13 цифр.");
                Console.Write("Введите штрих-код: ");
                barcode = Console.ReadLine();
            }
            return barcode;
        }

        static decimal ReadPrice()
        {
            decimal price;
            Console.Write("Введите цену: ");
            while (!decimal.TryParse(Console.ReadLine(), out price)
                || price < 0 || Math.Round(price, 2) != price)
            {
                Console.Error.WriteLine("Цена должна быть положительным числом, до двух знаков после запятой");
                Console.Write("Введите цену: ");
            }
            return price;
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
