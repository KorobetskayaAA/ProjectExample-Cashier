using CashierModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCashier
{
    static class InputValidator
    {
        public static Barcode ReadBarcode(IEnumerable<string> knownBarcodes)
        {
            Console.Write("Введите штрих-код товара: ");
            string barcode = Console.ReadLine();
            while (!Barcode.IsCorrect(barcode) || !knownBarcodes.Contains(barcode))
            {
                Console.Error.WriteLine("Штрих-код должен состоять из 13 цифр. Указанный штрих-код не найден.");
                Console.Write("Введите штрих-код: ");
                barcode = Console.ReadLine();
            }
            return barcode;
        }

        public static double ReadAmount()
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

        public static Barcode ReadNewBarcode(IEnumerable<string> knownBarcodes)
        {
            Console.Write("Введите штрих-код товара: ");
            string barcode = Console.ReadLine();
            while (true)
            {
                if (!Barcode.IsCorrect(barcode))
                { 
                    Console.Error.WriteLine("Штрих-код должен состоять из 13 цифр.");
                }
                else if (knownBarcodes.Contains(barcode))
                {
                    Console.Error.WriteLine("Товар с таким штрих-кодом уже существует.");
                }
                else
                {
                    break;
                }
                Console.Write("Введите штрих-код: ");
                barcode = Console.ReadLine();
            }
            return barcode;
        }

        public static string ReadProductName()
        {
            Console.Write("Введите наименование товара: ");
            return Console.ReadLine();
        }

        public static decimal ReadPrice()
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

        public static uint ReadBillNuber()
        {
            Console.Write("Введите номер чека: ");
            string input = Console.ReadLine();
            uint number;
            while (!uint.TryParse(input, out number))
            {
                Console.Error.WriteLine("Номер чека должен быть положительным целым числом.");
                Console.Write("Введите номер чека: ");
                input = Console.ReadLine();
            }
            return number;
        }
    }
}
