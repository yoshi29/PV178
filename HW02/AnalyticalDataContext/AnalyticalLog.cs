namespace HW02.AnalyticalDataContext
{
    public class AnalyticalLog
    {
        public int CategoryId { get; }
        public string CategoryName { get; }
        public int ProductCount { get; }


        public AnalyticalLog(int categoryId, string categoryName, int productCount)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            ProductCount = productCount;
        }
    }
}
