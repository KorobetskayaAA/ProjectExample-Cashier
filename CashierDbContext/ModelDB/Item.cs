using System;
using System.Collections.Generic;

#nullable disable

namespace CashierDbContext.ModelDB
{
    public sealed class Item
    {
        public long BillNumber { get; set; }
        public long Barcode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public float Quantity { get; set; }

        public Product BarcodeNavigation { get; set; }
        public Bill BillNumberNavigation { get; set; }
    }
}
