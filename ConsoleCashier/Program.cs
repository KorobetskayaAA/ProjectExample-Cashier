using System;

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
            string number = ReadBillNumber();
            DateTime date = DateTime.Now;
            int itemsCount = 0;
            decimal sum = 0;
            do
            {
                itemsCount++;
                Console.WriteLine();
                Console.WriteLine(itemsCount + ".");
                sum += ReadItem();
                Console.WriteLine(
                    "Нажмите ESC, чтобы завершить, или любую другую клавишу, чтобы продолжить."
                );
            } while (Console.ReadKey().Key != ConsoleKey.Escape);

            Console.WriteLine("***********************************");
            PrintBill(number, date, itemsCount, sum);
        }

        static decimal ReadItem()
        {
            string itemBarcode = ReadBarcode();
            string itemName = ReadItemName();
            decimal itemPrice = ReadPrice();
            double itemAmount = ReadAmount();
            decimal itemCost = Math.Truncate(itemPrice * (decimal)itemAmount);
            Console.WriteLine("===");
            PrintItem(itemBarcode, itemName, itemPrice, itemAmount, itemCost);
            return itemCost;
        }

        static void PrintBill(string number, DateTime date, int itemsCount, decimal sum)
        {
            Console.WriteLine("     ЧЕК №" + number);
            Console.WriteLine(date.ToLocalTime());
            Console.WriteLine("Количество позиций: " + itemsCount);
            Console.WriteLine("Сумма позиций: " + sum);
        }

        static void PrintItem(string barcode, string name, decimal price, 
            double amount, decimal cost)
        {
            Console.WriteLine(barcode + " " + name);
            Console.WriteLine("  " + price + " * " + amount + " = " + cost);
        }

        static string ReadItemName()
        {
            Console.Write("Введите наименование товара: ");
            return Console.ReadLine();
        }

        static string ReadBillNumber()
        {
            Console.Write("Введите номер чека: ");
            string number;
            number = Console.ReadLine();
            while (!ulong.TryParse(number, out _))
            {
                Console.Error.WriteLine("Номер чека должен состоять из цифр.");
                Console.Write("Введите номер чека: ");
                number = Console.ReadLine();
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
