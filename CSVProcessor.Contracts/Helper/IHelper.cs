using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVProcessor.Contracts.Helper
{
    public interface IHelper
    {
        bool FileExists(string path);

        DataTable GetDataTableFromCSVFile(string filePath);

        bool AppendDataTofile(string path, string data);

    }
}
