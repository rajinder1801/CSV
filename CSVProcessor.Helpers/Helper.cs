using CSVProcessor.Contracts.Helper;
using CSVProcessor.Contracts.Logging;
using System;
using System.Data;
using System.IO;

namespace CSVProcessor.Helpers
{
    public class Helper : IHelper
    {
        /// <summary>
        /// Logger
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        public Helper(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger) + "is null.");
        }

        /// <summary>
        /// File Append Function
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <returns>bool value</returns>
        public bool AppendDataTofile(string path, string data)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(data))
                return false;

            if (File.Exists(path))
            {
                File.AppendAllText(path, data);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check whether file exists/not
        /// </summary>
        /// <param name="path"></param>
        /// <returns>bool value</returns>
        public bool FileExists(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            return File.Exists(path);
        }

        /// <summary>
        /// Get data table from the csv file.
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public DataTable GetDataTableFromCSVFile(string strFilePath)
        {
            DataTable csvData = new DataTable();
            try
            {
                DataTable dt = new DataTable();
                using (StreamReader sr = new StreamReader(strFilePath))
                {
                    var headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header.Replace(" ", string.Empty).Trim().ToLower());
                    }
                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i];
                        }
                        dt.Rows.Add(dr);
                    }

                }

                return dt;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: ", ex);
                throw ex;
            }
        }
    }
}
