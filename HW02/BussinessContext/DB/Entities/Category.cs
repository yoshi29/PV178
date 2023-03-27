namespace HW02.BussinessContext.DB.Entities
{
    public class Category : ILoggable
    {
        public int Id { get; }
        public string Name { get; }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
