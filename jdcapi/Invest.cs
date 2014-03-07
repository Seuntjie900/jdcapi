using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDCAPI
{
    public class Invest
    {
        public decimal i { get; set; }
        public decimal p { get; set; }
        public decimal pft { get; set; }
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
