using System;

namespace CashierModel
{
    public class Product
    {
        public Barcode Barcode { get; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(Barcode barcode, string name, decimal price)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("amount", "Название товара не может быть пустым");
            }
            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException("amount", "Количество товара должно быть положительным числом");
            }
            Barcode = barcode;
            Name = name;
            Price = price;
        }

        public Product(string barcode, string name, decimal price) :
            this(new Barcode(barcode), name, price) { }
    }
}
