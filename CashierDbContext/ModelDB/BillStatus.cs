using System;
using System.Collections.Generic;

#nullable disable

namespace CashierDbContext.ModelDB
{
    public sealed class BillStatus
    {
        public BillStatus()
        {
            Bills = new HashSet<Bill>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Bill> Bills { get; set; }
    }
}
