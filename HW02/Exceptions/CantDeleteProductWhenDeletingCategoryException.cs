using HW02.BussinessContext.DB.Entities;
using HW02.BussinessContext.DB.Enums;

namespace HW02.Exceptions
{
    public class CantDeleteProductWhenDeletingCategoryException : BaseException
    {
        public CantDeleteProductWhenDeletingCategoryException(Category entity, LogType logType) : base(
            entity, typeof(Category), logType, $"Can't delete products when deleting category {entity}")
        {
        }
    }
}
