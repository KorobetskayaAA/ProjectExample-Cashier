
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

        bool isOrderByPriceDesc = false;
        string searchBy = null;
        string SearchBy { get => searchBy; set => searchBy = value.ToLower(); }
        IList<Product> OrderedProducts
        {
            get
            {
                var products = Catalog.ProductsList;
                if (!string.IsNullOrEmpty(SearchBy))
                {
                    products = products.Where(product =>
                        product.Name.ToLower().Contains(SearchBy) ||
                        product.Barcode.ToString().Contains(SearchBy)
                    );
                }
                if (isOrderByPriceDesc)
                {
                    products = products.OrderByDescending(product => product.Price);
                }
                else
                {
                    products = products.OrderBy(product => product.Price);
                }
                return products.ToList();
            }
        }
        readonly Menu SortMenu;
        void ChooseSortOrder()
        {
            Console.CursorTop = 0;
            SortMenu.Print();
            SortMenu.Action(Console.ReadKey().Key);
        }
        void SearchProducts()
        {
            Console.Clear();
            Console.WriteLine("Поиск товаров по названию и/или штрих-коду");
            Console.Write("Введите текст для поиска, или пустое значение, чтобы показать все товары:");
            SearchBy = Console.ReadLine();
        }

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
                    new MenuAction(ConsoleKey.F4, "Сортировать", ChooseSortOrder),
                    new MenuAction(ConsoleKey.F5, "Поиск", SearchProducts),
                    new MenuAction(ConsoleKey.F9, "Сохранить", SaveToFile),
                    new MenuAction(ConsoleKey.F10, "Загрузить", LoadFromFile),
                    new MenuClose(ConsoleKey.Tab, "Вернуться к чекам"),
                }
            );
            SortMenu = new Menu(new List<MenuItem>() {
                new MenuAction(ConsoleKey.DownArrow, "Сортировка по цене по убыванию",
                    () => isOrderByPriceDesc = true),
                new MenuAction(ConsoleKey.UpArrow, "Сортировка по цене по ворастанию",
                    () => isOrderByPriceDesc = false),
            });
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
            if (!string.IsNullOrEmpty(SearchBy))
            {
                Console.WriteLine("Выполнен поиск по строке \"{0}\"", SearchBy);
            }
            table.Print(OrderedProducts, SelectedProduct);
        }


        void SaveToFile()
        {
            SelectFile.SaveToFile(
                Catalog.ProductsList.Select(p => ProductFileDto.Map(p)),
                "Каталог товаров",
                "к каталогу"
            );
        }

        void LoadFromFile()
        {
            var loadedData = SelectFile.LoadFromFile<ProductFileDto>(
                "Каталог товаров",
                "к каталогу"
            );
            if (loadedData != null)
            {
                Catalog.LoadProducts(
                    loadedData.Select(p => ProductFileDto.Map(p))
                );
            }
        }
    }
}
