using CSVProcessor.Contracts.Helper;
using CSVProcessor.Contracts.Logging;
using CSVProcessor.Contracts.Repository;
using CSVProcessor.Contracts.Service;
using CSVProcessor.Helpers;
using CSVProcessor.Logging;
using CSVProcessor.Repository;
using CSVProcessor.Service;
using System;
using Unity;

namespace CSVProcessor
{
    class Program
    {
        static void Main(string[] args)
        {            
            var container = new UnityContainer();
            
            // Do registrations.
            RegisterTypes(container);

            // Let Unity resolve ProgramStarter and create a build plan.
            var program = container.Resolve<StartUp>();

            Console.WriteLine("All done. Starting program...");
            program.Run();

            Console.WriteLine("Press Any Key to Exit.");
            Console.ReadKey();

        }

        private static void RegisterTypes(UnityContainer container)
        {
            container.RegisterSingleton<ICSVProcessorRepository, CSVProcessorRepository>();
            container.RegisterSingleton<ICSVProcessorService, CSVProcessorService>();
            container.RegisterSingleton<ILogger, Logger>();
            container.RegisterSingleton<IHelper, Helper>();
        }
    }
}
