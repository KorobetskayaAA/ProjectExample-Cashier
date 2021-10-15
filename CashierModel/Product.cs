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
            Barcode = barcode;
            Name = name;
            Price = price;
        }

        public Product(string barcode, string name, decimal price) :
            this(new Barcode(barcode), name, price) { }
    }
}
