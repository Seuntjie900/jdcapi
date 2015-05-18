using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDCAPI
{
    public class Various
    {
        public string name { get; set; }
        public ArrayList args { get; set; }
    }

    public class SeedInfo
    {
        public string OldServerSeed { get; set; }
        public string OldServerHash { get; set; }
        public string OldClientSeed { get; set; }
        public string TotalRolls { get; set; }
        public string NewServerHash { get; set; }
    }

    public class Address
    {
        public string DepositAddress { get; set; }
        public string ImageHTML { get; set; }
        public string Note { get; set; }
    }
 
    public enum SettingsType_Numeric
    {
        Minimum_Risk, 
        Minimum_Change, 
        Chat_Minimum_Risk, 
        Chat_Minimum_Change, 
        Roll_Delay,
        Max_Double
    }

    public enum SettingsType_String
    {
        Watch_Player,
        Chat_Watch_Player,
        Email,
        Emergency_Address
    }

    public enum SettingsType_Boolean
    {
        
        AutoInvest,
        AllBetsMe,
        MuteChat,
        Shortcuts,
        Chatstake,
        Alarm,
        max_bet
    }

    public class ReceivedObject
    {
        public string name { get; set; }
        public object[] args { get; set; }
    }
}
