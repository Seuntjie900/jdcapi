using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDCAPI
{
    public class init
    {
        public string balance { get; set; }
        public string bankroll { get; set; }
        public double bet { get; set; }
        public long bets { get; set; }
        public double chance { get; set; }
        public string csrf { get; set; }
        public double edge { get; set; }
        public double fee { get; set; }
        public string[] ignores { get; set; }
        public double investment { get; set; }
        public double invest_pft { get; set; }
        public string losses { get; set; }
        public string luck { get; set; }
        public Double max_profit { get; set; }
        public string name { get; set; }
        public string nonce { get; set; }
        public double percent { get; set; }
        public string profit { get; set; }
        public string seed { get; set; }
        public Settings settings { get; set; }
        public string shash { get; set; }
        public string uid { get; set; }
        public string username { get; set; }
        public string wagered { get; set; }
        public string wins { get; set; }
        public Stats stats { get; set; }
        public string wdaddr {get; set;}
        public ArrayList chat { get; set; }
    }
    public class Settings
    {
        public string btcaddr { get; set; }
        public string watch_player { get; set; }
        public double roll_delay {get; set; }
        public int autoinvest { get; set; }
        public double chat_min_risk { get; set; }
        public double chat_min_change { get; set; }
       public double min_risk { get; set; }
        public double  chat_watch_player { get; set; }

    }
    public class initbase
    {
        public string name { get; set; }
        public init args { get; set; }
    }
}
