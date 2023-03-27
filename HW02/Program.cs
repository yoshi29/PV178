using HW02.AnalyticalDataContext;
using HW02.AnalyticalDataContext.DB;
using HW02.BussinessContext;
using HW02.BussinessContext.DB.Seeders;
using HW02.BussinessContext.FileDatabase;
using HW02.BussinessContext.Services;
using HW02.LoggerContext.DB;
using Microsoft.Extensions.DependencyInjection;

namespace HW02
{
    public class Program
    {
        public static void Main()
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            var commandEvaluator = serviceProvider.GetService<CommandEvaluator>();

            SubscribeToCommandEvaluatedEvent(commandEvaluator, serviceProvider);
            SeedData(commandEvaluator, serviceProvider);
            Console.WriteLine("Welcome to the eShop console interface!");

            var shouldEvaluateNextCommand = true;
            while (shouldEvaluateNextCommand)
            {
                var command = Console.ReadLine() ?? "";
                var commandParts = command.Split(' ');

                shouldEvaluateNextCommand = commandEvaluator.EvaluateCommand(commandParts);
            }

            UnsubscribeFromCommandEvaluatedEvent(commandEvaluator, serviceProvider);
        }

        private static void SeedData(CommandEvaluator commandEvaluator, ServiceProvider serviceProvider)
        {
            var dataSeeder = serviceProvider.GetService<DataSeeder>();
            commandEvaluator.CommandEvaluated += dataSeeder.AddProductToCreatedCategory;
            dataSeeder.Seed();
            commandEvaluator.CommandEvaluated -= dataSeeder.AddProductToCreatedCategory;
        }

        private static void SubscribeToCommandEvaluatedEvent(CommandEvaluator commandEvaluator, ServiceProvider serviceProvider)
        {
            commandEvaluator.CommandEvaluated += serviceProvider.GetService<LoggerListener>().LogCommand;
            commandEvaluator.CommandEvaluated += serviceProvider.GetService<AnalyticalDataListener>().LogCommand;
            commandEvaluator.CommandEvaluated += Printer.PrintInfo;
        }

        private static void UnsubscribeFromCommandEvaluatedEvent(CommandEvaluator commandEvaluator, ServiceProvider serviceProvider)
        {
            commandEvaluator.CommandEvaluated -= serviceProvider.GetService<LoggerListener>().LogCommand;
            commandEvaluator.CommandEvaluated -= serviceProvider.GetService<AnalyticalDataListener>().LogCommand;
            commandEvaluator.CommandEvaluated -= Printer.PrintInfo;
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<CategoryDBContext>();
            services.AddSingleton<ProductDBContext>();
            services.AddSingleton<LoggerDBContext>();
            services.AddSingleton<AnalyticalDBContext>();

            services.AddSingleton<CategoryService>();
            services.AddSingleton<ProductService>();

            services.AddSingleton<CommandEvaluator>();
            services.AddSingleton<DataSeeder>();

            services.AddSingleton<LoggerListener>();
            services.AddSingleton<AnalyticalDataListener>();

            return services;
        }
    }
}
