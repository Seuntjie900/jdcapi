using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JDCAPI
{
    public class Stake
    {
        public decimal bankroll { get; set; }
        public long date { get; set; }
        public decimal max_profit { get; set; }
        public decimal stake { get; set; }
        public decimal total { get; set; }
        public decimal investment { get; set; }
        public decimal percent { get; set; }
        public decimal invest_pft { get; set; }
        public decimal stake_pft { get; set; }
        
    }
    public class StakeBase
    {
        public string name { get; set; }
        public Stake[] args { get; set; }
    }
}
