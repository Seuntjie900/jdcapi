using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDCAPI
{
    public class Bet
    {
        public string bet { get; set; }
        public double betid { get; set; }
        public string chance { get; set; }
        public long date { get; set; }
        public bool high { get; set; }
        public double lucky { get; set; }
        public string name { get; set; }
        public long nonce { get; set; }
        public double payout { get; set; }
        public string returned { get; set; }
        public string this_profit { get; set; }
        public string uid { get; set; }

        
    }
}
