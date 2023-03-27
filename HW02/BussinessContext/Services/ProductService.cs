using HW02.BussinessContext.DB.Entities;
using HW02.BussinessContext.DB.Enums;
using HW02.BussinessContext.FileDatabase;
using HW02.Exceptions;
using HW02.Helpers;

namespace HW02.BussinessContext.Services
{
    public class ProductService
    {
        private readonly ProductDBContext _productDbContext;
        private readonly CategoryDBContext _categoryDbContext;

        public ProductService(ProductDBContext productDbContext, CategoryDBContext categoryDbContext)
        {
            _productDbContext = productDbContext;
            _categoryDbContext = categoryDbContext;
        }

        public List<Product> GetAllProducts() => _productDbContext.ReadProducts();

        public List<Product> GetProductsByCategoryId(int categoryId)
            => _productDbContext.ReadProducts().Where(p => p.CategoryId == categoryId).ToList();

        /// <exception cref="CategoryDoesNotExistException"></exception>
        /// <exception cref="EntityWithSameIdAlreadyExistException{T}"></exception>
        public Product AddProduct(string name, int categoryId, decimal price)
        {
            var products = _productDbContext.ReadProducts();
            var newId = IdGenerationHelper.GenerateUniqueId(products);
            var newProduct = new Product(newId, name, categoryId, price);

            var existingCategory = _categoryDbContext.ReadCategories().Find(c => c.Id == categoryId);
            if (existingCategory == null)
            {
                throw new CategoryDoesNotExistException(newProduct, LogType.Add);
            }

            products.Add(newProduct);

            try
            {
                _productDbContext.SaveProducts(products);
            }
            catch (EntityWithSameIdAlreadyExistException<Product>)
            {
                throw new EntityWithSameIdAlreadyExistException<Product>(newProduct, LogType.Add);
            }

            return newProduct;
        }


        /// <exception cref="ProductDoesNotExist"></exception>
        /// <exception cref="EntityWithSameIdAlreadyExistException{Product}"></exception>
        public Product DeleteProduct(int productId)
        {
            var products = _productDbContext.ReadProducts();
            var removedProduct = products.Find(p => p.Id == productId);

            if (removedProduct == null)
            {
                throw new ProductDoesNotExist(LogType.Delete, productId);
            }

            products.Remove(removedProduct);

            try
            {
                _productDbContext.SaveProducts(products);
            }
            catch (EntityWithSameIdAlreadyExistException<Product>)
            {
                throw new EntityWithSameIdAlreadyExistException<Product>(removedProduct, LogType.Add);
            }

            return removedProduct;
        }
    }
}
