using System;
using System.Collections.Generic;

#nullable disable

namespace CashierDbContext.ModelDB
{
    public sealed class Bill
    {
        public long Number { get; set; }
        public DateTime Created { get; set; }
        public int Status { get; set; }

        public BillStatus StatusNavigation { get; set; }
    }
}
