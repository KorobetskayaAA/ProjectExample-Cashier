
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

        int selectedProductIndex = 0;
        int SelectedProductIndex 
        { 
            get => selectedProductIndex; 
            set
            {
                if (value < 0)
                {
                    selectedProductIndex = 0;
                }
                else if (value >= Catalog.ProductsCount)
                {
                    selectedProductIndex = Catalog.ProductsCount - 1;
                }
                else
                {
                    selectedProductIndex = value;
                }
            }
        }
        Product SelectedProduct => Catalog.ProductsCount > 0 
            ? OrderedProducts[SelectedProductIndex] 
            : null;
        Table<Product> table = new Table<Product>(new [] {
            new TableColumn<Product>("Штрих-код", 13,
                product => product.Barcode),
            new TableColumn<Product>("Название", 35,
                product => product.Name),
            new TableColumn<Product>("Цена", 10,
                product => string.Format("{0:#,##0.00}", product.Price)),
        });

        public CatalogController(Catalog catalog)
        {
            Catalog = catalog;
        }

        public CatalogController() : this(new Catalog()) {}

        public CatalogController(IEnumerable<Product> products) :
            this(new Catalog(products)) { }

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
                SelectedProductIndex--;
            }
        }

        public void PrintAllProducts()
        {
            table.Print(OrderedProducts, SelectedProduct);
        }

        public static Menu Menu { get; } = new Menu(new[] {
            new MenuItem(ConsoleKey.F1, "Новый товар"),
            new MenuItem(ConsoleKey.F2, "Редактировать"),
            new MenuItem(ConsoleKey.F3, "Удалить"),
            new MenuItem(ConsoleKey.Tab, "Вернуться к чекам"),
        });


        public bool MenuAction(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.F1:
                    CreateProduct();
                    return true;
                case ConsoleKey.F2:
                    EditSelectedProduct();
                    return true;
                case ConsoleKey.F3:
                    DeleteSelectedProduct();
                    return true;
                case ConsoleKey.UpArrow:
                    SelectedProductIndex--;
                    return true;
                case ConsoleKey.DownArrow:
                    SelectedProductIndex++;
                    return true;
                case ConsoleKey.Tab: return false;
            }
            return true;
        }
    }
}
