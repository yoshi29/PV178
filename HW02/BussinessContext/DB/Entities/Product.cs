namespace HW02.BussinessContext.DB.Entities
{
    public class Product : ILoggable
    {
        public int Id { get; }
        public string Name { get; }
        public int CategoryId { get; }
        public decimal Price { get; }

        public Product(int id, string name, int categoryId, decimal price)
        {
            Id = id;
            Name = name;
            CategoryId = categoryId;
            Price = price;
        }
    }
}
