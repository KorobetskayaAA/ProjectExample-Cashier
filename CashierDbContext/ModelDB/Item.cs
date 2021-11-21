using System;
using System.Collections.Generic;

#nullable disable

namespace CashierDbContext.ModelDB
{
    public partial class Item
    {
        public long BillNumber { get; set; }
        public long Barcode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public float Quantity { get; set; }

        public virtual Product BarcodeNavigation { get; set; }
        public virtual Bill BillNumberNavigation { get; set; }
    }
}
