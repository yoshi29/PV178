using HW02.BussinessContext.DB.Entities;
using HW02.BussinessContext.Services;

namespace HW02.BussinessContext.DB.Seeders
{
    public class DataSeeder
    {
        private readonly CommandEvaluator _commandEvaluator;

        public DataSeeder(CommandEvaluator commandEvaluator)
        {
            _commandEvaluator = commandEvaluator;
        }

        public void Seed()
        {
            _commandEvaluator.EvaluateCommand(new[] { "add-category", "Motorcycles" });
            _commandEvaluator.EvaluateCommand(new[] { "add-category", "Cars" });
            _commandEvaluator.EvaluateCommand(new[] { "add-category", "Tools" });
        }

        public void AddProductToCreatedCategory(object sender, CommandEvaluatedEventArgs e)
        {
            if (e.IsSuccessful == null || !(bool)e.IsSuccessful || e.Entity is not Category category) return;
            
            switch (category.Name)
            {
                case "Motorcycles":
                    _commandEvaluator.EvaluateCommand(new[] { "add-product", "Yamaha FZS600 Fazer", category.Id.ToString(), "95000" });
                    _commandEvaluator.EvaluateCommand(new[] { "add-product", "Suzuki SV 650S", category.Id.ToString(), "82000" });
                    break;
                case "Cars":
                    _commandEvaluator.EvaluateCommand(new[] { "add-product", "Ford Mustang GT500 Shelby Super Snake", category.Id.ToString(), "2500000" });
                    _commandEvaluator.EvaluateCommand(new[] { "add-product", "SSC Tuatara", category.Id.ToString(), "37000000" });
                    break;
                case "Tools":
                    _commandEvaluator.EvaluateCommand(new[] { "add-product", "Compressor", category.Id.ToString(), "28000" });
                    _commandEvaluator.EvaluateCommand(new[] { "add-product", "Torque wrench", category.Id.ToString(), "8000" });
                    break;
            }
        }
    }
}
