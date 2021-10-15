using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CashierModel
{
    public class Catalog
    {
        Dictionary<Barcode, Product> Products { get; } = new Dictionary<Barcode, Product>();
        public Product this[Barcode barcode]
        {
            get => Products[barcode];
        }

        public void AddProduct(Product product)
        {
            Products.Add(product.Barcode, product);
        }

        public void RemoveProduct(Barcode barcode)
        {
            Products.Remove(barcode);
        }

        public void RemoveProduct(string barcode)
        {
            RemoveProduct(new Barcode(barcode));
        }

        public void RemoveProduct(Product product)
        {
            RemoveProduct(product.Barcode);
        }

        public IEnumerable<string> Barcodes => Products.Keys.Select(key => key.ToString());
    }
}
