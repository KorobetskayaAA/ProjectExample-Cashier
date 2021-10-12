using System;
using System.Collections.Generic;
using System.Text;

namespace CashierModel
{
    public class Catalog
    {
        Dictionary<string, Product> Products { get; } = new Dictionary<string, Product>();
        public Product this[string barcode]
        {
            get => Products[barcode];
        }

        public void AddProduct(Product product)
        {
            Products.Add(product.Barcode, product);
        }

        public void RemoveProduct(string barcode)
        {
            Products.Remove(barcode);
        }

        public void RemoveProduct(Product product)
        {
            RemoveProduct(product.Barcode);
        }

        public IEnumerable<string> Barcodes => Products.Keys;
    }
}
