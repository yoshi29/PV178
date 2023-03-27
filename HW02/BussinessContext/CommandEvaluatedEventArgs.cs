using HW02.BussinessContext.DB.Entities;
using HW02.BussinessContext.DB.Enums;

namespace HW02.BussinessContext
{
    public class CommandEvaluatedEventArgs: EventArgs
    {
        public LogType LogType { get; }
        public ILoggable? Entity { get; }
        public Type? EntityType { get; }
        public bool? IsSuccessful { get; }
        public string? ErrorMessage { get; }
        public DateTime Timestamp => DateTime.Now;

        public CommandEvaluatedEventArgs(LogType logType, string? errorMessage = null)
        {
            LogType = logType;
            ErrorMessage = errorMessage;
        }

        public CommandEvaluatedEventArgs(LogType logType, ILoggable entity, bool isSuccessful,
            string? errorMessage = null)
        {
            LogType = logType;
            IsSuccessful = isSuccessful;
            Entity = entity;
            EntityType = entity.GetType();
            ErrorMessage = errorMessage;
        }

        public CommandEvaluatedEventArgs(LogType logType, Type entityType, bool isSuccessful,
            string? errorMessage = null)
        {
            LogType = logType;
            IsSuccessful = isSuccessful;
            Entity = null;
            EntityType = entityType;
            ErrorMessage = errorMessage;
        }
    }
}
