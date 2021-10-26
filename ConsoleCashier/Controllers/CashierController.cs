﻿using CashierModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCashier
{
    class CashierController
    {
        public Cashier Cashier { get; private set; }
        public Catalog Catalog { get; }

        Func<Bill, object> orderBy = bill => bill.Created;
        BillStatus? FilterByStatus = null;
        public IList<Bill> OrderedBills => 
            (FilterByStatus != null 
                ? Cashier.FindBills(FilterByStatus ?? BillStatus.Active) 
                : Cashier.Bills
            )
            .OrderBy(orderBy)
            .ToList();
        readonly Menu SortMenu;
        readonly Menu StatusMenu;
        void ChooseOrder()
        {
            Console.CursorTop = 0;
            SortMenu.Print();
            SortMenu.Action(Console.ReadKey().Key);
        }
        void ChooseStatus()
        {
            Console.CursorTop = 0;
            StatusMenu.Print();
            StatusMenu.Action(Console.ReadKey().Key);
        }
        void SearchBillByName()
        {
            Console.Clear();
            Console.WriteLine("Поиск чека по номеру");
            uint searchNumber = InputValidator.ReadBillNuber();
            Bill foundBill = Cashier.FindBill(searchNumber); 
            if (foundBill != null)
            {
                SelectBill.SelectedNode = foundBill;
            }
            else
            {
                Console.WriteLine("Чек с номером {0} не найден", searchNumber);
                Console.WriteLine(
                    "Нажмите любую клавишу, чтобы продолжить..."
                );
                Console.ReadKey();
            }
        }

        Table<Bill> table = new Table<Bill>(new[] {
            new TableColumn<Bill>("№", 10,
                bill => string.Format("{0:0000000000}", bill.Number)),
            new TableColumn<Bill>("От", 20,
                bill => bill.Created.ToString("G")),
            new TableColumn<Bill>("Кол-во", 7,
                bill => bill.ItemsCount.ToString()),
            new TableColumn<Bill>("Сумма", 12,
                bill => string.Format("{0:#,##0.00}", bill.Sum)),
            new TableColumn<Bill>("Статус", 8,
                bill => BillStatusRus.Names[bill.Status]),
        });
        public Menu Menu { get; }
        public SelectFromList<Bill> SelectBill { get; }
        public Bill SelectedBill => SelectBill.SelectedNode;

        public CashierController(Catalog catalog, List<Bill> bills = null)
        {
            Catalog = catalog;
            Cashier = new Cashier(bills);
            SelectBill = new SelectFromList<Bill>(() => OrderedBills);
            Menu = new Menu(new List<MenuItem>(SelectBill.Menu.Items) {
                new MenuAction(ConsoleKey.F1, "Новый чек", FillBill),
                new MenuAction(ConsoleKey.F2, "Печать чека",
                    () => PrintBill(SelectedBill)),
                new MenuAction(ConsoleKey.F3, "Отменить чек",
                    () => CancelBill(SelectedBill)),
                new MenuAction(ConsoleKey.F4, "Сортировать",
                    ChooseOrder),
                new MenuAction(ConsoleKey.F5, "Фильтр по статусу",
                    ChooseStatus),
                new MenuAction(ConsoleKey.F6, "Поиск по номеру",
                    SearchBillByName),
                new MenuAction(ConsoleKey.F9, "Сохранить", 
                    SaveToFile),
                new MenuAction(ConsoleKey.F10, "Загрузить", 
                    LoadFromFile),
            });
            SortMenu = new Menu(new List<MenuItem>() {
                new MenuAction(ConsoleKey.D1, "Сортировка по дате",
                    () => orderBy = bill => bill.Created),
                new MenuAction(ConsoleKey.D2, "Сортировка по сумме",
                    () => orderBy = bill => bill.Sum),
                new MenuAction(ConsoleKey.D3, "Сортировка по статусу",
                    () => orderBy = bill => bill.Status),
            });
            StatusMenu = new Menu(
                new List<MenuItem>(
                    BillStatusRus.Names.Select((status, index) =>
                        new MenuAction(index + ConsoleKey.D1,
                            status.Value,
                            () => FilterByStatus = status.Key)
                        ))
                { 
                    new MenuAction(ConsoleKey.D0, "Все", () => FilterByStatus = null)
                }
            );
        }

        public void FillBill()
        {
            Bill bill = Cashier.OpenNewBill();
            Console.Clear();
            Console.WriteLine("Заполнение чека №{0:0000000000}", bill.Number);
            do
            {
                Console.WriteLine("\n{0}.", bill.ItemsCount + 1);
                string barcode = InputValidator.ReadBarcode(Catalog.Barcodes);
                Product product = Catalog[barcode];
                Console.WriteLine("{0} - {1:C2}", product.Name, product.Price);
                double amount = InputValidator.ReadAmount();
                bill.AddItem(product, amount);
                Console.WriteLine(
                    "Нажмите Enter, чтобы завершить, или любую другую клавишу, чтобы продолжить."
                );
            } while (Console.ReadKey().Key != ConsoleKey.Enter);
            bill.FinishEditing();
            PrintBill(bill);
        }

        public void PrintBill(Bill bill)
        {
            Console.Clear();
            Console.WriteLine(bill);
            Console.WriteLine(
                "Нажмите любую клавишу, чтобы продолжить..."
            );
            Console.ReadKey();
        }

        public void CancelBill(Bill bill)
        {
            Console.Clear();
            Console.WriteLine("Вы уверены, что хотете выполнить отмену чека №{0} от {1}?",
                bill.Number, bill.Created);

            Console.WriteLine("Нажмите C, чтобы подтвердить удаление, или любую другую клавишу, чтобы отменить");
            if (Console.ReadKey().Key == ConsoleKey.C)
            {
                bill.Cancel();
            }
        }

        public void PrintAllBills()
        {
            table.Print(OrderedBills, SelectedBill);
        }
        void SaveToFile()
        {
            SelectFile.SaveToFile(
                Cashier.Bills.Select(b => BillFileDto.Map(b)),
                "История продаж",
                "к списку чеков"
            );
        }

        void LoadFromFile()
        {
            var loadedData = SelectFile.LoadFromFile<BillFileDto>(
                "История продаж",
                "к списку чеков"
            );
            if (loadedData != null)
            {
                Cashier = new Cashier(
                    loadedData.Select(p => BillFileDto.Map(p)).ToList()
                );
            }
        }
    }
}
