using System;
using Microsoft.Extensions.DependencyInjection;
using StringCalculator.Interfaces;
using StringCalculator.Services;

namespace StringCalculator
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        static void Main(string[] args)
        {
            RegisterServices();
            var service = _serviceProvider.GetService<IStringCalculatorService>();
            Console.WriteLine("String Calculator initialized.");
            Console.WriteLine("Enter string representing numbers separated by commas, and will return their sum or enter EXIT to Close ");
            Console.WriteLine("Enter String:");
            while (true)
            {
                var inputCommand = Console.ReadLine();

                if (inputCommand != null && inputCommand.ToUpper().Equals("EXIT"))
                    break;


                try
                {
                    var output = service.Add(inputCommand);
                    if (!string.IsNullOrWhiteSpace(output.ToString()))
                        Console.WriteLine($"=> {output}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine("Exited - press any key to close");
            Console.ReadLine();
            DisposeServices();
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<IStringCalculatorService, StringCalculatorService>();



            _serviceProvider = collection.BuildServiceProvider();
        }

        private static void DisposeServices()
        {
            switch (_serviceProvider)
            {
                case null:
                    return;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }
    }
}
