using CashierDB.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CashierWebApi.BL
{
    public static class ServiceCollectionDataRepositoryExtension
    {
        public static void AddCashierRepositories(this IServiceCollection services)
        {
            services.AddTransient<ProductRepository>();
            services.AddTransient<BillStatusRepository>();
            services.AddTransient<BillRepository>();
        }

        public static void AddCashierServices(this IServiceCollection services)
        {
            services.AddTransient<ProductsService>();
            services.AddTransient<BillsService>();
        }
    }
}
