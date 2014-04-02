using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Threading;
using System.Net;

namespace JDCAPI
{
    public class jdInstance
    {
        HttpWebRequest request;
        string conid = "";
        string id = "";
        string csrf = "";
        string xhrval = "";
        
        bool inconnection = false;
        string host = "https://just-dice.coom";
        bool active = false;
        string privatehash = "";
        Thread poll = null;
        private bool gotinit = false;
        private bool logginging = false;
        string sUsername = "";
        string sPassword = "";
        string sGAcode = "";
        private string hash { get { return privatehash; } set { privatehash = value; } }
        public double Balance { get; private set; }
        public double Bankroll { get; private set; }
        public long Bets { get; private set; }
        public double Chance { get; private set; }
        public double Edge { get; private set; }
        public double Fee { get; private set; }
        public string[] Ignores { get; private set; }
        public decimal Investment { get; set; }
        public double Invest_pft { get; private set; }
        public long Losses { get; private set; }
        public string Luck { get; private set; }
        public double MaxProfit { get; private set; }
        public string Name { get; private set; }
        public long Nonce { get; private set; }
        public decimal Percent { get; private set; }
        public decimal Profit { get; private set; }
        public string seed { get; private set; }
        public Settings Settings { get; private set; }
        public string shash { get; private set; }
        public string uid { get; private set; }
        public string Username { get; private set; }
        public decimal Wagered { get; private set; }
        public long Wins { get; private set; }
        public Stats Stats { get; private set; }
        public string WDAddress { get; private set; }
        public string SecretHash { get {return hash;}}
        private Random RandomSeedGen = new Random();

        public jdInstance()
        {
            //AvailableBets = new List<JDCAPI.Bet>();
            //ChatMessages = new List<Chat>();
        }


        /// <summary>
        /// Connects to Just dice or doge dice
        /// </summary>
        /// <param name="DogeDice">if set to true, will attempt to connect to doge dice</param>
        /// <returns>if connected successfully, returns true</returns>
        public bool Connect(bool DogeDice)
        {
            host = "https://"+((DogeDice)?"doge":"just")+"-dice.com";            
            //Initial request for getting headers and cookies from site
            getInitalHeaders();
            getxhrval();
            gotinit = false;
            while (!gotinit)
            {
                 GetInfo();
            }
            active = true;
            if (poll != null && poll.IsAlive)
            {
                active = false;
                poll.Abort();
            }
            active = true;

            poll = new Thread(new ThreadStart(pollingLoop));
            poll.Start();
            return true;
        }


        /// <summary>
        /// Log into the site using a secret hash
        /// This simply sets the hash before doing the normal log in. Works great.
        /// </summary>
        /// <param name="DogeDice">If set to true, Site will connect to DogeDice, otherwise to Just-Dice</param>
        /// <param name="secretHash">The secret hash for the account you are attempting to connect to</param>
        /// <returns></returns>
        public bool Connect(bool DogeDice, string secretHash)
        {
            privatehash = secretHash;
            return Connect(DogeDice);
        }

        /// <summary>
        /// Log in using a username and password. You need a secret as well for this to work
        /// because you cannot get the login form without the hash. I guess i could use a random account
        /// hash so that the paramater isn't required, but will leave it there for now.
        /// </summary>
        /// <param name="DogeDice">If set to true, Site will connect to DogeDice, otherwise to Just-Dice</param>
        /// <param name="Username">username to log in with. Case sensitive</param>
        /// <param name="Password">Password to log in with. Case sensitive</param>
        /// <param name="GACode">Google Auth code.</param>
        /// <param name="secretHash">Hash from the hash cookie</param>
        /// <returns></returns>
        public bool Connect(bool DogeDice, string Username, string Password, string GACode)
        {
            privatehash = (!DogeDice) ? "0f3aa87b64103349a9cabcccbb312e606e9013c3eee8f364b9ee4e91ad2c67d3" : "0fc4126d7045e16c05d18b6fda82324c2f987ac7f51317f317d6488680a37668";
            host = "https://" + ((DogeDice) ? "doge" : "just") + "-dice.com";
            getInitalHeaders();
            sUsername = Username;
            sPassword = Password;
            string Message = string.Format("username={0}&password={1}&code={2}", Username, Password, GACode);            
            var tmprequest = (HttpWebRequest)HttpWebRequest.Create(host);
            tmprequest.ContentType = "application/x-www-form-urlencoded";
            tmprequest.ContentLength = Message.Length;
            tmprequest.Referer = host;
            tmprequest.CookieContainer = request.CookieContainer;            
            tmprequest.Method = "POST";
            using (var writer = new StreamWriter(tmprequest.GetRequestStream()))
            {
                string writestring = Message as string;
                writer.Write(writestring);
            }
            HttpWebResponse EmitResponse = (HttpWebResponse)tmprequest.GetResponse();
            string sEmitResponse = new StreamReader(EmitResponse.GetResponseStream()).ReadToEnd();            
            getxhrval();
            while (!gotinit)
            {
                GetInfo();
            }
            active = true;
            Thread poll = new Thread(new ThreadStart(pollingLoop));
            poll.Start();
            return true;
        }

