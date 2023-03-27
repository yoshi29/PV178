using HW02.BussinessContext.DB.Entities;
using HW02.BussinessContext.DB.Enums;
using HW02.Exceptions;

namespace HW02
{
    public class EntityWithSameIdAlreadyExistException<T> : BaseException where T : ILoggable
    {
        public EntityWithSameIdAlreadyExistException(T entity) : base(
            entity, entity.GetType(), LogType.Other, $"{entity} already exists")
        {

        }

        public EntityWithSameIdAlreadyExistException(T entity, LogType logType) : base(
            entity, entity.GetType(), logType, $"{entity} already exists")
        {

        }
    }
}
