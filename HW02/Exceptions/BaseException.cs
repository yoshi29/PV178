using HW02.BussinessContext.DB.Entities;
using HW02.BussinessContext.DB.Enums;

namespace HW02.Exceptions
{
    public abstract class BaseException : Exception {
        public ILoggable? Entity { get; }
        public Type EntityType { get; }
        public LogType LogType { get; }

        protected BaseException(Type entityType, LogType logType, string message) : base(message)
        {
            LogType = logType;
            EntityType = entityType;
        }

        protected BaseException(ILoggable entity, Type entityType, LogType logType, string message) : base(message)
        {
            Entity = entity;
            EntityType = entityType;
            LogType = logType;
        }
    }
}
