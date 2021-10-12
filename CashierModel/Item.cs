using System;
using System.Collections.Generic;
using System.Text;

namespace CashierModel
{
    public class Item
    {
        public string Barcode { get; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Amount { get; set; }
        public decimal Cost => Math.Truncate(Price * (decimal)Amount * 100m) / 100m;

        public Item(Product product, double amount)
        {
            Barcode = product.Barcode;
            Name = product.Name;
            Price = product.Price;
            Amount = amount;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}\n{2,11:#,##0.00} * {3,7:0.###} = {4,13:#,##0.00}",
                Barcode, Name,
                Price, Amount, Cost);
        }
    }
}
