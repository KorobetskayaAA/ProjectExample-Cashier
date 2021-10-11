using System;

namespace CashierModel
{
    public class Product
    {
        public string Barcode { get; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string barcode, string name, decimal price)
        {
            Barcode = barcode;
            Name = name;
            Price = price;
        }
    }
}
