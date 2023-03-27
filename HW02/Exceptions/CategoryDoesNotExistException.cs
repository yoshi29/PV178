using HW02.BussinessContext.DB.Entities;
using HW02.BussinessContext.DB.Enums;

namespace HW02.Exceptions
{
    public class CategoryDoesNotExistException : BaseException
    {
        public CategoryDoesNotExistException(Product product, LogType logType) : base(
            product, typeof(Product), logType, $"Category id {product.CategoryId} does not exist")
        {

        }

        public CategoryDoesNotExistException(LogType logType, int categoryId) : base(
            typeof(Product), logType, $"Category id {categoryId} does not exist")
        {

        }
    }
}
