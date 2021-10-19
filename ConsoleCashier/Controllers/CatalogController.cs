
using CashierModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCashier
{
    class CatalogController
    {
        public Catalog Catalog { get; }

        public CatalogController()
        {
            Catalog = new Catalog();
        }

        public CatalogController(Catalog catalog)
        {
            Catalog = catalog;
        }

        public CatalogController(IEnumerable<Product> products)
        {
            Catalog = new Catalog();
            foreach (var product in products)
            {
                Catalog.AddProduct(product);
            }
        }

        public void CreateProduct()
        {
            Console.Clear();
            Catalog.AddProduct(new Product(
                InputValidator.ReadNewBarcode(Catalog.Barcodes),
                InputValidator.ReadProductName(),
                InputValidator.ReadPrice()
            ));
        }

        public void PrintAllProducts()
        {
            Console.WriteLine("{0,-13} {1,-35} {2,-10}",
                   "Штрих-код", "Название", "Цена");
            foreach (var product in Catalog.ProductsList)
            {
                Console.WriteLine("{0,13} {1,35} {2,10}",
                    product.Barcode,
                    product.Name,
                    product.Price);
            }
        }
    }
}
