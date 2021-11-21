using System;
using System.Collections.Generic;

#nullable disable

namespace CashierDbContext.ModelDB
{
    public partial class Product
    {
        public long Barcode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
