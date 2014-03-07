using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDCAPI
{
    public class Chat
    {
        public string Message { get; set; }
        public string User { get; set; }
        public string UID { get; set; }
        public DateTime Date { get; set; }
    }
    public class initchat
    {
        public string name { get; set; }
        public string user { get; set; }
        public string txt { get; set; }

        public Chat ConvertToChat(string Date)
        {
            Chat tmp = new Chat();
            tmp.Date = jdInstance.ToDateTime(Date);
            tmp.Message = txt;
            tmp.UID = user;
            tmp.User = name;
            return tmp;
        }
    }
    public class baseChat
    {
        public string name { get; set; }
        public string[] args { get; set; }

        public Chat ConvertToChat()
        {
            string Message = "";
            string Name = "";
            string UID = "";
            DateTime Date = new DateTime();

            UID = (args[0].Substring(0, args[0].IndexOf('<')-1)).Replace("(", "").Replace(")", "");
            Name = (args[0].Substring(args[0].IndexOf('<')+1, args[0].IndexOf('>')- (args[0].IndexOf('<')+1)));
            Message = args[0].Substring(args[0].IndexOf('>') + 2);
            Date = DateTime.Parse(args[1].Replace("T"," ").Replace("Z",""));
            Date += (DateTime.Now - DateTime.UtcNow);
            Chat tmp = new Chat();
            tmp.Message = Message;
            tmp.UID = UID;
            tmp.User = Name;
            tmp.Date = Date;
            return tmp;
        }
    }
}
