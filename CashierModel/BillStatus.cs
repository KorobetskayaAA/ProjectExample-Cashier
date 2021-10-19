using System;
using System.Collections.Generic;
using System.Text;

namespace CashierModel
{
    public enum BillStatus
    {
        Editing,
        Active,
        Cancelled
    }

    public static class BillStatusRus
    {
        public static readonly Dictionary<BillStatus, string> Names =
            new Dictionary<BillStatus, string>()
            {
                { BillStatus.Editing, "Открыт" },
                { BillStatus.Active, "Закрыт" },
                { BillStatus.Cancelled, "Отмена" },
            };
    }
}
