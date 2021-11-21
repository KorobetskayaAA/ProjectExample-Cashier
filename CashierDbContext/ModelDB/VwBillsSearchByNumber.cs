using System;
using System.Collections.Generic;

#nullable disable

namespace CashierDbContext.ModelDB
{
    public sealed class VwBillsSearchByNumber
    {
        public long Number { get; set; }
        public DateTime Created { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int? ItemsCount { get; set; }
        public double? BillSum { get; set; }
    }
}
