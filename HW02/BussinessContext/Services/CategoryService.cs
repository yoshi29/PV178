using HW02.BussinessContext.DB.Entities;
using HW02.BussinessContext.DB.Enums;
using HW02.BussinessContext.FileDatabase;
using HW02.Exceptions;
using HW02.Helpers;

namespace HW02.BussinessContext.Services
{
    public class CategoryService
    {
        private readonly CategoryDBContext _categoryDbContext;
        private readonly ProductDBContext _productDbContext;

        public CategoryService(CategoryDBContext categoryDbContext, ProductDBContext productDbContext)
        {
            _categoryDbContext = categoryDbContext;
            _productDbContext = productDbContext;
        }

        public List<Category> GetAllCategories() => _categoryDbContext.ReadCategories();

        /// <exception cref="EntityWithSameIdAlreadyExistException{Category}"></exception>
        public Category AddCategory(string name)
        {
            var categories = _categoryDbContext.ReadCategories();
            var newId = IdGenerationHelper.GenerateUniqueId(categories);
            var newCategory = new Category(newId, name);

            categories.Add(newCategory);

            try
            {
                _categoryDbContext.SaveCategories(categories);
            }
            catch (EntityWithSameIdAlreadyExistException<Category>)
            {
                throw new EntityWithSameIdAlreadyExistException<Category>(newCategory, LogType.Add);
            }

            return newCategory;
        }

        /// <exception cref="CategoryDoesNotExistException"></exception>
        /// <exception cref="CantDeleteProductWhenDeletingCategoryException"></exception>
        /// <exception cref="EntityWithSameIdAlreadyExistException{Category}"></exception>
        public Category DeleteCategory(int categoryId)
        {
            var categories = _categoryDbContext.ReadCategories();
            var deletedCategory = categories.Find(c => c.Id == categoryId);

            if (deletedCategory == null)
            {
                throw new CategoryDoesNotExistException(LogType.Delete, categoryId);
            }

            try
            {
                DeleteRelatedProducts(categoryId);
                categories.RemoveAll(c => c.Id == categoryId);
                _categoryDbContext.SaveCategories(categories);
            }
            catch (EntityWithSameIdAlreadyExistException<Product>)
            {
                throw new CantDeleteProductWhenDeletingCategoryException(deletedCategory, LogType.Delete);
            }
            catch (EntityWithSameIdAlreadyExistException<Category>)
            {
                throw new EntityWithSameIdAlreadyExistException<Category>(deletedCategory, LogType.Delete);
            }

            return deletedCategory;
        }

        /// <exception cref="EntityWithSameIdAlreadyExistException{Product}"></exception>
        private void DeleteRelatedProducts(int categoryId)
        {
            var products = _productDbContext.ReadProducts();
            products.RemoveAll(p => p.CategoryId == categoryId);

            _productDbContext.SaveProducts(products);
        }
    }
}
