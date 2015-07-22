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
        public decimal offsite { get; set; }
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
        public decimal stake_pft { get; set; }
        public decimal offset { get; set; }
        public GoogleAuthSettings ga { get; set; }
    }
    public class Settings
    {

        public Settings()
        {
            btcaddr = "";
            watch_player = "";
            roll_delay = 0;
            autoinvest = false;
            min_risk = 0;
            min_change = 0;
            max_double = 0;
            chat_min_risk = 8;
            chat_min_change = 8;
            chat_watch_player = "";
            allbetsme = true;
            shortcuts = true;
            chatstake = true;
            mutechat = false;
            alarm = false;

        }
        //settings":{"btcaddr":"1M1zUqZUZg6AH4KQdq9AUoouQgWHdQzySd","watch_player":null,"roll_delay":null,"autoinvest":1,"chat_min_risk":5,"chat_min_change":5,"min_risk":1,"chat_watch_player":null,"min_change":1,"max_double":null,"allbetsme":1,"mutechat":0,"shortcuts":1,"chatstake":0,"alarm":0}
        public string btcaddr { get; set; }
        public string watch_player { get; set; }
        public double roll_delay { get; set; }
        public bool autoinvest { get; set; }
        public double chat_min_risk { get; set; }
        public double chat_min_change { get; set; }
        public double min_risk { get; set; }
        public string chat_watch_player { get; set; }
        public double min_change { get; set; }
        public double max_double { get; set; }
        public bool allbetsme { get; set; }
        public bool mutechat { get; set; }
        public bool shortcuts { get; set; }
        public bool chatstake { get; set; }
        public bool alarm { get; set; }

    }
    public class initbase
    {
        public string name { get; set; }
        public init args { get; set; }
    }
}
