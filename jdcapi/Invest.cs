using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDCAPI
{
    public class Invest
    {
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public decimal Profit { get; set; }
        public decimal Offsite { get; set; }
    }
    public class InvestError
    {
        public string msg { get; set; }
    }
    public class DivestError
    {
        public string msg { get; set; }
    }

}
