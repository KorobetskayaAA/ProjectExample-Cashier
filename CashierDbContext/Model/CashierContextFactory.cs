using Microsoft.EntityFrameworkCore.Design;

namespace CashierDB.Model
{
    public class CashierContextFactory : IDesignTimeDbContextFactory<CashierContext>
    {
        public CashierContext CreateDbContext(string[] args)
        {
            return new CashierContext();
        }
    }
}
