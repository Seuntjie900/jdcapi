using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JDCAPI
{
    public class GoogleAuthSettings
    {
        public bool active { get; set; }
        public int failures { get; set; }
        public GAFlags flags { get; set; }
        public int last { get; set; }
    }

    public class GAFlags
    {
        public bool login { get; set; }
        public bool withdraw { get; set; }
        public bool invest { get; set; }
        public bool divest { get; set; }
        public bool edit { get; set; }
        public bool manage_api_keys { get; set; }
        public bool play { get; set; }
        public bool randomize { get; set; }        
    }

    public class SetupGa
    {
        public string secret { get; set; }
        public bool active { get; set; }
        public int failures { get; set; }
        public GAFlags flags { get; set; }
        public string qrCode { get; set; }
    }
}
