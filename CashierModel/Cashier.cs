using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CashierModel
{
    public class Cashier
    {
        List<Bill> bills = new List<Bill>();
        public IEnumerable<Bill> Bills => bills;

        public Bill OpenNewBill()
        {
            var bill = new Bill();
            bills.Add(bill);
            return bill;
        }

        public Bill FindBill(uint number)
        {
            return bills.Find(bill => bill.Number == number);
        }

        public IEnumerable<Bill> FindBills(BillStatus status)
        {
            return bills.Where(bill => bill.Status == status);
        }
    }
}
