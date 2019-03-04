using CSVProcessor.Contracts.Service;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace CSVProcessor
{
    public class StartUp
    {
        /// <summary>
        /// CSV Base path
        /// </summary>
        private readonly string _csvBasePath;

        /// <summary>
        /// Service Object
        /// </summary>
        private ICSVProcessorService _processorService;

        public StartUp(ICSVProcessorService processorService)
        {
            _csvBasePath = ConfigurationManager.AppSettings["BasePath"] ?? throw new ArgumentException("Base Path is not defined.");
            _processorService = processorService ?? throw new ArgumentNullException(nameof(processorService) + "is null");
        }

        public void Run()
        {
            var csvFiles = new DirectoryInfo(_csvBasePath).GetFiles("*.csv", SearchOption.TopDirectoryOnly).Select(o => o.Name); //Directory.GetFiles(_csvBasePath, "*.csv", SearchOption.TopDirectoryOnly);

            Console.WriteLine("List of available CSV files: ");

            foreach (var file in csvFiles)
            {
                Console.WriteLine(file);
            }
            Console.Write("Please Enter a File Name: ");
            var fileName = Console.ReadLine();

            while (!csvFiles.Contains(fileName))
            {
                Console.Write("Please Enter a Valid File Name: ");
                fileName = Console.ReadLine();
            }

            Console.WriteLine("Read     -       1");
            Console.WriteLine("Write    -       2");
            Console.Write("Please Enter Choice: ");
            var choice = Console.ReadLine().ToString();

            if (int.TryParse(choice, out int result))
            {
                if (result == 1)
                {
                    var data = _processorService.GetData(fileName);
                    foreach (var item in data)
                    {
                        var properties = item.GetType().GetProperties();
                        foreach (var property in properties)
                        {
                            Console.Write(property.Name + ": " + property.GetValue(item) + "  ");
                        }
                        Console.WriteLine();
                    }
                }
                else if (result == 2)
                {
                    Console.WriteLine("Enter Comma separated CSV data");
                    var data = Console.ReadLine();

                    var success = _processorService.WriteData(fileName, data);

                    if (success)
                        Console.WriteLine("Updation Successfull.");
                    else
                        Console.WriteLine("Updation Unsuccessfull.");
                }
            }
        }
    }
}
