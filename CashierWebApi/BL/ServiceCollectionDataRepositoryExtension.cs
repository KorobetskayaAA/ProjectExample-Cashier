using CashierDB.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CashierWebApi.BL
{
    public static class ServiceCollectionDataRepositoryExtension
    {
        public static void AddCashierRepositories(this IServiceCollection services)
        {
            services.AddTransient<ProductRepository>();
        }

        public static void AddCashierServices(this IServiceCollection services)
        {
            services.AddTransient<ProductsService>();
        }
    }
}
