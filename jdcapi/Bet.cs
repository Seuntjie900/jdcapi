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

    public class Roll
    {
        public long betid { get; set; }
        public DateTime date { get; set; }
        public long userid { get; set; }
        public decimal multiplier { get; set; }
        public decimal stake { get; set; }
        public decimal profit { get; set; }
        public decimal chance { get; set; }
        public decimal target { get; set; }
        public bool high { get; set; }
        public decimal lucky { get; set; }
        public int result { get; set; }
        public string hash { get; set; }
        public string server_seed { get; set; }
        public string client_seed { get; set; }
        public long nonce { get; set; }
    }
}
