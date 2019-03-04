using System.Collections.Generic;

namespace CSVProcessor.Contracts.Service
{
    /// <summary>
    /// Service Interface
    /// </summary>
    public interface ICSVProcessorService
    {
        /// <summary>
        /// Reads, Parses and returns a type list from a csv.
        /// </summary>
        /// <param name="csvName">Name of the csv file.</param>
        /// <returns>A type list.</returns>
        IEnumerable<object> GetData(string csvName);

        /// <summary>
        /// Writes the data supplied to the csv.
        /// </summary>
        /// <param name="csvName">Name of the csv file.</param>
        /// <param name="data">The data to be written to the csv.</param>
        /// <returns>A boolean value.</returns>
        bool WriteData(string csvName, string data);
    }
}
