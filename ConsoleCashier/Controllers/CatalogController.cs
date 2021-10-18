
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
    }
}