        /// <summary>
        /// Gets the xhr polling information
        /// </summary>
        private void getxhrval()
        {
            var getxhrval = (HttpWebRequest)HttpWebRequest.Create(host+"/socket.io/1/" + "?t=" + CurrentDate());
            getxhrval.Referer = host;

            getxhrval.CookieContainer = request.CookieContainer;
           
            HttpWebResponse respGetxhrVal = (HttpWebResponse)getxhrval.GetResponse();
            string xhrString = new StreamReader(respGetxhrVal.GetResponseStream()).ReadToEnd();
            foreach (Cookie cookievalue in respGetxhrVal.Cookies)
            {
                request.CookieContainer.Add(cookievalue);
                switch (cookievalue.Name)
                {
                    case "connect.sid": conid = cookievalue.Value; break;
                    case "__cfduid": id = cookievalue.Value; break;
                    case "hash": hash = cookievalue.Value; break;
                }

            }
            xhrval = xhrString.Split(':')[0];
        }

        /// <summary>
        /// gets the inital cookies for the connections.
        /// cookies includeL __cfduid, connect.sid, hash. These are required for mainting the same connection
        /// </summary>
        private void getInitalHeaders()
        {

            request = (HttpWebRequest)HttpWebRequest.Create(host);
            var cookies = new CookieContainer();
            request.CookieContainer = cookies;
            if (!string.IsNullOrEmpty(privatehash))
            {
                request.CookieContainer.Add(new Cookie("hash", privatehash, "/", (host.Contains("just")) ? ".just-dice.com" : ".doge-dice.com"));
            }
            //request.CookieContainer.Add(new Cookie("cf_clearance", "bc22bf9b9733912f976dc28c78796fc91e19b7fe-1393330223-86400", "/", ".just-dice.com"));
            HttpWebResponse Response = null;
            try
            {
                
                Response = (HttpWebResponse)request.GetResponse();
                
            }
            catch (WebException e)
            {
                Response = (HttpWebResponse)e.Response;
                string s1 = new StreamReader(Response.GetResponseStream()).ReadToEnd();
                int indexofa = s1.IndexOf("a.value");
                string aval = s1.Substring(indexofa+10);
                aval = aval.Substring(0, aval.IndexOf(";")).Replace(" ","");
                int op1 = int.Parse(aval.Substring(0, aval.IndexOf("+")));
                int p1 = aval.IndexOf("+") + 1;
                int p2 = aval.IndexOf("*") - aval.IndexOf("+");
                int op2 = int.Parse(aval.Substring(aval.IndexOf("+")+1,aval.IndexOf("*")-aval.IndexOf("+")-1));
                int op3 = int.Parse(aval.Substring(aval.IndexOf("*")+1));
                aval = ((op2 * op3) + op1).ToString();
                int.Parse(aval);
                string jschl_vc = s1.Substring(s1.IndexOf(" name=\"jschl_vc\" value=\""));
                jschl_vc = jschl_vc.Substring(("name=\"jschl_vc\" value=\"").Length + 1);
                int len = jschl_vc.IndexOf("\"/>\n");
                jschl_vc = jschl_vc.Substring(0, len);
                try
                {
                    //string content = new WebClient().DownloadString("cdn-cgi/l/chk_jschl?jschl_vc=1bb30f6e73b41c8dd914ccbf64576147&jschl_answer=84");
                    CookieContainer cookies2 = request.CookieContainer;
                    string req = string.Format(host + "/cdn-cgi/l/chk_jschl?jschl_vc={0}&jschl_answer={1}",  jschl_vc,int.Parse(aval) + 13);
                    request = (HttpWebRequest)HttpWebRequest.Create(req);
                    request.CookieContainer = cookies2;
                    Response = (HttpWebResponse)request.GetResponse();
                }
                catch
                {

                }
            }
            

            string s = new StreamReader(Response.GetResponseStream()).ReadToEnd();

            foreach (Cookie cookievalue in Response.Cookies)
            {
                request.CookieContainer.Add(cookievalue);
                switch (cookievalue.Name)
                {
                    case "connect.sid": conid = cookievalue.Value; break;
                    case "__cfduid": id = cookievalue.Value; break;
                    case "hash":
                    case "hash ":
                    case " hash":
                    case " hash ": hash = cookievalue.Value; break;
                }

            }
            
            Response = (HttpWebResponse)request.GetResponse();
            s = new StreamReader(Response.GetResponseStream()).ReadToEnd();
            foreach (Cookie cookievalue in Response.Cookies)
            {
                request.CookieContainer.Add(cookievalue);
                switch (cookievalue.Name)
                {
                    case "connect.sid": conid = cookievalue.Value; break;
                    case "__cfduid": id = cookievalue.Value; break;
                    case "hash":
                    case "hash ":
                    case " hash":
                    case " hash ": hash = cookievalue.Value; break;
                }

            }
            
            bool founhash = false;
            for (int i = 0; i < request.CookieContainer.GetCookies(request.RequestUri).Count; i++)
            {
                if (request.CookieContainer.GetCookies(request.RequestUri)[i].Name == "hash")
                {
                    founhash = true;
                    break;
                }
            }

            for (int i = 0; i < Response.Headers.Count && !founhash; i++)
            {
                string hash = Response.Headers[i];
                if (hash.Contains("hash"))
                {
                    string[] tmpCookies = hash.Split(';');
                    foreach (string CurCookie in tmpCookies)
                    {
                        if (CurCookie.Contains("hash"))
                        {
                            string HashValue = CurCookie.Split('=')[1];
                            request.CookieContainer.Add(new Cookie("hash", HashValue,"/",".just-dice.com"));
                            privatehash = HashValue;
                            founhash = true;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// not yet implemented, will not be needed after ping has been added
        /// </summary>
        /// <returns></returns>
        public void Reconnect()
        {
            if (OnJDError!=null)
            {
                Various var = new Various();
                var.name = "reconnect";
                var.args = new System.Collections.ArrayList();
                var.args.Add("Reconnecting");
                OnJDError(var);
            }
            Disconnect();
            if (!string.IsNullOrEmpty(sUsername) & !string.IsNullOrEmpty(sPassword))
            {
                Connect((host.Contains("doge") ? true : false), sUsername, sPassword, sGAcode);
            }
            else
            {
                Connect((host.Contains("doge") ? true : false),privatehash);
            }
        }

        /// <summary>
        /// disconnects all current connections. Any emits will not work after this.
        /// </summary>
        public void Disconnect()
        {
            active = false;
            //Clear all of the cookies
            request = (HttpWebRequest)HttpWebRequest.Create(host);
            hash = "";
            conid = "";
            id = "";
            Thread.Sleep(300);
        }

        /// <summary>
        /// Returns date in milliseconds since 1970/01/01 00:00:00, as used by socket.io
        /// </summary>
        /// <returns>string</returns>
        public static string CurrentDate()
        {
            TimeSpan dt = DateTime.UtcNow - DateTime.Parse("1970/01/01 00:00:00");
            double mili = dt.TotalMilliseconds;
            return ((long)mili).ToString();
            
        }

        /// <summary>
        /// Converts to current date and time for local time zone
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(string milliseconds)
        {
            DateTime tmpDate = DateTime.Parse("1970/01/01 00:00:00");
            tmpDate = tmpDate.AddMilliseconds(long.Parse(milliseconds));
            tmpDate += (DateTime.Now - DateTime.UtcNow);
            return tmpDate;   
        }

        public static DateTime ToDateTime2(string milliseconds)
        {
            DateTime tmpDate = DateTime.Parse("1970/01/01 00:00:00");
            tmpDate = tmpDate.AddSeconds(long.Parse(milliseconds));
            tmpDate += (DateTime.Now - DateTime.UtcNow);
            return tmpDate;
        }

        private void pollingLoop()
        {
            while (active)
            {
                if (!inconnection)
                {
                    inconnection = true;
                    GetInfo();
                    inconnection = false;
                    
                }
                Thread.Sleep(100);
                
            }
            
        }
        int dcount = 0;
        private void GetInfo()
        {
            var MaintainConnectoin = (HttpWebRequest)HttpWebRequest.Create(host+"/socket.io/1/xhr-polling/" + xhrval + "?t=" + CurrentDate());

            MaintainConnectoin.CookieContainer = request.CookieContainer;
            MaintainConnectoin.Referer = host;
            MaintainConnectoin.Timeout = 10000;
            try
            {
                HttpWebResponse Response2 = (HttpWebResponse)MaintainConnectoin.GetResponse();

                string s2 = new StreamReader(Response2.GetResponseStream()).ReadToEnd();
                foreach (Cookie cookievalue in Response2.Cookies)
                {
                    switch (cookievalue.Name)
                    {
                        //case "connect.sid": conid = cookievalue.Value; break;
                        //case "__cfduid": id = cookievalue.Value; break;
                        case "hash": hash = cookievalue.Value; break;
                    }

                }
                if (s2 != "")
                {
                    if (logging)
                        writelog(s2);
                    //UpdateLog(s2);
                    if (s2 == ("7:::1+0"))
                    {
                        if (++dcount > 5)
                        {
                            dcount = 0;
                            Thread trecon = new Thread(new ThreadStart(Reconnect));
                            trecon.Start();
                            
                        }
                    }
                    else
                    {
                        StartPorcessing(s2);

                    }
                }

            }
            catch (Exception e)
            {
                string s2 = e.Message;
                if (logging)
                    writelog(s2);
                //active = false;
            }
        }

        private void StartPorcessing(string s2)
        {
            s2 = s2.Replace(":::", ((char)011).ToString());
            string[] returns = s2.Split((char)011);
            foreach (string s in returns)
            {
                if (s.Contains("pong"))
                {
                    if (OnPong != null)
                    {
                        OnPong();
                    }
                }
                if (s.Contains("\"result\""))
                {
                    ProcessResult(s);
                }
                else if (s.Contains("init"))
                {
                    ProcessInit(s);
                }
                else if (s.Contains("\"chat\""))
                {
                    ProcessChat(s);
                }
                else if (s.Contains("set_hash"))
                {
                    ProcessSetHash(s);
                }
                else if (s.Contains("old_results"))
                {
                    ProcessOldResults(s);
                }
                else if (s.Contains("timeout"))
                {
                    if (OnTimeout != null && !logginging)
                        OnTimeout();
                }
                else if (s.Contains("dismiss") && !logginging)
                {
                    if (OnDismiss != null)
                        OnDismiss();
                }
                else //for everything that uses the various class as output
                {
                    try
                    {
                        Various tmp = ProcessVarious(s);
                        switch (tmp.name)
                        {
                            case "invest": if (onInvest != null && !logginging) onInvest(tmp); break;
                            case "invest_error": if (OnInvestError != null && !logginging) OnInvestError(tmp); break;
                            case "divest_error": if (OnDivestError != null && !logginging) OnDivestError(tmp); break;
                            case "wdaddr": if (OnWDAddress != null && !logginging) OnWDAddress(tmp); break;
                            case "balance": if (OnBalance != null && !logginging) OnBalance(tmp); break;
                            case "shash": if (OnSecretHash != null && !logginging) OnSecretHash(tmp); break;
                            case "seed": if (OnClientSeed != null && !logginging) OnClientSeed(tmp); break;
                            case "bad_seed": if (OnBadClientSeed != null && !logginging) OnBadClientSeed(tmp); break;
                            case "nonce": if (OnNonce != null && !logginging) OnNonce(tmp); break;
                            case "jderror": if (OnJDError != null && !logginging) OnJDError(tmp); break;
                            case "jdmsg": if (OnJDMessage != null && !logginging) OnJDMessage(tmp); break;
                            case "form_error": if (OnFormError != null && !logginging) OnFormError(tmp); break;
                            case "login_error": if (OnLoginError != null && !logginging) OnLoginError(tmp); break;
                            case "wins": if (OnWins != null && !logginging) OnWins(tmp); break;
                            case "losses": if (OnLossess != null && !logginging) OnLossess(tmp); break;
                            //case "details": if (OnDetails != null && !logginging) OnDetails(tmp); break;
                            case "max_profit": if (OnMaxProfit != null && !logginging) OnMaxProfit(tmp); break;
                            case "new_client_seed": if (OnNewClientSeed != null && !logginging) OnNewClientSeed(tmp); break;
                            case "address": if (OnAddress != null && !logginging) OnAddress(tmp); break;
                            case "pong": if (OnPong != null && !logginging) OnPong(); break;
                            case "reload": Reconnect(); break;
                        }
                    }
                    catch
                    {
                        if (s.Contains("details"))
                        {
                            string s3 = s.Replace(" ", "") ;
                            Roll tmp = new Roll();
                            string date = s3.Substring(s3.IndexOf("moment") + 8, 10);
                            tmp.date = ToDateTime2(date);
//                            tmp.date = DateTime.Parse(date);
                           
                            string id = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
                            id = id.Substring(id.IndexOf(">") + 1);
                            tmp.betid = long.Parse(id);

                            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
                            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
                            string user = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
                            user = user.Substring(user.IndexOf(">") + 1);
                            tmp.userid = long.Parse(user);
                            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
                            string multiplier = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
                            multiplier = multiplier.Substring(multiplier.IndexOf(">") + 1).Replace("x","");
                            tmp.multiplier = decimal.Parse(multiplier);
                            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
                            string stake = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
                            stake = stake.Substring(stake.IndexOf(">") + 1).ToLower().Replace("doge","").Replace("btc","").Replace(" ","");
                            tmp.stake = decimal.Parse(stake);
                            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
                            string profit = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
                            profit = profit.Substring(profit.IndexOf(">") + 1).ToLower().Replace("doge", "").Replace("btc", "").Replace(" ", "");
                            tmp.profit = decimal.Parse(profit);
                            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
                            string chance = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
                            chance = chance.Substring(chance.IndexOf(">") + 1).ToLower().Replace("%", "").Replace(" ", "");
                            tmp.chance = decimal.Parse(chance);
                            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
                            if (s3.Contains("&gt"))
                                tmp.high=true;
                            else
                                tmp.high=false;
                            //s3 = s3.Substring(s3.IndexOf("</span>") + 7);
                            string target = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
                            target = target.Substring(target.IndexOf(">") + 1).ToLower().Replace("%", "").Replace(" ", "");
                            tmp.target = decimal.Parse(target);
                            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
                            string lucky = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
                            lucky = lucky.Substring(lucky.IndexOf(">") + 1).ToLower().Replace("%", "").Replace(" ", "");
                            tmp.lucky = decimal.Parse(lucky);
                            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
                            string result = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
                            result = result.Substring(result.IndexOf(">") + 1);
                            tmp.result = result.Contains("lose") ? 0 : 1;
                            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
                            string hash = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
                            hash = hash.Substring(hash.IndexOf(">") + 1);
                            tmp.hash = hash;
                            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
                            string server = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
                            server = server.Substring(server.IndexOf(">") + 1);
                            tmp.server_seed = server;
                            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
                            string client = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
                            client = client.Substring(client.IndexOf(">") + 1);
                            tmp.client_seed = client;
                            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
                            string nonce = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
                            nonce = nonce.Substring(nonce.IndexOf(">") + 1).ToLower().Replace(" ", "");
                            tmp.nonce = long.Parse(nonce);
                            if (OnRoll != null && !logginging) OnRoll(tmp);
                        }
                    }
                } 
            }
        }

        #region processing socket.on results from getinfo

        private void ProcessResult(string JsonString)
        {
            int start = JsonString.IndexOf("[") + 1;
            int length = JsonString.IndexOf("]") - start;
            start = JsonString.IndexOf('[') + 1;
            length = JsonString.LastIndexOf(']') - start + 1;
            JsonString = JsonString.Substring(start, length);
            Result tmp = json.JsonDeserialize<Result>(JsonString);
            if (tmp.bankroll != null)
            {
                Bankroll = double.Parse(tmp.bankroll);
                MaxProfit = double.Parse(tmp.max_profit);
                Investment = tmp.investment;
                Invest_pft = tmp.invest_pft;
                Percent = tmp.percent;
                Stats = tmp.stats;                
                if (tmp.balance == null)
                {
                    tmp.balance = Balance.ToString();
                }
                else
                {
                    Balance = double.Parse(tmp.balance);
                }
                if (tmp.profit == null)
                {
                    tmp.profit = Profit.ToString();
                }
                else
                {
                    Profit = decimal.Parse(tmp.profit);
                }
                
                if (OnResult != null)
                    OnResult(tmp, (tmp.uid == uid) ? true : false);

                Bet tmp2 = new Bet();
                tmp2.bet = tmp.bet;
                tmp2.betid = tmp.betid;
                tmp2.chance = tmp.chance;
                tmp2.date = tmp.date;
                tmp2.high = tmp.high;
                tmp2.lucky = tmp.lucky;
                tmp2.name = tmp.name;
                tmp2.nonce = tmp.nonce;
                tmp2.payout = tmp.payout;
                tmp2.returned = tmp.ret;
                tmp2.this_profit = tmp.this_profit;
                tmp2.uid = tmp.uid;
                if (OnBet != null && !logginging)
                    OnBet(tmp2, (tmp2.uid == uid) ? true : false);
                

            }
        }

        private void ProcessChat(string JsonString)
        {
            int start = JsonString.IndexOf("{");
            int length = JsonString.LastIndexOf("}") - start + 1;
             JsonString = JsonString.Substring(start, length);
             JsonString = JsonString.Replace(((char)011).ToString(), "");
             baseChat tmp = json.JsonDeserialize<baseChat>(JsonString);
             if (OnChat != null && !logginging)
                OnChat(tmp.ConvertToChat());
        }

        private void ProcessSetHash(string JsonString)
        {
            int start = JsonString.IndexOf("{");
            int length = JsonString.LastIndexOf("}") - start + 1;
            string jsonstring = JsonString.Substring(start, length);
            jsonstring = jsonstring.Replace(((char)011).ToString(), "");
            Various tmp = json.JsonDeserialize<Various>(jsonstring);
            bool found = false;
            foreach (Cookie c in request.CookieContainer.GetCookies(new Uri(host)))
            {
                if (c.Name == "hash")
                {

                    c.Value = tmp.args[0].ToString();
                    found = true;
                }
            }
            if (!found)
            {
                request.CookieContainer.Add(new Cookie("hash", tmp.args[0].ToString(), "/", ".just-dice.com"));

            }
            privatehash = tmp.args[0].ToString();
            gotinit = false;
            getxhrval();
            while (!gotinit)
            {
                GetInfo();
            }

        }

        private void ProcessOldResults(string JsonString)
        {
            gotinit = true;
            int start = JsonString.IndexOf("{");
            int length = JsonString.LastIndexOf("}") - start + 1;
            /*start = JsonString.IndexOf('[') + 1;
            length = JsonString.LastIndexOf(']') - start + 1;*/
            JsonString = JsonString.Substring(start, length);
            start = JsonString.IndexOf('[');
            JsonString = JsonString.Remove(start, 1);
            length = JsonString.LastIndexOf(']');
            JsonString = JsonString.Remove(length, 1);
            try
            {
                oldbets tmp = json.JsonDeserialize<oldbets>(JsonString);
                for (int i = 0; i < tmp.args.Length; i++)
                {

                    if (OnBet != null && !logginging)
                    {
                        OnBet(tmp.args[i], (tmp.args[i].uid == uid) ? true : false);
                    }
                }
            }
            catch
            {

            }
            logginging = false;
        }

        private void ProcessInit(string JsonString)
        {
            
            int start = JsonString.IndexOf("{");
            int length = JsonString.LastIndexOf("}") - start + 1;
           /* JsonString = JsonString.Remove(JsonString.IndexOf('['), 1);
            JsonString = JsonString.Remove(JsonString.LastIndexOf(']'), 1);*/
            gotinit = true;
            JsonString = JsonString.Replace(":null", ":0");
            start = JsonString.IndexOf('[') + 1;
            length = JsonString.LastIndexOf(']') - start + 1;
            JsonString = JsonString.Substring(start, length);
            init Initial = json.JsonDeserialize<init>(JsonString);
            Balance = double.Parse(Initial.balance);
            Bankroll = Double.Parse(Initial.bankroll);
            Bets = Initial.bets;
            Chance = Initial.chance;
            csrf = Initial.csrf;
            Edge = Initial.edge;
            Fee = Initial.fee;
            Ignores = Initial.ignores;
            Investment = (decimal)Initial.investment;
            Invest_pft = Initial.invest_pft;
            long loss = 0;
            long.TryParse(Initial.lossess, out loss);
            Losses = loss;
            Luck = Initial.luck;
            MaxProfit = Initial.max_profit;
            Name = Initial.name;
            Nonce = long.Parse(Initial.nonce);
            Percent = (decimal)Initial.percent;
            seed = Initial.seed;
            this.Settings = Initial.settings;
            shash = Initial.shash;
            this.Stats = Initial.stats;
            WDAddress = Initial.wdaddr;
            uid = Initial.uid;
            for (int i = 0; i < Initial.chat.Count; i += 2)
            {
                Chat tmpChat = json.JsonDeserialize<initchat>(Initial.chat[i].ToString()).ConvertToChat(Initial.chat[i + 1].ToString());
                if (OnOldChat != null && !logginging)
                {
                    OnOldChat(tmpChat);
                }
            }
            //logginging = false;
        }

        private Various ProcessVarious(string JsonString)
        {

            int length = JsonString.IndexOf("}");
            JsonString = JsonString.Remove(length + 1);
            JsonString = JsonString.Replace(((char)011).ToString(), "");
            Various tmp = json.JsonDeserialize<Various>(JsonString);
            return tmp;

        }

        //processing socket.on results from getinfo
        #endregion
        

        #region Emits

        public void Login(string Username, string Password, string GACode)
        {
            Thread tLogin = new Thread(new ParameterizedThreadStart(Emit));
            tLogin.Start("5:::{\"name\":\"login\",\"args\":[\"" + csrf + "\",\"" + Username + "\",\"" +Password + "\",\"" + GACode + "\"]}");
        }

        public void Bet(double Chance, double Bet, bool Hi)
        {
            Thread tbet = new Thread(new ParameterizedThreadStart(Emit));
            tbet.Start("5:::{\"name\":\"bet\",\"args\":[\"" + csrf + "\",{\"chance\":\"" + Chance.ToString("0.00000000") + "\",\"bet\":\"" + Bet.ToString("0.00000000") + "\",\"which\":\"" + ((Hi) ? "hi" : "lo") + "\"}]}");
        }

        public void  Withdraw(string Address, double Amount, string Code)
        {
            Thread tWithdraw = new Thread(new ParameterizedThreadStart(Emit));
            tWithdraw.Start(string.Format("5:::{{\"name\":\"withdraw\",\"args\":[\"{0}\",\"{1}\",\"{2}\",\"{3}\"]}}", csrf, Address, Amount, Code));
        }

        public void Randomize()
        {
            Thread tRandom = new Thread(new ParameterizedThreadStart(Emit));
            tRandom.Start(string.Format("5:::{{\"name\":\"random\",\"args\":[\"{0}\"]}}", csrf));
        }

        public void SetName(string NickName)
        {
            Thread tName = new Thread(new ParameterizedThreadStart(Emit));
            tName.Start(string.Format("5:::{{\"name\":\"name\",\"args\":[\"{0}\",\"{1}\"]}}", csrf, NickName));
        }

        public void Invest(double Amount, double Code)
        {
            Thread tInvest = new Thread(new ParameterizedThreadStart(Emit));
            tInvest.Start(string.Format("5:::{{\"name\":\"invest\",\"args\":[\"{0}\",\"{1}\",\"{2}\"]}}", csrf, Amount, Code));
        }

        public void Divest(double Amount, double Code)
        {
            Thread tDivest = new Thread(new ParameterizedThreadStart(Emit));
            tDivest.Start(string.Format("5:::{{\"name\":\"divest\",\"args\":[\"{0}\",\"{1}\",\"{2}\"]}}", csrf, Amount, Code));
        }

        public bool SetupGaCode(string Code)
        {
            return false;
        }

        private string RandomSeed()
        {
            string tmpseed = "";
            for (int i = 0; i < 8; i++)
            {
                tmpseed += RandomSeedGen.Next(0, 1000).ToString();
            }
            return tmpseed;
        }

        public void Seed()
        {
            Thread tSeed = new Thread(new ParameterizedThreadStart(Emit));
            string NewSeed = RandomSeed();
            string tmp = string.Format("5:::{{\"name\":\"seed\",\"args\":[\"{0}\",\"{1}\",\"false\"]}}", csrf, RandomSeed());
            tSeed.Start(tmp);
        }

        public void Seed(string Seed)
        {
            Thread tSeed = new Thread(new ParameterizedThreadStart(Emit));
            tSeed.Start(string.Format("5:::{{\"name\":\"seed\",\"args\":[\"{0}\",\"{1}\",\"true\"]}}", csrf, Seed));
        }

        public bool History(string Type)
        {
            return false;
        }

        public void ChangePassword(string CurrentPassword, string Password)
        {
            Thread tPassword = new Thread(new ParameterizedThreadStart(Emit));
            tPassword.Start(string.Format("5:::{{\"name\":\"change_password\",\"args\":[\"{0}\",\"{1}\",\"{2}\"]}}",csrf, CurrentPassword, Password));
        }

        public void SetupAccount(string Username, string Password)
        {
            Thread tAccount = new Thread(new ParameterizedThreadStart(Emit));
            tAccount.Start(string.Format("5:::{{\"name\":\"setup_account\",\"args\":[\"{0}\",\"{1}\",\"{2}\"]}}",csrf, Username, Password));
        }
        
        public void Chat(string Message)
        {
            Thread tbet = new Thread(new ParameterizedThreadStart(Emit));
            tbet.Start("5:::{\"name\":\"chat\",\"args\":[\"" + csrf + "\",\""+Message+"\"]}");
        }

        public void Roll(long Betid)
        {
            Thread tRoll = new Thread(new ParameterizedThreadStart(Emit));
            tRoll.Start(string.Format("5:::{{\"name\":\"roll\",\"args\":[\"{0}\",\"{1}\"]}}", csrf,Betid));
        }

        public void Ping()
        {
            Thread tRoll = new Thread(new ParameterizedThreadStart(Emit));
            tRoll.Start(string.Format("5:::{{\"name\":\"ping\",\"args\":[\"{0}\",\"ping\"]}}", csrf));
        }
        
        private void Emit(object Message)
        {
            while (inconnection)
            {
                Thread.Sleep(10);
            }

            inconnection = true;

            try
            {
                var hwrEmit = (HttpWebRequest)HttpWebRequest.Create(host + "/socket.io/1/xhr-polling/" + xhrval + "?t=" + CurrentDate());

                hwrEmit.CookieContainer = request.CookieContainer;
                hwrEmit.Referer = host;
                hwrEmit.Method = "POST";
                using (var writer = new StreamWriter(hwrEmit.GetRequestStream()))
                {
                    string writestring = Message as string;
                    writer.Write(writestring);
                }

                HttpWebResponse EmitResponse = (HttpWebResponse)hwrEmit.GetResponse();
                string sEmitResponse = new StreamReader(EmitResponse.GetResponseStream()).ReadToEnd();
                StartPorcessing(sEmitResponse);
               
                //return false;
            }
            catch (Exception e)
            {
                if (logging)
                    writelog(e.Message);
                //return true;
            }
            inconnection = false;
        }

        #endregion

        #region Events
        //On result, can be either own bet or a random bet
        public delegate void dOnresult(Result result, bool IsMine);
        public event dOnresult OnResult;
        
        //happens when you receive chat message
        public delegate void dOnChat(Chat chat);
        public event dOnChat OnChat;

        //Old chat messages that loads when the site connects
        public event dOnChat OnOldChat;

        //Gets triggered together with onResult, has less info about the other stuff
        public delegate void dOnBet(Bet bet, bool IsMine);
        public event dOnBet OnBet;

        //triggers on successfull invest
        public delegate void dInvest(Various InvestResult);
        public event dInvest onInvest;

        //OnVersion when site is updated, this forces a reload

        //On Welcome. When new user connects, this shows the welcome screen
        //public delegate void dWelcome();
        //public event dWelcome Welcome;

        //on Dismiss, closes the fancybox on site //migh be included
        public delegate void dDismiss();
        public event dDismiss OnDismiss;

        //on Timeout, If theres been no activity for an hour, this notifies client about timeout
        public delegate void dTimeout();
        public event dTimeout OnTimeout;
        //on Invest_Box open fancybox for investing and divesting //not to be included

        //on invest_error happens when invest failed, for whatever reason
        public delegate void dInvestError(Various InvestError);
        public event dInvestError OnInvestError;

        //on divest_error happens when divest failed, for whatever reason
        public delegate void dDivestError(Various DivestError);
        public event dDivestError OnDivestError;
        
        //various Google Auth stuff, skipping for now, will implement when i have figured out what each does

        //on wdaddr - gets the new withdraw address after it has been set
        public delegate void dWDAddress(Various WDaddress);
        public event dWDAddress OnWDAddress;

        //on balance - gets new balance after events like withdraw, invest, divest. 
        //Will be called when bet result is received as well
        public delegate void dBalance (Various Balance);
        public event dBalance OnBalance;

        //on details - no idea what this is used for, i assume its for fancybox messages etc, might or might not implement
        public delegate void dDetails(Various Details);
        public event dDetails OnDetails;

        //max profit - Happens when max profit changes due to large bets, investing or divesting
        public delegate void dMaxProfit(Various MaxProfit);
        public event dMaxProfit OnMaxProfit;

        //on shash - happens when server seed is changed with randomize, returns new server seed hash
        public delegate void dShash(Various secretHash);
        public event dShash OnSecretHash;


        //on seed - happens when client seed is successfully changed
        public delegate void dSeed(Various Seed);
        public event dSeed OnClientSeed;

        //on bad_seed - happens when client seed is NOT successfully changed
        public delegate void dBadSeed(Various Message);
        public event dBadSeed OnBadClientSeed;

        //on nonce - afaik this happens when ever a bet is made, returns the new nonce
        public delegate void dNonce(Various Nonce);
        public event dNonce OnNonce;

        //on address - called when a user requests a deposit address, returns the address,
        //i think the url to a qrcode image and a confs param, no idea what it is
        public delegate void dAddress(Various Address);
        public event dAddress OnAddress;

        //on new_client_seed - happens after a randomize, returns the old server seed, old server hash, old client 
        //seed, old nonce and new secret hash
        public delegate void dNewClientSeed(Various SeedInfo);
        public event dNewClientSeed OnNewClientSeed;

        //on jderror - important shit this, returns most errors you can get while doing stuff
        public delegate void dJDError(Various Error);
        public event dJDError OnJDError;

        //on jd message, called for messages, not sure wich messages, but messages
        public delegate void dJDMessage(Various Message);
        public event dJDMessage OnJDMessage;

        //on form_error - not really sure when this is called
        public delegate void dFormError(Various Error);
        public event dFormError OnFormError;

        //on login Error triggers when you give incorrect login details, i assume? 
        public delegate void dLoginError(Various Error);
        public event dLoginError OnLoginError;

        //on wins - is called with balance after a bet, only notifies you of how many winning bets YOU made
        //doesn't include any info on balance or profit or such stuff
        public delegate void dWins(Various Wins);
        public event dWins OnWins;

        //on lossess - same as on wins, just for lossess
        public delegate void dLossess(Various Lossess);
        public event dLossess OnLossess;

        //on pong, doesn't seem to really mean anything, only goes into console log. might of might not implement
        public delegate void dPong();
        public event dPong OnPong;

        //on History, still need to Figure out what this does....


        //on roll - gets the result of a roll requested by user
        public delegate void dRoll(Roll roll);
        public event dRoll OnRoll;
        


        #endregion

        //used for logging and debugging stuff
        //will probably get removed
        bool writing = false;
        bool logging = false;
        void writelog(string msg)
        {
            try
            {
                writing = true;
                using (StreamWriter sq = File.AppendText("jdcapilog.txt"))
                {
                    sq.WriteLine(DateTime.Now.ToString() + ">>>   " + msg + "\n");
                    //UpdateChat(DateTime.Now.ToString() + ">>>   " + msg + "\n");
                }
                
            }
            catch
            {
                //UpdateChat(DateTime.Now.ToString() + ">>>   " + msg + "\n");
                //UpdateChat(DateTime.Now.ToString() + ">>>   " + "#E# Failed to write to log file" + "\n");
            }
            writing = false;
        }

    }

    
}
