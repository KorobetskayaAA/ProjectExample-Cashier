using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CashierModel
{
    public class Cashier
    {
        List<Bill> bills;
        public IEnumerable<Bill> Bills => bills;

        public Cashier(List<Bill> bills = null)
        {
            if (bills != null)
            {
                this.bills = bills;
            }
            else
            {
                this.bills = new List<Bill>();
            }
        }

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
