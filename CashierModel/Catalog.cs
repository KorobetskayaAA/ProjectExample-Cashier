using System;
using System.Collections.Generic;
using System.Text;

namespace CashierModel
{
    public class Catalog
    {
        List<Product> Products { get; } = new List<Product>();
        public Product this[string barcode]
        {
            get => Products.Find(product => product.Barcode == barcode);
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }

        public void RemoveProduct(Product product)
        {
            Products.Remove(product);
        }

        public void RemoveProduct(string barcode)
        {
            RemoveProduct(Products.Find(product => product.Barcode == barcode));
        }
    }
}
