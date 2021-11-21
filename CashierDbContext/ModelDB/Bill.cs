using System;
using System.Collections.Generic;

#nullable disable

namespace CashierDbContext.ModelDB
{
    public partial class Bill
    {
        public long Number { get; set; }
        public DateTime Created { get; set; }
        public int Status { get; set; }

        public virtual BillStatus StatusNavigation { get; set; }
    }
}
