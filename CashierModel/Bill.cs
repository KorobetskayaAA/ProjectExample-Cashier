using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CashierModel
{
    public class Bill
    {
        static uint NextNumber { get; set; } = 1;
        public uint Number { get; }
        public DateTime Created { get; }

        public Bill()
        {
            Number = NextNumber++;
            Created = DateTime.Now;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("       КАССОВЫЙ ЧЕК №{0:0000000000}\n", Number));
            sb.Append(Created.ToLocalTime() + "\n");
            sb.Append("*************************************\n");
            sb.Append(string.Join("\n", items) + "\n");
            sb.Append("*************************************\n");
            var sum = Sum;
            decimal vat = 0.2m;
            sb.Append(string.Format("ИТОГ ={0,31:#,##0.00}\n", sum));
            sb.Append(string.Format("СУММА БЕЗ НДС ={0,22:#,##0.00}", sum / (1 + vat)));
            return sb.ToString();
        }

        #region Items
        readonly List<Item> items = new List<Item>();
        public IEnumerable<Item> Items => items;
        public Item this[Barcode barcode]
        {
            get => items.Find(item => item.Barcode == barcode);
        }
        public Item this[int index]
        {
            get => items[index];
        }

        public void AddItem(Product product, double amount = 1)
        {
            var existingItem = items.Find(item => item.Barcode == product.Barcode);
            if (existingItem != null)
            {
                existingItem.Amount += amount;
            }
            else
            {
                items.Add(new Item(product, amount));
            }
        }

        public void RemoveItem(Item item)
        {
            items.Remove(item);
        }

        public void RemoveItem(string barcode)
        {
            var item = items.Find(item => item.Barcode == barcode);
            RemoveItem(item);
        }

        public decimal Sum => items.Select(item => item.Cost).Sum();
        public decimal ItemsCount => items.Count;
        #endregion

        #region Status
        public BillStatus Status { get; private set; } = BillStatus.Editing;

        public void FinishEditing()
        {
            Status = BillStatus.Active;
        }

        public void Cancel()
        {
            Status = BillStatus.Cancelled;
        }
        #endregion
    }
}
