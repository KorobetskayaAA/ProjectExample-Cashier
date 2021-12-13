using CashierDB.Repositories;
using CashierWebApi.BL.Exceptions;
using CashierWebApi.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashierWebApi.BL
{
    public class ProductsService
    {
        protected ProductRepository repository;

        public ProductsService(ProductRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<ProductApiDto>> GetAsync(string search, bool? sortPriceAsc)
        {
            var products = await repository.GetAllAsync(search, sortPriceAsc);
            return products.Select(p => new ProductApiDto(p)).ToList();
        }

        public async Task<(ProductApiDto, Exception)> GetAsync(string barcode)
        {
            (bool validBarcode, string validationMessage) = ValidateBarcode(barcode);
            if (!validBarcode)
            {
                return (null, new ArgumentException(validationMessage, "barcode"));
            }
            var product = await repository.GetAsync(barcode);
            if (product == null)
            {
                return (null, new KeyNotFoundException($"Товар со штрих-кодом {barcode} не найден"));
            }
            return (new ProductApiDto(product), null);
        }

        public async Task<Exception> CreateAsync(ProductApiDto product)
        {
            (bool validBarcode, string validationMessage) = ValidateBarcode(product.Barcode);
            if (!validBarcode)
            {
                return new ArgumentException(validationMessage, "barcode");
            }

            var productToCreate = product.Create();
            repository.Create(productToCreate);

            try
            {
                await repository.SaveAsync();
            }
            catch (Exception ex)
            {
                if (await repository.Exists(product.Barcode))
                {
                    return new AlreadyExistsException($"Товар со штрих-кодом {product.Barcode} уже существует.");
                }
                return ex;
            }
            return null;
        }

        public async Task<Exception> UpdateAsync(string barcode, ProductApiDto product)
        {
            if (!string.IsNullOrEmpty(product.Barcode) && barcode != product.Barcode)
            {
                return new ArgumentException("Штрих-код не совпадает", "barcode");
            }

            (bool validBarcode, string validationMessage) = ValidateBarcode(barcode);
            if (!validBarcode)
            {
                return new ArgumentException(validationMessage, "barcode");
            }

            var productToUpdate = await repository.GetAsync(barcode);
            if (productToUpdate == null)
            {
                return new KeyNotFoundException($"Товар со штрих-кодом ${barcode} не найден.");
            }
            product.Update(productToUpdate);
            repository.Update(productToUpdate);

            try
            {
                await repository.SaveAsync();
            }
            catch (Exception ex)
            {
                return new Exception("Произошла ошибка при сохранении данных", ex);
            }
            return null;
        }

        public async Task<(ProductApiDto, Exception)> DeleteAsync(string barcode)
        {
            (bool validBarcode, string validationMessage) = ValidateBarcode(barcode);
            if (!validBarcode)
            {
                return (null, new ArgumentException(validationMessage, "barcode"));
            }

            var deletedProduct = await repository.DeleteAsync(barcode);
            if (deletedProduct == null)
            {
                return (null, new KeyNotFoundException($"Товар со штрих-кодом ${barcode} не найден."));
            }

            try
            {
                await repository.SaveAsync();
            }
            catch (Exception ex)
            {
                return (null, new Exception("Произошла ошибка при сохранении данных", ex));
            }

            return (new ProductApiDto(deletedProduct), null);
        }

        (bool, string) ValidateBarcode(string barcode)
        {
            return barcode.Length == 13 && long.TryParse(barcode, out _) ?
                (true, null)
                : (false, $"Штрих-код {barcode} должен быть 13-значным числом.");
        }
    }
}
