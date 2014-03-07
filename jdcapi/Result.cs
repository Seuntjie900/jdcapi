using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDCAPI
{
    public class Result
    {
        public string bankroll { get; set; }
        public string bet { get; set; }
        public double betid { get; set; }
        public string bets { get; set; }
        public string chance { get; set; }
        public long date { get; set; }
        public bool high { get; set; }
        public string luck { get; set; }
        public double lucky { get; set; }
        public string max_profit { get; set; }
        public string name { get; set; }
        public long nonce { get; set; }
        public double payout { get; set; }
        public string ret { get; set; }
        public string this_profit { get; set; }
        public string uid { get; set; }
        public string wagered { get; set; }
        public bool win { get; set; }
        public decimal investment { get; set; }
        public decimal percent { get; set; }
        public double invest_pft { get; set; }
        public Stats stats { get; set; }
        public string balance { get; set; }
        public string profit { get; set; }
        


    }



    public class Stats
    {
        public double bets { get; set; }
        public double profit { get; set; }
        public double wins { get; set; }
        public double losses { get; set; }
        public double purse { get; set; }
        public double commission { get; set; }
        public double wagered { get; set; }
        public double luck { get; set; }
    }
}
