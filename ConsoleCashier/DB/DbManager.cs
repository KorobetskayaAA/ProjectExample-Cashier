using CashierDB.Model;
using CashierModel;
using ConsoleCashier.DB.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCashier.DB
{
    static class DbManager
    {
        static string connectionString = ConnectionManager.GetConnectionString();

        static CashierContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CashierContext>();
            var options = optionsBuilder
                    .UseSqlServer(connectionString)
                    .Options;
            return new CashierContext(options);
        }

        public static List<Bill> GetBills()
        {
            using (var context = CreateContext())
            {
                return context.Bills
                    .Include(bill => bill.Status)
                    .Include(bill => bill.Items)
                    .Select(bill => BillMapper.Map(bill))
                    .ToList();
            }
        }

        public static void UpdateBills(IEnumerable<Bill> bills)
        {
            using (var context = CreateContext())
            {
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    // у чека может поменяться только статус
                    foreach (var billToUpdate in context.Bills)
                    {
                        billToUpdate.StatusId = (int)bills
                            .First(b => b.Number == billToUpdate.Number)
                            .Status;
                    }
                    context.SaveChanges();

                    var billsToAdd = bills
                        .Where(bill => context.Bills.Find(bill.Number) == null)
                        .Select(bill => BillMapper.Map(bill))
                        .ToList();

                    var itemsToAdd = billsToAdd
                        .SelectMany(bill => bill.Items)
                        .ToList();

                    context.AddRange(itemsToAdd);
                    context.AddRange(billsToAdd);
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Произошла ошибка при сохранении чеков в базу: " + ex.Message);
                    transaction.Rollback();
                }
            }
        }

        public static List<Product> GetProducts()
        {
            using (var context = CreateContext())
            {
                return context.Products
                    .Select(product => ProductMapper.Map(product))
                    .ToList();
            }
        }

        public static void UpdateProducts(IEnumerable<Product> products)
        {
            using (var context = CreateContext())
            {
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    var productsInDb = context.Products
                        .AsNoTracking();
                    

                    foreach (var productInDb in productsInDb)
                    {
                        var product = products.FirstOrDefault(pModel => pModel.Barcode == productInDb.Barcode);
                        if (product != null)
                        {
                            //*
                            context.Update(ProductMapper.Map(product));
                            /*/ //или
                            productInDb.Barcode = product.Barcode;
                            productInDb.Name = product.Name;
                            productInDb.Price = product.Price;
                            //*/
                        }
                        else
                        {
                            context.Remove(productInDb);
                        }
                    }
                    context.SaveChanges();

                    var productsToAdd = products
                        .Where(p => context.Products.Find(p.Barcode.ToString()) == null)
                        .Select(p => ProductMapper.Map(p))
                        .ToList();
                    context.AddRange(productsToAdd);
                    context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Произошла ошибка при сохранении товаров в базу: " + ex.Message);
                    transaction.Rollback();
                }
            }
        }
    }
}
