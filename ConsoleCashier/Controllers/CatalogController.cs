﻿
using CashierModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCashier
{
    class CatalogController
    {
        public Catalog Catalog { get; }

        IList<Product> OrderedProducts => Catalog.ProductsList
            .OrderBy(product => product.Barcode.ToString())
            .ToList();

        Table<Product> table = new Table<Product>(new[] {
            new TableColumn<Product>("Штрих-код", 13,
                product => product.Barcode),
            new TableColumn<Product>("Название", 35,
                product => product.Name),
            new TableColumn<Product>("Цена", 10,
                product => string.Format("{0:#,##0.00}", product.Price)),
        });
        public Menu Menu { get; }
        public SelectFromList<Product> SelectProduct { get; }
        public Product SelectedProduct => SelectProduct.SelectedNode;

        public CatalogController(Catalog catalog)
        {
            Catalog = catalog;
            SelectProduct = new SelectFromList<Product>(() => OrderedProducts);
            Menu = new Menu(new List<MenuItem>(SelectProduct.Menu.Items) {
                    new MenuAction(ConsoleKey.F1, "Новый товар", CreateProduct),
                    new MenuAction(ConsoleKey.F2, "Редактировать", EditSelectedProduct),
                    new MenuAction(ConsoleKey.F3, "Удалить", DeleteSelectedProduct),
                    new MenuClose(ConsoleKey.Tab, "Вернуться к чекам"),
                }
            );
        }

        public CatalogController() : this(new Catalog()) { }

        public CatalogController(IEnumerable<Product> products) :
            this(new Catalog(products))
        { }

        public void CreateProduct()
        {
            Console.Clear();
            Catalog.AddProduct(new Product(
                InputValidator.ReadNewBarcode(Catalog.Barcodes),
                InputValidator.ReadProductName(),
                InputValidator.ReadPrice()
            ));
        }

        public void EditSelectedProduct()
        {
            Console.Clear();
            var product = SelectedProduct;
            Console.WriteLine("Редактирование товара {0} {1} ({2:C2})",
                product.Barcode, product.Name, product.Price);
            if (product != null)
            {
                product.Name = InputValidator.ReadProductName();
                product.Price = InputValidator.ReadPrice();
            }
        }

        public void DeleteSelectedProduct()
        {
            Console.Clear();
            var product = SelectedProduct;
            Console.WriteLine("Вы уверены, что хотете безвозвратно удалить товар {0} {1} ({2:C2})?",
                product.Barcode, product.Name, product.Price);

            Console.WriteLine("Нажмите D, чтобы подтвердить удаление, или любую другую клавишу, чтобы отменить");
            if (Console.ReadKey().Key == ConsoleKey.D)
            {
                Catalog.RemoveProduct(product);
                SelectProduct.SelectedNodeIndex--;
            }
        }

        public void PrintAllProducts()
        {
            table.Print(OrderedProducts, SelectedProduct);
        }

    }
}
