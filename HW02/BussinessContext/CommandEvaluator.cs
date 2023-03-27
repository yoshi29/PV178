using HW02.BussinessContext.DB.Entities;
using HW02.BussinessContext.DB.Enums;
using HW02.BussinessContext.Services;
using HW02.Exceptions;

namespace HW02.BussinessContext
{
    public class CommandEvaluator
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;

        public event EventHandler<CommandEvaluatedEventArgs> CommandEvaluated;
        private void OnCommandEvaluated(CommandEvaluatedEventArgs e) => CommandEvaluated?.Invoke(this, e);


        public CommandEvaluator(CategoryService categoryService, ProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public bool EvaluateCommand(string[] commandParts)
        {
            var arguments = commandParts.Skip(1).ToArray();
            var commandName = commandParts[0];

            try
            {
                switch (commandName)
                {
                    case "list-products":
                        ListProductsCommand(arguments, commandName);
                        break;

                    case "get-products-by-category":
                        GetProductsByCategoryCommand(arguments, commandName);
                        break;

                    case "add-product":
                        AddProductCommand(arguments, commandName);
                        break;

                    case "delete-product":
                        DeleteProductCommand(arguments, commandName);
                        break;

                    case "list-categories":
                        ListCategoriesCommand(arguments, commandName);
                        break;

                    case "add-category":
                        AddCategoryCommand(arguments, commandName);
                        break;

                    case "delete-category":
                        DeleteCategoryCommand(arguments, commandName);
                        break;

                    case "exit":
                        return false;

                    default:
                        OnCommandEvaluated(new CommandEvaluatedEventArgs(LogType.Other, $"Command {commandName} is not supported"));
                        break;
                }
            }
            catch (BaseException ex)
            {
                OnCommandEvaluated(ex.Entity != null
                    ? new CommandEvaluatedEventArgs(ex.LogType, ex.Entity, false, ex.Message)
                    : new CommandEvaluatedEventArgs(ex.LogType, ex.EntityType, false, ex.Message));
            }

            return true;
        }

        private void ListProductsCommand(string[] arguments, string commandName)
        {
            if (arguments.Length == 0)
            {
                var products = _productService.GetAllProducts();
                if (products.Count != 0) Printer.PrintEntitiesAsTable(products);
                else Printer.PrintInfo("No products.");

                OnCommandEvaluated(new CommandEvaluatedEventArgs(LogType.Get, typeof(Product), true));
            }
            else
            {
                OnCommandEvaluated(new CommandEvaluatedEventArgs(LogType.Get, typeof(Product), false,
                    $"Invalid arguments for command {commandName}"));
            }
        }

        private void GetProductsByCategoryCommand(string[] arguments, string commandName)
        {
            if (AreArgumentsValid(arguments, typeof(int)))
            {
                var productsByCategory = _productService.GetProductsByCategoryId(int.Parse(arguments[0]));
                if (productsByCategory.Count != 0) Printer.PrintEntitiesAsTable(productsByCategory);
                else Printer.PrintInfo("No product in this category.");

                OnCommandEvaluated(new CommandEvaluatedEventArgs(LogType.Get, typeof(Product), true));
            }
            else
            {
                OnCommandEvaluated(new CommandEvaluatedEventArgs(LogType.Get, typeof(Product), false,
                    $"Invalid arguments for command {commandName}"));
            }
        }

        private void AddProductCommand(string[] arguments, string commandName)
        {
            if (AreArgumentsValid(arguments, typeof(string), typeof(int), typeof(decimal)))
            {
                var newProduct = _productService.AddProduct(arguments[0], int.Parse(arguments[1]), decimal.Parse(arguments[2]));
                OnCommandEvaluated(new CommandEvaluatedEventArgs(LogType.Add, newProduct, true));
            }
            else
            {
                OnCommandEvaluated(new CommandEvaluatedEventArgs(LogType.Add, typeof(Product), false,
                    $"Invalid arguments for command {commandName}"));
            }
        }

        private void DeleteProductCommand(string[] arguments, string commandName)
        {
            if (AreArgumentsValid(arguments, typeof(int)))
            {
                var deletedProduct = _productService.DeleteProduct(int.Parse(arguments[0]));
                OnCommandEvaluated(new CommandEvaluatedEventArgs(LogType.Delete, deletedProduct, true));
            }
            else
            {
                OnCommandEvaluated(new CommandEvaluatedEventArgs(LogType.Delete, typeof(Product), false,
                    $"Invalid arguments for command {commandName}"));
            }
        }

        private void ListCategoriesCommand(string[] arguments, string commandName)
        {
            if (arguments.Length == 0)
            {
                var categories = _categoryService.GetAllCategories();
                if (categories.Count != 0) Printer.PrintEntitiesAsTable(categories);
                else Printer.PrintInfo("No categories.");

                OnCommandEvaluated(new CommandEvaluatedEventArgs(LogType.Get, typeof(Category), true));
            }
            else
            {
                OnCommandEvaluated(new CommandEvaluatedEventArgs(LogType.Get, typeof(Category), false,
                    $"Invalid arguments for command {commandName}"));
            }
        }

        private void AddCategoryCommand(string[] arguments, string commandName)
        {
            if (AreArgumentsValid(arguments, typeof(string)))
            {
                var newCategory = _categoryService.AddCategory(arguments[0]);
                OnCommandEvaluated(new CommandEvaluatedEventArgs(LogType.Add, newCategory, true));
            }
            else
            {
                OnCommandEvaluated(new CommandEvaluatedEventArgs(LogType.Add, typeof(Category), false,
                    $"Invalid arguments for command {commandName}"));
            }
        }

        private void DeleteCategoryCommand(string[] arguments, string commandName)
        {
            if (AreArgumentsValid(arguments, typeof(int)))
            {
                var deletedCategory = _categoryService.DeleteCategory(int.Parse(arguments[0]));
                OnCommandEvaluated(new CommandEvaluatedEventArgs(LogType.Delete, deletedCategory, true));
            }
            else
            {
                OnCommandEvaluated(new CommandEvaluatedEventArgs(LogType.Delete, typeof(Category), false,
                    $"Invalid arguments for command {commandName}"));
            }
        }

        private bool AreArgumentsValid(string[] args, params Type[] expectedArgTypes)
        {
            var expectedArgCnt = expectedArgTypes.Length;

            if (args.Length != expectedArgCnt) return false;

            for (var i = 0; i < expectedArgCnt; i++)
            {
                try
                {
                    _ = Convert.ChangeType(args[i], expectedArgTypes[i]);
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
