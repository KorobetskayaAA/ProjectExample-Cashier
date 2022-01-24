using CashierDB.Model;
using CashierDB.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq.Expressions;

namespace CashierDB.Repositories
{
    public class BillRepository : CashierRepositoryBase
    {
        public BillRepository(CashierContext context) : base(context) {}

        public async Task<IEnumerable<BillDbDto>> GetAllAsync(int? statusId, string orderBy, bool orderAsc)
        {
            var bills =  context.Bills.AsQueryable();

            if (statusId != null)
            {
                bills = bills.Where(b => b.StatusId == statusId);
            }

            switch (orderBy) {
                case "date": bills = OrderByDate(orderAsc, bills); break;
                case "cost": bills = OrderBySum(orderAsc, bills); break;
                case "status": bills = OrderByStatus(orderAsc, bills); break;
            }

            return await bills
                .Include(b => b.Items)
                .Include(b => b.Status)
                .Include(b => b.Creator)
                .ToListAsync();
        }

        private static IQueryable<BillDbDto> OrderByStatus(bool orderAsc, IQueryable<BillDbDto> bills)
        {
            return OrderBy(orderAsc, bills, b => b.StatusId);
        }

        private static IQueryable<BillDbDto> OrderBySum(bool orderAsc, IQueryable<BillDbDto> bills)
        {
            return OrderBy(orderAsc, bills, b => b.Items.Sum(i => i.Price * (decimal)i.Amount));
        }

        private static IQueryable<BillDbDto> OrderByDate(bool orderAsc, IQueryable<BillDbDto> bills)
        {
            return OrderBy(orderAsc, bills, b => b.Created);
        }

        private static IQueryable<BillDbDto> OrderBy<TKey>(bool orderAsc, IQueryable<BillDbDto> bills,
            Expression<Func<BillDbDto, TKey>> expression)
        {
            if (orderAsc)
            {
                return bills.OrderBy(expression);
            }
            else
            {
                return bills.OrderByDescending(expression);
            }
        }

        public async Task<BillDbDto> GetAsync(long number)
        {
            return await context.Bills
                .Include(b => b.Items)
                .Include(b => b.Status)
                .Include(b => b.Creator)
                .FirstOrDefaultAsync(b => b.Number == number);
        }

        public void Create(BillDbDto bill)
        {
            context.Bills.Add(bill);
        }
    }
}
