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
            string barcode;
            barcode = Console.ReadLine();
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
    }
}
