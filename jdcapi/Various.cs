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
        Watch_Player, 
        Chat_Minimum_Risk, 
        Chat_Minimum_Change, 
        Chat_Watch_Player, 
        Roll_Delay
    }

    public enum SettingsType_Boolean
    {
        
        AutoInvest,
        AllBetsMe,
        MuteChat,
        Shortcuts,
        Chatstake,
        Alarm
    }
}
