using HW02.AnalyticalDataContext.DB;
using HW02.BussinessContext;
using HW02.BussinessContext.DB.Entities;
using HW02.BussinessContext.DB.Enums;

namespace HW02.AnalyticalDataContext
{
    public class AnalyticalDataListener
    {
        private readonly AnalyticalDBContext _dbContext;
        public AnalyticalDataListener(AnalyticalDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void LogCommand(object sender, CommandEvaluatedEventArgs e)
        {
            if (e.IsSuccessful == null || !(bool)e.IsSuccessful || e.LogType is LogType.Other or LogType.Get) return;

            var analyticalDataLogs = _dbContext.ReadAnalyticalData();

            switch (e.Entity)
            {
                case Product product:
                {
                    var categoryId = product.CategoryId;
                    var existingCategoryLog = analyticalDataLogs.Find(log => log.CategoryId == categoryId);
                    
                    if (existingCategoryLog == null) return;

                    var updatedProductCnt = existingCategoryLog.ProductCount;
                    updatedProductCnt = e.LogType switch
                    {
                        LogType.Add => ++updatedProductCnt,
                        LogType.Delete => --updatedProductCnt,
                        _ => updatedProductCnt
                    };

                    SaveLogs(analyticalDataLogs, categoryId, existingCategoryLog.CategoryName, updatedProductCnt, existingCategoryLog);
                    break;
                }
                case Category category:
                    SaveLogs(analyticalDataLogs, e.Entity.Id, e.Entity.Name, 0);
                    break;
            }
        }

        private void SaveLogs(List<AnalyticalLog> analyticalDataLogs, int categoryId, string categoryName, int productCnt, 
            AnalyticalLog? existingAnalyticalLog = null)
        {
            var newLog = new AnalyticalLog(categoryId, categoryName, productCnt);

            if (existingAnalyticalLog != null)
            {
                var index = analyticalDataLogs.IndexOf(existingAnalyticalLog);
                analyticalDataLogs[index] = newLog;
            }
            else
            {
                analyticalDataLogs.Add(newLog);
            }
            _dbContext.SaveAnalyticalData(analyticalDataLogs);
        }
    }
}

