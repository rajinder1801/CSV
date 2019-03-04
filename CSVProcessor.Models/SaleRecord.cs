using System;

namespace CSVProcessor.Models
{
    public class SaleRecord
    {
        public string Country { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderID { get; set; }
        public int UnitsSold { get; set; }
        public double TotalCost { get; set; }
    }

}
