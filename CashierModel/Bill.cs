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

        #region Items
        public readonly List<Item> items = new List<Item>();
        public IEnumerable<Item> Items => items;
        public Item this[string barcode]
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
