using HW02.BussinessContext;
using HW02.BussinessContext.DB.Entities;
using HW02.LoggerContext;
using HW02.LoggerContext.DB;

namespace HW02
{
    public class LoggerListener
    {
        private readonly LoggerDBContext _dbContext;
        public LoggerListener(LoggerDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void LogCommand(object sender, CommandEvaluatedEventArgs e)
        {
            int? categoryId = null;
            if (e.Entity is Product product) categoryId = product.CategoryId;

            _dbContext.WriteLog(new Log(e, categoryId));
        }
    }
}
