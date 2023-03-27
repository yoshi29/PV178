using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW02.BussinessContext;
using HW02.BussinessContext.DB.Entities;
using HW02.BussinessContext.DB.Enums;

namespace HW02.LoggerContext
{
    public class Log
    {
        private CommandEvaluatedEventArgs Data { get; }
        private int? CategoryId { get; }

        public Log(CommandEvaluatedEventArgs commandEvaluatedEventArgs, int? categoryId = null)
        {
            Data = commandEvaluatedEventArgs;
            CategoryId = categoryId;
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            var entityType = Data.Entity?.GetType().Name ?? Data.EntityType?.Name;

            var entityTypeStrPart = entityType != null ? $"; {entityType}" : "";
            var resultStrPart = Data.IsSuccessful != null
                ? (bool)Data.IsSuccessful ? "; Success" : "; Failure"
                : null;
            var errorMsgStrPart = Data.ErrorMessage != null ? $"; {Data.ErrorMessage}" : "";
            var categoryIdStrPart = CategoryId != null ? $"; {CategoryId}" : "";

            sb.Append($"[{Data.Timestamp}] {Data.LogType}{entityTypeStrPart}{resultStrPart}{errorMsgStrPart}");

            if (Data.Entity != null) sb.Append($"; {Data.Entity.Id}; {Data.Entity.Name}{categoryIdStrPart}");

            return sb.ToString();
        }
    }
}
