using CashierModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCashier
{
    static class MocksFabric
    {
        public static IEnumerable<Product> MockProducts => new List<Product>()
        {
            new Product("4634567890098", "Батон горчичный", 37.12m ),
            new Product("8001234567891", "Спагетти Италия 450г", 89.9m),
            new Product("4609876541212", "Вода негаз. 0,5л", 15.6m),
            new Product("5345738573637", "Томаты вес.", 90.5m),
            new Product("1234567891234", "Огурцы вес.", 75.25m),
        };


        public static Bill FillMockBill(Bill bill, int seed = 0)
        {
            var rnd = new Random(seed);
            var products = MockProducts;
            int itemsCount = rnd.Next(products.Count());
            for (int i = 0; i < itemsCount; i++)
            {
                bill.AddItem(
                    products.ElementAt(rnd.Next(products.Count())),
                    Math.Round(rnd.NextDouble() * 10, 3)
                );
            }
            bill.FinishEditing();
            return bill;
        }

        public static Bill GetMockBill(int seed = 0)
        {
            return FillMockBill(new Bill(), seed);
        }

        public static List<Bill> GetMockBills(int count = 1, int seed = 0)
        {
            var rnd = new Random(seed);
            var bills = new List<Bill>(count);
            for (int i = 0; i < count; i++)
            {
                bills.Add(GetMockBill(rnd.Next()));
            }
            return bills;
        }
    }
}
