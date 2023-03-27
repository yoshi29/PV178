using HW02.BussinessContext.DB.Entities;
using HW02.BussinessContext.DB.Enums;

namespace HW02.Exceptions
{
    public class ProductDoesNotExist : BaseException
    {
        public ProductDoesNotExist(LogType logType, int productId) : base(
            typeof(Product), logType, $"Product with id {productId} does not exist") { }
    }
}
