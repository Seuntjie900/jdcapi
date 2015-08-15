using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Threading;
using System.Net;
using WebSocket4Net;
//using WebSocketSharp;
using System.IO.Compression;
namespace JDCAPI
{
    public class jdInstance: IDisposable
    {
        WebProxy Proxy;
        HttpWebRequest request;
        WebSocket Client;
        string conid = "";
        string id = "";
        string csrf = "";
        string xhrval = "";
        
        bool inconnection = false;
        string host = "https://just-dice.com";
        bool active = false;
        string privatehash = "";
        Thread poll = null;
        private bool gotinit = false;
        private bool logginging = false;
        string sUsername = "";
        string sPassword = "";
        string sGAcode = "";

        /// <summary>
        /// Google Auth settings for connected account
        /// </summary>
        public GoogleAuthSettings ga { get; set; }

        /// <summary>
        /// Indicates whether jdcapi is successfully connected to just dice or not
        /// </summary>
        public bool Connected { get; private set; }

        private string hash { get { return privatehash; } set { privatehash = value; } }
        /// <summary>
        /// User Balance
        /// </summary>
        public double Balance { get; private set; }

        /// <summary>
        /// Site Bankroll
        /// </summary>
        public double Bankroll { get; private set; }

        /// <summary>
        /// Total number of bets on current account
        /// </summary>
        public long Bets { get; private set; }

        /// <summary>
        /// Default Chance when opening connection. Automatically set to the last bet placed before closing previous connection
        /// </summary>
        public double Chance { get; private set; }

        /// <summary>
        /// House edge, used to calculate winning etc
        /// </summary>
        public double Edge { get; private set; }

        /// <summary>
        /// Fee for withdrawing
        /// </summary>
        public double Fee { get; private set; }

        /// <summary>
        /// list of people currently being ignored by user
        /// </summary>
        public string[] Ignores { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Investment { get; set; }
        public decimal Invest_pft { get; private set; }

        /// <summary>
        /// total number of losed bets for connected account
        /// </summary>
        public long Losses { get; private set; }

        /// <summary>
        /// luck percentage of connected account
        /// </summary>
        public string Luck { get; private set; }

        /// <summary>
        /// Maximum profit that can be made by a single bet. Martingale bots should take this into consideration
        /// </summary>
        public double MaxProfit { get; private set; }

        /// <summary>
        /// Screen name of connected account
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// nonce of current server-client seed combination
        /// </summary>
        public long Nonce { get; private set; }

        /// <summary>
        /// invested percentage
        /// </summary>
        public decimal Percent { get; private set; }

        /// <summary>
        /// offsite invested amount
        /// </summary>
        public decimal Offsite { get; private set; }

        /// <summary>
        /// Profit from staking
        /// </summary>
        public decimal stake_profit { get; private set; }

        /// <summary>
        /// User profit
        /// </summary>
        private decimal _Profit = 0;
        public decimal Profit { get { return _Profit - Offset; } }

        public decimal Offset { get; private set; }

        /// <summary>
        /// Client seed
        /// </summary>
        public string seed { get; private set; }

        /// <summary>
        /// User specified settings concerning bet filtering, player watching and auto invest.
        /// </summary>
        public Settings Settings { get; private set; }

        /// <summary>
        /// hash of current server seed
        /// </summary>
        public string shash { get; private set; }

        /// <summary>
        /// User ID of current connected account
        /// </summary>
        public string uid { get; private set; }

        /// <summary>
        /// Login Username of current connected account. Blank if secret url is used
        /// </summary>
        public string Username { get; private set; }


        public decimal Wagered { get; private set; }

        /// <summary>
        /// Total amount of bets won by connected account
        /// </summary>
        public long Wins { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Stats Stats { get; private set; }

        /// <summary>
        /// Previously set withdraw address
        /// </summary>
        public string WDAddress { get; private set; }
        
        /// <summary>
        /// Hash used in secret url, secret url becomes http://just-dice.com/<SecretHash>
        /// </summary>
        public string SecretHash { get {return hash;}}

       
        private string sUserAgent = "";
        /// <summary>
        /// User agent shown when connecting to site. By default it is JDCAPI -. When this property is set, it is JDCAPI - <user agent>
        /// </summary>
        public string UserAgent 
        { 
            get 
            { 
                return sUserAgent; 
            } 
            set
            {
                if (string.IsNullOrEmpty(uid)) 
                    sUserAgent=value; 
            }
        }

        private Random RandomSeedGen = new Random();

        private DateTime Lastbet = DateTime.Now;
        private bool bGotLastResult = true;

        public jdInstance()
        {
            //AvailableBets = new List<JDCAPI.Bet>();
            //ChatMessages = new List<Chat>();
        }

        /// <summary>
        /// Set a proxy for the Just-Dice connection
        /// </summary>
        /// <param name="ProxyAddress">Host or IP of the proxy server</param>
        public void SetProxy(string ProxyAddress)
        {
            Proxy = new WebProxy(ProxyAddress);
        }

        /// <summary>
        /// Set a proxy for the Just-Dice connection
        /// </summary>
        /// <param name="ProxyAddress">Host or IP of the proxy server</param>
        /// <param name="Port">Port for the proxy server</param>
        public void SetProxy(string ProxyAddress, int Port)
        {
            Proxy = new WebProxy(ProxyAddress, Port);
        }

        /// <summary>
        /// Set an authenticated proxy for the Just-Dice connection
        /// </summary>
        /// <param name="ProxyAddress">Host or IP of the proxy server</param>
        /// <param name="Username">Proxy Username</param>
        /// <param name="Password">Proxy Password</param>
        public void SetProxy(string ProxyAddress, string Username, string Password)
        {
            Proxy = new WebProxy(ProxyAddress);
            Proxy.Credentials = new NetworkCredential(Username, Password);
        }
        /// <summary>
        /// Set an authenticated proxy for the Just-Dice connection
        /// </summary>
        /// <param name="ProxyAddress">Host or IP of the proxy server</param>
        /// <param name="Port">Port for the proxy server</param>
        /// <param name="Username">Proxy Username</param>
        /// <param name="Password">Proxy Password</param>
        public void SetProxy(string ProxyAddress, int Port, string Username, string Password)
        {
            Proxy = new WebProxy(ProxyAddress, Port);
            Proxy.Credentials = new NetworkCredential(Username, Password);
        }

        /// <summary>
        /// asynchronous Connect calls. Same paramaters as normal connect calls.
        /// Triggers event LoginEnd when login is done.
        /// </summary>
        /// <param name="DogeDice"></param>
        public void BeginConnect(bool DogeDice)
        {
            string Params = (DogeDice?"1":"0");
            Thread ConThread = new Thread(new ParameterizedThreadStart(beginasyncConnecy));
            ConThread.Start(Params);
        }

        public void BeginConnect(bool DogeDice, string SecretUrl)
        {
            string Params = (DogeDice ? "1" : "0")+"|"+SecretUrl;
            Thread ConThread = new Thread(new ParameterizedThreadStart(beginasyncConnecy));
            ConThread.Start(Params);
        }

        public void BeginConnect(bool DogeDice, string Username, string Password, string GoogleAuth)
        {
            string Params = (DogeDice ? "1" : "0")+"|"+Username+"|"+Password+"|"+GoogleAuth;
            Thread ConThread = new Thread(new ParameterizedThreadStart(beginasyncConnecy));
            ConThread.Start(Params);
        }

        private void beginasyncConnecy(object Params)
        {
            string[] Paramaters = (Params as string).Split('|');
            switch (Paramaters.Length)
            {
                case 1: Connect((Paramaters[0] == "1" ? true : false)); break;
                case 2: Connect((Paramaters[0] == "1" ? true : false),Paramaters[1]); break;
                case 3: Connect((Paramaters[0] == "1" ? true : false), Paramaters[1], Paramaters[2], ""); break;
                case 4: Connect((Paramaters[0] == "1" ? true : false), Paramaters[1], Paramaters[2], Paramaters[3]); break;
                default: Connected = false; if (this.LoginEnd != null) this.LoginEnd(false); break;
            }

        }

        /// <summary>
        /// Connects to Just dice or doge dice
        /// </summary>
        /// <param name="DogeDice">if set to true, will attempt to connect to doge dice</param>
        /// <returns>if connected successfully, returns true</returns>
        public bool Connect(bool DogeDice)
        {
            hash = "";
            return IntConnect(DogeDice);

        }

        
        private bool IntConnect(bool DogeDice)
        {
            xhrval = "";

            host = "https://" + ((DogeDice) ? "test.just" : "just") + "-dice.com";
            request = (HttpWebRequest)HttpWebRequest.Create(host);
            //Initial request for getting headers and cookies from site
            bool _Connected = getInitalHeaders();
            getxhrval();
            gotinit = false;
            int counter = 0;
            while (!gotinit && _Connected)
            {
                if (counter++ > 4)
                    _Connected = false;
                GetInfo();
            }
            /*if (_Connected)
            {
                active = true;
                if (poll != null && poll.IsAlive)
                {
                    active = false;
                    poll.Abort();
                }
                active = true;

                poll = new Thread(new ThreadStart(pollingLoop));
                poll.Start();
                Connected = true;
                if (this.LoginEnd != null)
                {
                    this.LoginEnd(Connected);
                }
                return Connected;

            }
            else
            {
                Connected = false;
                if (this.LoginEnd != null)
                {
                    this.LoginEnd(Connected);
                }
                Connected = false;
                return Connected;
            }*/
            if (_Connected)
            {
                List<KeyValuePair<string, string>> cookies = new List<KeyValuePair<string, string>>();
                List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>();
                request.CookieContainer.Add(new Cookie("io", xhrval, "/", "just-dice.com"));
                foreach (Cookie c in request.CookieContainer.GetCookies(new Uri("https://" +(DogeDice?"test.":"") + "just-dice.com")))
                {
                    cookies.Add(new KeyValuePair<string, string>(c.Name, c.Value));
                }
                headers.Add(new KeyValuePair<string, string>("origin", host));
                headers.Add(new KeyValuePair<string, string>("upgrade", "websocket"));
                headers.Add(new KeyValuePair<string, string>("connection", "upgrade"));
                headers.Add(new KeyValuePair<string, string>("user-agent", "JDCAPI - " + UserAgent));
                headers.Add(new KeyValuePair<string, string>("accept-language", "en-GB,en-US;q=0.8,en;q=0.6"));
                //headers.Add(new KeyValuePair<string, string>("Sec-WebSocket-Extensions", "deflate; client_max_window_bits; server_no_context_takeover"));
                Client = new WebSocket("wss://" + (DogeDice ? "test." : "") + "just-dice.com:443/socket.io/?EIO=3&transport=websocket&sid=" + xhrval, "", cookies, headers);
                Client.ReceiveBufferSize = 1024;
                Client.Opened += Client_Opened;
                Client.Error += Client_Error;
                Client.Closed += Client_Closed;
                Client.MessageReceived += Client_MessageReceived;
                Client.Open();
                while (Client.State == WebSocketState.Connecting)
                {
                    Thread.Sleep(100);
                }
                if (Client.State == WebSocketState.Open)
                {
                    Client.Send("2probe");
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
                Connected = Client.State == WebSocketState.Open;
                if (this.LoginEnd != null)
                {
                    this.LoginEnd(Connected);
                }
                return Connected;
            
        }
            else
            {
                Connected = false;
                if (this.LoginEnd != null)
                {
                    this.LoginEnd(Connected);
                }
                return Connected;
            }
        }

       /* void Client_OnOpen(object sender, EventArgs e)
        {
            Client.Send("2probe");
        }

        void Client_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Client_OnClose(object sender, CloseEventArgs e)
        {
            
        }

        void Client_OnMessage(object sender, MessageEventArgs e)
        {
            if (logging)
                writelog(e.Data);

            if (!first)
            {
                Client.Send("5");
                first = !first;

            }
            else
            {
                StartPorcessing(e.Data);
            }
        }*/
        void Client_DataReceived(object sender, DataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
        }
        bool first = false;
        void Client_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            /*string s = "";
            try
            {
                byte[] Bytes = System.Text.Encoding.UTF8.GetBytes(e.Message);
                Bytes[0] = (byte)((int)Bytes[0] + 1);
                Array.Resize<byte>(ref Bytes, Bytes.Length -2);
                /*Bytes[Bytes.Length - 4] = (byte)(0x00);
                Bytes[Bytes.Length - 3] = (byte)(0x00);
                Bytes[Bytes.Length - 2] = (byte)(0xff);
                Bytes[Bytes.Length - 1] = (byte)(0xff);
            */
                /*using (MemoryStream str = new MemoryStream(Bytes))
                {
                    //str.ReadByte(); str.ReadByte();
                    using (DeflateStream decompressionStream = new DeflateStream (str, CompressionMode.Decompress))
                    {
                        using (StreamReader sr = new StreamReader(decompressionStream))
                        {
                            s = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
            }*/
            if (logging)
                writelog(e.Message);

            if (e.Message == "3probe")
            {
                Thread.Sleep(1510);
                Client.Send("5");
                first = !first;

            }
            else
            {
                StartPorcessing(e.Message);
            }


            //throw new NotImplementedException();
        }

        

        void Client_Closed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void Client_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void Client_Opened(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
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
            return IntConnect(DogeDice);
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
            xhrval = "";


            privatehash = (!DogeDice) ? "0f3aa87b64103349a9cabcccbb312e606e9013c3eee8f364b9ee4e91ad2c67d3" : "6fba23685788919b1cfc47ba4346ab671ea1cc89c9987b894d7a21eb3a8714b8";
            host = "https://" + ((DogeDice) ? "test.just" : "just") + "-dice.com";
            request = (HttpWebRequest)HttpWebRequest.Create(host);
            sUsername = Username;
            sPassword = Password;
            bool _Connected = getInitalHeaders();
            
            string Message = string.Format("username={0}&password={1}&code={2}", Username, Password, GACode);            
            var tmprequest = (HttpWebRequest)HttpWebRequest.Create(host);
            if (Proxy != null)
                tmprequest.Proxy = Proxy;
            tmprequest.ContentType = "application/x-www-form-urlencoded";
            tmprequest.ContentLength = Message.Length;
            tmprequest.Referer = host;
            tmprequest.CookieContainer = request.CookieContainer;            
            tmprequest.Method = "POST";
            tmprequest.UserAgent = "JDCAPI - " + UserAgent;
            using (var writer = new StreamWriter(tmprequest.GetRequestStream()))
            {
                string writestring = Message as string;
                writer.Write(writestring);
            }
            HttpWebResponse EmitResponse = (HttpWebResponse)tmprequest.GetResponse();
            string sEmitResponse = new StreamReader(EmitResponse.GetResponseStream()).ReadToEnd();

            if (OnLoginError != null && sEmitResponse.StartsWith("<!DOCTYPE html><html lang=\"en\"><head><script src=\"/javascripts/jquery-1.10.0.min.js\">"))
            {
                OnLoginError("Incorrect Password");
                _Connected = false;
            }

            getxhrval();
            int counter = 0;
            while (!gotinit && _Connected)
            {
                if (counter++ > 4)
                    _Connected = false;
                GetInfo();
            }
            /*if (_Connected)
            {
                active = true;
                Thread poll = new Thread(new ThreadStart(pollingLoop));
                poll.Start();
                Connected = true;
                if (this.LoginEnd != null)
                {
                    this.LoginEnd(Connected);
                }
                return Connected;
            }
            else
            {
                Connected = false;
                if (this.LoginEnd != null)
                {
                    this.LoginEnd(Connected);
                }
                return Connected;
            }*/
            if (_Connected)
            {
            List<KeyValuePair<string, string>> cookies = new List<KeyValuePair<string, string>>();
                List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>();
                request.CookieContainer.Add(new Cookie("io", xhrval, "/", "just-dice.com"));
                foreach (Cookie c in request.CookieContainer.GetCookies(new Uri("https://" +(DogeDice?"test.":"") + "just-dice.com")))
                {
                    cookies.Add(new KeyValuePair<string, string>(c.Name, c.Value));
                }
                headers.Add(new KeyValuePair<string, string>("origin", host));
                headers.Add(new KeyValuePair<string, string>("upgrade", "websocket"));
                headers.Add(new KeyValuePair<string, string>("connection", "upgrade"));
                headers.Add(new KeyValuePair<string, string>("user-agent", "JDCAPI - " + UserAgent));
                headers.Add(new KeyValuePair<string, string>("accept-language", "en-GB,en-US;q=0.8,en;q=0.6"));
                //headers.Add(new KeyValuePair<string, string>("Sec-WebSocket-Extensions", "deflate; client_max_window_bits; server_no_context_takeover"));
                Client = new WebSocket("wss://" + (DogeDice ? "test." : "") + "just-dice.com:443/socket.io/?EIO=3&transport=websocket&sid=" + xhrval, "", cookies, headers);
                Client.ReceiveBufferSize = 1024;
                Client.Opened += Client_Opened;
                Client.Error += Client_Error;
                Client.Closed += Client_Closed;
                Client.MessageReceived += Client_MessageReceived;
                Client.Open();
                while (Client.State == WebSocketState.Connecting)
                {
                    Thread.Sleep(100);
                }
                if (Client.State == WebSocketState.Open)
                {
                    Client.Send("2probe");
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
                Connected = Client.State == WebSocketState.Open;
                if (this.LoginEnd != null)
                {
                    this.LoginEnd(Connected);
                }
                return Connected;

            }
            else
            {
                Connected = false;
                if (this.LoginEnd != null)
                {
                    this.LoginEnd(Connected);
                }
                return Connected;
            }
        }
        int xhrLevel = 0;
        
        /// <summary>
        /// Gets the xhr polling information
        /// </summary>
        private void getxhrval()
        {
            try
            {
                Thread.Sleep(300);
                var getxhrval = (HttpWebRequest)HttpWebRequest.Create(host + "/socket.io/?EIO=1&transport=polling&t=" + CurrentDate()+"-"+reqId++);
                if (Proxy != null)
                    getxhrval.Proxy = Proxy;
                getxhrval.Referer = host;
                getxhrval.UserAgent = "JDCAPI - " + UserAgent;
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
                        case "io": xhrval = cookievalue.Value; break;
                    }

                }
                if (string.IsNullOrEmpty(xhrval))
                {
                    Connected=false;
                }
                xhrLevel = 0;
            }
            catch
            {
                xhrLevel++;
                if (xhrLevel <3)
                {

                    getxhrval();
                }
                else
                {

                }
            }
        }

        /// <summary>
        /// gets the inital cookies for the connections.
        /// cookies includeL __cfduid, connect.sid, hash. These are required for mainting the same connection
        /// </summary>
        private bool getInitalHeaders()
        {
            
            request = (HttpWebRequest)HttpWebRequest.Create(host);
            if (Proxy != null)
                request.Proxy = Proxy;
            var cookies = new CookieContainer();
            request.CookieContainer = cookies;
            request.UserAgent = "JDCAPI - " + UserAgent;
            if (!string.IsNullOrEmpty(privatehash))
            {
                request.CookieContainer.Add(new Cookie("hash", privatehash, "/", (host.Contains("just")) ? ".just-dice.com" : ".test.just-dice.com"));
            }
            //request.CookieContainer.Add(new Cookie("cf_clearance", "bc22bf9b9733912f976dc28c78796fc91e19b7fe-1393330223-86400", "/", ".just-dice.com"));
            HttpWebResponse Response = null;
            try
            {
                
                Response = (HttpWebResponse)request.GetResponse();
                
            }
            catch (WebException e)
            {
                if (logging)
                    writelog(e.Message);

                if (e.Response != null)
                {
                    Response = (HttpWebResponse)e.Response;
                    string s1 = new StreamReader(Response.GetResponseStream()).ReadToEnd();
                    string tmp = s1.Substring(s1.IndexOf("var t,r,a,f,"));
                    string varfirst = tmp.Substring("var t,r,a,f,".Length + 1, tmp.IndexOf("=") - "var t,r,a,f,".Length - 1);
                    string varsec = tmp.Substring(tmp.IndexOf("{\"") + 2, tmp.IndexOf("\"", tmp.IndexOf("{\"") + 3) - tmp.IndexOf("{\"") - 2);
                    string var = varfirst + "." + varsec;
                    string varline = "var " + tmp.Substring("var t,r,a,f,".Length + 1, tmp.IndexOf(";") - "var t,r,a,f,".Length);
                    string initbval = tmp.Substring(tmp.IndexOf(":+") + 1, tmp.IndexOf("))") + 3 - tmp.IndexOf(":+") - 1);
                    string vallist = tmp.Substring(tmp.IndexOf(var), tmp.IndexOf("a.value") - tmp.IndexOf(var));
                    
                    string script = varline + vallist;
                    object Result = 0;
                    try
                    {
                        Result = ScriptEngine.Eval("jscript", script);
                    }
                    catch (Exception ex)
                    {

                    }
                    int aval = int.Parse(Result.ToString(), System.Globalization.CultureInfo.InvariantCulture);


                    string jschl_vc = s1.Substring(s1.IndexOf(" name=\"jschl_vc\" value=\""));
                    jschl_vc = jschl_vc.Substring(("name=\"jschl_vc\" value=\"").Length + 1);
                    int len = jschl_vc.IndexOf("\"/>\n");
                    jschl_vc = jschl_vc.Substring(0, len);
                    try
                    {
                        //string content = new WebClient().DownloadString("cdn-cgi/l/chk_jschl?jschl_vc=1bb30f6e73b41c8dd914ccbf64576147&jschl_answer=84");
                        CookieContainer cookies2 = request.CookieContainer;
                        string req = string.Format(host + "/cdn-cgi/l/chk_jschl?jschl_vc={0}&jschl_answer={1}", jschl_vc, aval + 13);
                        request = (HttpWebRequest)HttpWebRequest.Create(req);
                        if (Proxy != null)
                            request.Proxy = Proxy;
                        request.UserAgent = "JDCAPI - " + UserAgent;
                        request.CookieContainer = cookies2;
                        Response = (HttpWebResponse)request.GetResponse(); 
                        
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    Connected = false;
                    return false;
                }
            }
            

            string s = new StreamReader(Response.GetResponseStream()).ReadToEnd();
            if (s.StartsWith("<!DOCTYPE html><html lang=\"en\"><head><script src=\"/javascripts/jquery-1.10.0.min.js\">"))
            {
                if (OnLoginError != null && sUsername == "" && sPassword == "")
                {
                    OnLoginError("This account requires a username and password");
                    Connected = false;
                    return false;
                }
            }
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
                            request.CookieContainer.Add(new Cookie("hash", HashValue,"/",host.Substring("https://".Length)));
                            privatehash = HashValue;
                            founhash = true;
                            break;
                        }
                    }
                }
            }
            return true;
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
                OnJDError(var.args[0].ToString());
            }
            Disconnect();
            if (!string.IsNullOrEmpty(sUsername) & !string.IsNullOrEmpty(sPassword))
            {
                Connect((host.Contains("test.just") ? true : false), sUsername, sPassword, sGAcode);
            }
            else
            {
                Connect((host.Contains("test.just") ? true : false),privatehash);
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
            if (Proxy != null)
                request.Proxy = Proxy;
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
            TimeSpan dt = DateTime.UtcNow - DateTime.Parse("1970/01/01 00:00:00", System.Globalization.CultureInfo.InvariantCulture);
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
            DateTime tmpDate = DateTime.Parse("1970/01/01 00:00:00", System.Globalization.CultureInfo.InvariantCulture);
            tmpDate = tmpDate.AddMilliseconds(long.Parse(milliseconds));
            tmpDate += (DateTime.Now - DateTime.UtcNow);
            return tmpDate;   
        }

        public static DateTime ToDateTime2(string milliseconds)
        {
            DateTime tmpDate = DateTime.Parse("1970/01/01 00:00:00", System.Globalization.CultureInfo.InvariantCulture);
            tmpDate = tmpDate.AddSeconds(long.Parse(milliseconds));
            tmpDate += (DateTime.Now - DateTime.UtcNow);
            return tmpDate;
        }

        private void pollingLoop()
        {
            while (active) 
            {
                for (int i = 0; i < request.CookieContainer.GetCookies(request.RequestUri).Count; i++)
                {
                    if (request.CookieContainer.GetCookies(request.RequestUri)[i].Name == "cf_clearance" && request.CookieContainer.GetCookies(request.RequestUri)[i].Expired)
                    {
                        Thread trecon = new Thread(new ThreadStart(Reconnect));
                        trecon.Start();
                    }
                }
                bool poll = true;
                if (Client != null)
                {
                    if (Client.State == WebSocketState.Open)
                    {
                        if ((DateTime.Now - LastHeartbeat).TotalSeconds >= 30)
                        {
                            LastHeartbeat = DateTime.Now;
                            Client.Send("2");
                        }
                    }
                    poll = Client.State != WebSocketState.Open;
                }
                else
                {
                    if (!inconnection)
                    {
                        inconnection = true;
                        GetInfo();
                        inconnection = false;

                    }
                }
                /*if (!bGotLastResult && (DateTime.Now - Lastbet).TotalSeconds >= 7)
                {
                    Repeat();
                }*/

                Thread.Sleep(5);
                
            }
            
        }
        int reqId = 0;
        int dcount = 0;
        DateTime LastHeartbeat = DateTime.Now;
        private void GetInfo()
        {
            var MaintainConnectoin = (HttpWebRequest)HttpWebRequest.Create(host+"/socket.io/?EIO=3&transport=polling&t="+ CurrentDate()+"-"+ (reqId++) +"&sid="+xhrval);
            if (Proxy != null)
                MaintainConnectoin.Proxy = Proxy;
            MaintainConnectoin.UserAgent = "JDCAPI - " + UserAgent;
            MaintainConnectoin.CookieContainer = request.CookieContainer;
            MaintainConnectoin.Referer = host;
            MaintainConnectoin.Timeout = 10000;
            try
            {
                if ((DateTime.Now - LastHeartbeat).TotalSeconds >= 30)
                {
                    LastHeartbeat = DateTime.Now;
                    MaintainConnectoin.Method = "POST";
                    MaintainConnectoin.ContentLength = 3;
                    using (var writer = new StreamWriter(MaintainConnectoin.GetRequestStream()))
                    {
                        string writestring = "1:2";
                        writer.Write(writestring);
                    }
                }
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
                        if (++dcount > 2)
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
                    writelog("caught!" + s2);
                //throw e;
                //active = false;
            }
        }

        private void StartPorcessing(object s2)
        {
            if (s2 is string)
            StartPorcessing(s2 as string);
        }
        private void StartPorcessing(string s2)
        {
            
            if (s2.Length > 5)
            {
                List<string> returns = new List<string>();
                /*if (s2[0] == '�')
                {
                    while (s2.Length > 0)
                    {
                        if (s2[0] == '�')
                            
                        {
                            int length = int.Parse(s2.Substring(1, s2.IndexOf('�', 1) - 1));
                            string tmp = s2.Substring(s2.IndexOf('�', 1) + 1, length);
                            returns.Add(tmp.Substring(4));
                            s2 = s2.Substring(s2.IndexOf('�', 1) + length + 1);
                        }
                    }
                }
                else if (s2.Substring(0, 4) == "5:::")
                {
                    returns.Add(s2.Substring(4));
                }
                else
                {
                    
                }*/
                returns = s2.Split(new string[] { "�42" }, 100, StringSplitOptions.RemoveEmptyEntries).ToList<string>();


                foreach (string s1 in returns)
                {

//                    ReceivedObject t = json.JsonDeserialize<ReceivedObject>(s);
                    if (s1.Length > 13)
                    {
                        string s = s1;
                        string tmpstring ="";
                        if (s1.IndexOf("\0") != -1)
                            s = s.Substring(0, s1.IndexOf("\0"));
                        if (!s.StartsWith("42"))
                        {
                            tmpstring= s.Substring(2, s.IndexOf("\"", 2) - 2);
                        }
                        else
                        {
                            tmpstring = s.Substring(4, s.IndexOf("\"", 4) - 4);
                        }
                        s = s.Substring(s.IndexOf(",") + 1);
                        s = s.Substring(0, s.Length - 1);
                        if (tmpstring.Contains("pong"))
                        {
                            if (OnPong != null)
                            {
                                OnPong();
                            }
                        }
                        if (tmpstring.Contains("result") && !tmpstring.Contains("old_results"))
                        {
                            //Result tmp =  t.args[0] as Result;
                            ProcessResult(s);
                        }
                        else if (tmpstring.Contains("init"))
                        {
                            ProcessInit(s);
                        }
                        else if (tmpstring.Contains("history"))
                        {
                            ProcessHistory(s);
                        }
                        else if (tmpstring.Contains("chat"))
                        {
                            ProcessChat(s);
                        }
                        else if (tmpstring.Contains("set_hash"))
                        {
                            ProcessSetHash(s);
                        }
                        else if (tmpstring.Contains("old_results"))
                        {
                            ProcessOldResults(s);
                        }
                        else if (tmpstring.Contains("timeout"))
                        {
                            if (OnTimeout != null && !logginging)
                                OnTimeout();
                        }
                        else if (tmpstring.Contains("dismiss") && !logginging)
                        {
                            if (OnDismiss != null)
                                OnDismiss();
                        }
                        else if (tmpstring.Contains("details") && !logginging)
                        {
                            ProcessDetails(s);
                        }
                        else if (tmpstring.Contains("setup_ga") && !logginging)
                        {
                            ProcessSetupGa(s);
                        }
                        else if (tmpstring.Contains("ga_info") && !logginging)
                        {
                            ProcessGaInfo(s);
                        }
                        else if (tmpstring.Contains("ga_code_ok") && !logginging)
                        {
                            ProcessGaCode(s);
                        }
                        else if (tmpstring.Contains("ga_done") && !logginging)
                        {
                            if (onGaDone != null)
                                onGaDone();
                        }
                        else if (tmpstring.Contains("ga_code_done") && !logginging)
                        {
                            if (onGaDone != null)
                                onGaDone();
                        }
                        else if (tmpstring.Contains("staked") && !logginging)
                        {
                            ProcessStake(s);
                        }



                        else //for everything that uses the various class as output
                        {
                            try
                            {
                                Various tmp = ProcessVarious(s);
                                tmp.name = tmpstring;
                                switch (tmp.name)
                                {
                                    case "invest": Invest tmp2 = new Invest
                                    {
                                        Amount = decimal.Parse(tmp.args[0].ToString(), System.Globalization.CultureInfo.InvariantCulture),
                                        Percentage = decimal.Parse(tmp.args[1].ToString(), System.Globalization.CultureInfo.InvariantCulture),
                                        Profit = decimal.Parse(tmp.args[2].ToString(), System.Globalization.CultureInfo.InvariantCulture),
                                        Offsite = decimal.Parse(tmp.args[3].ToString(), System.Globalization.CultureInfo.InvariantCulture),

                                    }; this.Investment = tmp2.Amount; this.Percent = tmp2.Percentage; this.Invest_pft = tmp2.Profit; this.Offsite = tmp2.Offsite; if (onInvest != null && !logginging) onInvest(tmp2); break;
                                    case "invest_error": if (OnInvestError != null && !logginging) OnInvestError(tmp.args[0].ToString()); break;
                                    case "divest_error": if (OnDivestError != null && !logginging) OnDivestError(tmp.args[0].ToString()); break;
                                    case "offset": this.Offset = decimal.Parse(tmp.args[0].ToString(), System.Globalization.CultureInfo.InvariantCulture); if (this.OnOffset != null) OnOffset(this.Offset); break;
                                    case "wdaddr": if (OnWDAddress != null && !logginging) OnWDAddress(tmp.args[0].ToString()); break;
                                    case "balance": this.Balance = double.Parse(tmp.args[0].ToString(), System.Globalization.CultureInfo.InvariantCulture); if (OnBalance != null && !logginging) OnBalance(decimal.Parse(tmp.args[0].ToString(), System.Globalization.CultureInfo.InvariantCulture)); break;
                                    case "shash": this.shash = tmp.args[0].ToString(); if (OnSecretHash != null && !logginging) OnSecretHash(tmp.args[0].ToString()); break;
                                    case "seed": this.seed = tmp.args[0].ToString(); if (OnClientSeed != null && !logginging) OnClientSeed(tmp.args[0].ToString()); break;
                                    case "bad_seed": if (OnBadClientSeed != null && !logginging) OnBadClientSeed(tmp.args[0].ToString()); break;
                                    case "nonce": this.Nonce = int.Parse(tmp.args[0].ToString()); if (OnNonce != null && !logginging) OnNonce(int.Parse(tmp.args[0].ToString())); break;
                                    case "jderror": if (OnJDError != null && !logginging) OnJDError(tmp.args[0].ToString()); break;
                                    case "jdmsg": if (OnJDMessage != null && !logginging) OnJDMessage(tmp.args[0].ToString()); break;
                                    case "form_error": if (OnFormError != null && !logginging) OnFormError(tmp.args[0].ToString()); break;
                                    case "login_error": if (OnLoginError != null && !logginging) OnLoginError(tmp.args[0].ToString()); break;
                                    case "wins": this.Wins = long.Parse(tmp.args[0].ToString()); if (OnWins != null && !logginging) OnWins(long.Parse(tmp.args[0].ToString())); break;
                                    case "losses": this.Losses = long.Parse(tmp.args[0].ToString()); if (OnLossess != null && !logginging) OnLossess(long.Parse(tmp.args[0].ToString())); break;
                                    //case "details": if (OnDetails != null && !logginging) OnDetails(tmp); break;
                                    case "max_profit": this.MaxProfit = double.Parse(tmp.args[0].ToString(), System.Globalization.CultureInfo.InvariantCulture); if (OnMaxProfit != null && !logginging) OnMaxProfit(decimal.Parse(tmp.args[0].ToString(), System.Globalization.CultureInfo.InvariantCulture)); break;
                                    case "new_client_seed": this.shash = tmp.args[0].ToString(); if (OnNewClientSeed != null && !logginging) OnNewClientSeed(
                                        new SeedInfo
                                        {
                                            OldServerSeed = tmp.args[0].ToString(),
                                            OldServerHash = tmp.args[1].ToString(),
                                            OldClientSeed = tmp.args[2].ToString(),
                                            TotalRolls = tmp.args[3].ToString(),
                                            NewServerHash = tmp.args[4].ToString()
                                        }
                                        ); break;
                                    case "address": if (OnAddress != null && !logginging) OnAddress(new Address
                                    {
                                        DepositAddress = tmp.args[0].ToString(),
                                        ImageHTML = tmp.args[0].ToString(),
                                        Note = tmp.args[0].ToString(),
                                    }
                                    ); break;
                                    case "pong": if (OnPong != null && !logginging) OnPong(); break;
                                    //case "reload": Reconnect(); break;
                                }
                            }
                            catch (Exception E)
                            {
                                if (logging)
                                    writelog("Caught! " + E.Message);
                                //throw E;
                            }
                        }
                    }
                }
            }
        }

        private void ProcessHistory(string JsonString)
        {
            object[] obs = json.JsonDeserialize<object[]>("["+ JsonString +"]");
            string s = JsonString.Substring(JsonString.IndexOf("["));
            

            History tmp = new History ();
            switch ( obs[0] as string )
            {
                case "withdraw": tmp.withdraw = json.JsonDeserialize<WithdrawHistory[]>(s); break;
                case  "deposit":tmp.deposit = json.JsonDeserialize<DepositHistory[]>(s);break;
                case  "invest":tmp.invest = json.JsonDeserialize<InvestHistory[]>(s);break;
                case "commission": tmp.commission = json.JsonDeserialize<CommissionHistory[]>(s); break;
            }
            if (OnHistory != null)
                OnHistory(tmp);
        }

        private void ProcessGaCode(string JsonString)
        {
            
            GoogleAuthSettings tmp = json.JsonDeserialize<GoogleAuthSettings>(JsonString);
            if (OnGaCodeOk != null)
                OnGaCodeOk(tmp);
            
        }

        private void ProcessSetupGa(string JsonString)
        {
            
            SetupGa tmp = json.JsonDeserialize<SetupGa>(JsonString);
            if (OnSetupGa != null)
                OnSetupGa(tmp);
        }

        private void ProcessGaInfo(string JsonString)
        {
           
            GoogleAuthSettings tmp = json.JsonDeserialize<GoogleAuthSettings>(JsonString);
            if (OnGaInfo != null)
                OnGaInfo(tmp);
        }

        private void ProcessDetails(string tmpstring)
        {
            string s3 = tmpstring.Replace(" ", "");
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
            multiplier = multiplier.Substring(multiplier.IndexOf(">") + 1).Replace("x", "");
            tmp.multiplier = decimal.Parse(multiplier, System.Globalization.CultureInfo.InvariantCulture);
            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
            string stake = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
            stake = stake.Substring(stake.IndexOf(">") + 1).ToLower().Replace("test.just", "").Replace("btc", "").Replace(" ", "");
            tmp.stake = decimal.Parse(stake, System.Globalization.CultureInfo.InvariantCulture);
            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
            string profit = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
            profit = profit.Substring(profit.IndexOf(">") + 1).ToLower().Replace("test.just", "").Replace("btc", "").Replace(" ", "");
            tmp.profit = decimal.Parse(profit, System.Globalization.CultureInfo.InvariantCulture);
            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
            string chance = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
            chance = chance.Substring(chance.IndexOf(">") + 1).ToLower().Replace("%", "").Replace(" ", "");
            tmp.chance = decimal.Parse(chance, System.Globalization.CultureInfo.InvariantCulture);
            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
            if (s3.Contains("&gt"))
                tmp.high = true;
            else
                tmp.high = false;
            //s3 = s3.Substring(s3.IndexOf("</span>") + 7);
            string target = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
            target = target.Substring(target.IndexOf(">") + 1).ToLower().Replace("%", "").Replace(" ", "");
            tmp.target = decimal.Parse(target, System.Globalization.CultureInfo.InvariantCulture);
            s3 = s3.Substring(s3.IndexOf("</span>") + 7);
            string lucky = s3.Substring(s3.IndexOf("<span>"), s3.IndexOf("</span>") - s3.IndexOf("<span>"));
            lucky = lucky.Substring(lucky.IndexOf(">") + 1).ToLower().Replace("%", "").Replace(" ", "");
            tmp.lucky = decimal.Parse(lucky, System.Globalization.CultureInfo.InvariantCulture);
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

        #region processing socket.on results from getinfo
        private void ProcessResult(object JsonString)
        {
            
            //ProcessResult(JsonString as string);
        }

        private void ProcessResult(string JsonString)
        {
            
            Result tmp = json.JsonDeserialize<Result>(JsonString);
            if (tmp.bankroll != null)
            {
                
                Bankroll = double.Parse(tmp.bankroll, System.Globalization.CultureInfo.InvariantCulture);
                MaxProfit = double.Parse(tmp.max_profit, System.Globalization.CultureInfo.InvariantCulture);
                Investment = tmp.investment;
                Invest_pft = (decimal)tmp.invest_pft;
                Percent = tmp.percent;
                Stats = tmp.stats;      
                if (tmp.uid == uid)
                {
                    this.Nonce = tmp.nonce;
                    bGotLastResult = true;
                }
                if (tmp.balance == null)
                {
                    tmp.balance = Balance.ToString();
                }
                else
                {
                    Balance = double.Parse(tmp.balance, System.Globalization.CultureInfo.InvariantCulture);
                }
                if (tmp.profit == null)
                {
                    tmp.profit = Profit.ToString();
                }
                else
                {
                    _Profit = decimal.Parse(tmp.profit, System.Globalization.CultureInfo.InvariantCulture);
                }
                
                

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
                if (OnResult != null)
                    OnResult(tmp, (tmp.uid == uid));
                if (OnBet != null && !logginging)
                    OnBet(tmp2, (tmp2.uid == uid) );
                

            }
        }

        private void ProcessStake(string JsonString)
        {

            Stake tmp = json.JsonDeserialize<Stake>(JsonString);
            this.Investment = tmp.investment;
            this.Invest_pft = tmp.invest_pft;
            this.stake_profit = tmp.stake_pft;
            if (OnStake != null)
                OnStake(tmp);
        }

        private void ProcessChat(string JsonString)
        {
            
             string[] tmps = json.JsonDeserialize<string[]>("["+JsonString+"]");
            baseChat tmp = new baseChat{name="chat", args = tmps};
             if (!tmp.args[0].StartsWith("INFO:"))
             {
                 if (OnChat != null && !logginging)
                     OnChat(tmp.ConvertToChat());
             }
             else if (tmp.args.Length == 1)
             {
                 if (tmp.args[0].StartsWith("INFO: you received a"))
                 {
                     ProcessTip(tmp.args[0]);
                 }
                 if (tmp.args[0].StartsWith("INFO: /tip auth="))
                 {
                     ProcessTipConfirmation(tmp.args[0]);
                 }
                 if (OnChatInfo != null && !logginging)
                     OnChatInfo(tmp.args[0]);
             }
        }
        void ProcessTipConfirmation(string Message)
        {
            string works = Message.Substring(Message.IndexOf("=") + 1);
            string message = works.Substring(works.IndexOf("--") + 3);
            string[] args = works.Split(' ');
            if (OnConfirmTip!=null)
            {
                OnConfirmTip(args[0], args[1], args[2], message);
            }
            
        }

        private void ProcessTip(string Message)
        {
            ReceivedTip tmp = new ReceivedTip();
            Message = Message.Replace("INFO: you received a ", "");
            tmp.Amount = double.Parse(Message.Substring(0, Message.IndexOf("CLAM") - 1), System.Globalization.CultureInfo.InvariantCulture);
            Message = Message.Substring(tmp.Amount.ToString().Length+ " CLAM tip from (".Length);
            tmp.FromID = int.Parse(Message.Substring(0, Message.IndexOf(")")));
            tmp.FromUser = Message.Substring(Message.IndexOf(" ") + 1);
            if (OnTipReceived != null)
            {
                OnTipReceived(tmp);
            }
        }

        private void ProcessSetHash(string JsonString)
        {

            string tmp = json.JsonDeserialize<string>(JsonString);
            bool found = false;
            
            foreach (Cookie c in request.CookieContainer.GetCookies(new Uri(host)))
            {
                if (c.Name == "hash")
                {
                    c.Value = tmp;
                    found = true;
                }
                
            }
            if (!found)
            {
                request.CookieContainer.Add(new Cookie("hash", tmp, "/", host.Substring("https://".Length)));

            }
            privatehash = tmp;
            gotinit = false;
            getxhrval();
            found = false;
            foreach (Cookie c in request.CookieContainer.GetCookies(new Uri(host)))
            {
                if (c.Name == "io")
                {
                    c.Value = xhrval;
                    found = true;
                }

            }
            if (!found)
            {
                request.CookieContainer.Add(new Cookie("io", xhrval, "/", host.Substring("https://".Length)));

            }
            Client.Close();
            while (!gotinit)
            {
                GetInfo();
            }
                        
            
                List<KeyValuePair<string, string>> cookies = new List<KeyValuePair<string, string>>();
                List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>();
                request.CookieContainer.Add(new Cookie("io", xhrval, "/", "just-dice.com"));
                foreach (Cookie c in request.CookieContainer.GetCookies(new Uri(host)))
                {
                    cookies.Add(new KeyValuePair<string, string>(c.Name, c.Value));
                }
                headers.Add(new KeyValuePair<string, string>("origin", host));
                headers.Add(new KeyValuePair<string, string>("upgrade", "websocket"));
                headers.Add(new KeyValuePair<string, string>("connection", "upgrade"));
                headers.Add(new KeyValuePair<string, string>("user-agent", "JDCAPI - " + UserAgent));
                headers.Add(new KeyValuePair<string, string>("accept-language", "en-GB,en-US;q=0.8,en;q=0.6"));
                //headers.Add(new KeyValuePair<string, string>("Sec-WebSocket-Extensions", "deflate; client_max_window_bits; server_no_context_takeover"));
                Client = new WebSocket("wss://" + (host.Contains("test") ? "test." : "") + "just-dice.com:443/socket.io/?EIO=3&transport=websocket&sid=" + xhrval, "", cookies, headers);
                Client.ReceiveBufferSize = 1024;
                Client.Opened += Client_Opened;
                Client.Error += Client_Error;
                Client.Closed += Client_Closed;
                Client.MessageReceived += Client_MessageReceived;
                Client.Open();
                while (Client.State == WebSocketState.Connecting)
                {
                    Thread.Sleep(100);
                }
                if (Client.State == WebSocketState.Open)
                {
                    Client.Send("2probe");
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
                Connected = Client.State == WebSocketState.Open;
                if (this.LoginEnd != null)
                {
                    this.LoginEnd(Connected);
                }
                
            
            
        }

        
        private void ProcessOldResults(string JsonString)
        {
            gotinit = true;
            
            try
            {
                oldbets tmp = new oldbets { name = "oldbets", args = json.JsonDeserialize<Bet[]>(JsonString)};
                for (int i = 0; i < tmp.args.Length; i++)
                {
                    if (OnOldBets != null && !logginging)
                    {
                        OnOldBets(tmp.args[i], (tmp.args[i].uid == uid));
                    }
                    if (OnBet != null && !logginging)
                    {
                        OnBet(tmp.args[i], (tmp.args[i].uid == uid));
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
            
            
            gotinit = true;
            init Initial = json.JsonDeserialize<init>(JsonString);
            Balance = double.Parse(Initial.balance, System.Globalization.CultureInfo.InvariantCulture);
            Bankroll = double.Parse(Initial.bankroll, System.Globalization.CultureInfo.InvariantCulture);
            Bets = Initial.bets;
            Chance = Initial.chance;
            csrf = Initial.csrf;
            Edge = Initial.edge;
            Fee = Initial.fee;
            Ignores = Initial.ignores;
            Investment = (decimal)Initial.investment;
            Invest_pft = (decimal)Initial.invest_pft;
            this.Offsite = Initial.offsite;
            this.Offset = Initial.offset;
            this.stake_profit = Initial.stake_pft;
            Losses = long.Parse(Initial.losses, System.Globalization.CultureInfo.InvariantCulture);
            
            Wins  = long.Parse(Initial.wins, System.Globalization.CultureInfo.InvariantCulture);
                        
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
            _Profit = decimal.Parse(Initial.profit, System.Globalization.CultureInfo.InvariantCulture);
            Wagered = decimal.Parse(Initial.wagered, System.Globalization.CultureInfo.InvariantCulture);

            this.ga = Initial.ga;
            if (Initial.chat != null)
            for (int i = 0; i < Initial.chat.Count-1; i += 1)
            {
                try
                {
                    Chat tmpChat = json.JsonDeserialize<initchat>(Initial.chat[i].ToString()).ConvertToChat(Initial.chat[i + 1].ToString());
                    if (OnOldChat != null && !logginging)
                    {
                        OnOldChat(tmpChat);
                    }
                }
                catch
                {

                }
                
            }
            if (OnInit != null)
                OnInit(Initial);
            //logginging = false;
        }

        private Various ProcessVarious(string JsonString)
        {
            /*try
            {
                int length = JsonString.IndexOf("}");
                JsonString = JsonString.Remove(length + 1);
                
            }
            catch
            {

            }*/
            
            System.Collections.ArrayList tmp = json.JsonDeserialize<System.Collections.ArrayList>("["+JsonString+"]");
                         
            return new Various { args = tmp};

        }

        //processing socket.on results from getinfo
        #endregion
          


        #region Emits
        public void ConfirmTip(string auth, string id, string amount)
        {
            Thread tDeposit = new Thread(new ParameterizedThreadStart(Emit));
            string tmp = "[\"chat\",\"" + csrf + "\",\"/tip auth=" + auth + " " + id + " " + amount + "\"]";
            tDeposit.Start(tmp);
        }

        public void Repeat()
        {
            Lastbet = DateTime.Now;
            Thread tDeposit = new Thread(new ParameterizedThreadStart(Emit));
            tDeposit.Start("[\"repeat\",\"" + csrf + "\"]");
        }

        public void ResetProfit()
        {
            Thread tDeposit = new Thread(new ParameterizedThreadStart(Emit));
            tDeposit.Start("42[\"reset_profit\",\"" + csrf + "\"]");
        }

        public void Deposit()
        {
            Thread tDeposit = new Thread(new ParameterizedThreadStart(Emit));
            tDeposit.Start("42[\"deposit\",\"" + csrf + "\"]");
        }

        public void Login(string Username, string Password, string GACode)
        {
            Thread tLogin = new Thread(new ParameterizedThreadStart(Emit));
            tLogin.Start("42[\"login\",\"" + csrf + "\",\"" + Username + "\",\"" +Password + "\",\"" + GACode + "\"]");
        }

        public void Bet(double Chance, double Bet, bool Hi)
        {
            Lastbet = DateTime.Now;
            bGotLastResult = false;
            Thread tbet = new Thread(new ParameterizedThreadStart(Emit));
            tbet.Start("42[\"bet\",\"" + csrf + "\",{\"chance\":\"" + Chance.ToString("0.00000000") + "\",\"bet\":\"" + Bet.ToString("0.00000000") + "\",\"which\":\"" + ((Hi) ? "hi" : "lo") + "\"}]");
        }

        public void  Withdraw(string Address, double Amount, string Code)
        {
            Thread tWithdraw = new Thread(new ParameterizedThreadStart(Emit));
            tWithdraw.Start(string.Format("42[\"withdraw\",\"{0}\",\"{1}\",\"{2}\",\"{3}\"]", csrf, Address, Amount, Code));
        }

        public void Randomize()
        {
            Thread tRandom = new Thread(new ParameterizedThreadStart(Emit));
            tRandom.Start(string.Format("42[\"random\",\"{0}\"]", csrf));
        }

        public void SetName(string NickName)
        {
            Thread tName = new Thread(new ParameterizedThreadStart(Emit));
            tName.Start(string.Format("42[\"name\",\"{0}\",\"{1}\"]", csrf, NickName));
        }

        public void Invest(double Amount, string Code)
        {
            Thread tInvest = new Thread(new ParameterizedThreadStart(Emit));
            tInvest.Start(string.Format("42[\"invest\",\"{0}\",\"{1}\",\"{2}\"]", csrf, Amount, Code));
        }

        public void Divest(double Amount, string Code)
        {
            Thread tDivest = new Thread(new ParameterizedThreadStart(Emit));
            tDivest.Start(string.Format("42[\"divest\",\"{0}\",\"{1}\",\"{2}\"]", csrf, Amount, Code));
        }
        public void InvestAll(string Code)
        {
            Thread tInvest = new Thread(new ParameterizedThreadStart(Emit));
            tInvest.Start(string.Format("42[\"invest\",\"{0}\",\"all\",\"{1}\"]", csrf,  Code));
        }

        public void DivestAll(string Code)
        {
            Thread tDivest = new Thread(new ParameterizedThreadStart(Emit));
            tDivest.Start(string.Format("42[\"divest\",\"{0}\",\"all\",\"{1}\"]", csrf,  Code));
        }

        public void SetupAccount(string Username, string Password)
        {
            Thread tSetupAccount = new Thread(new ParameterizedThreadStart(Emit));
            tSetupAccount.Start(string.Format("42[\"setup_account\",\"{0}\",\"{1}\",\"{2}\"]", csrf, Username, Password));
        }

        public void SetupGaCode(string Code)
        {
            Thread tSetupGaCode = new Thread(new ParameterizedThreadStart(Emit));
            tSetupGaCode.Start(string.Format("42[\"setup_ga_code\",\"{0}\",\"{1}\"]", csrf, Code));
        }

        public void EditGa()
        {
            Thread tSetupGaCode = new Thread(new ParameterizedThreadStart(Emit));
            tSetupGaCode.Start(string.Format("42[\"edit_ga\",\"{0}\"]", csrf));
        }

        public void DoneEditGa(string Code, GAFlags Flags)
        {
            Thread tSetupGaCode = new Thread(new ParameterizedThreadStart(Emit));
            tSetupGaCode.Start(string.Format("42[\"done_edit_ga\",\"{0}\",\"{1}\",{2}]", csrf, Code, json.JsonSerializer<GAFlags>(Flags).Replace("_"," ")));
        }

        public void DisableGa(string Code)
        {
            Thread tSetupGaCode = new Thread(new ParameterizedThreadStart(Emit));
            tSetupGaCode.Start(string.Format("42[\"disable_ga\",\"{0}\",\"{1}\"]", csrf, Code));
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
            string tmp = string.Format("42[\"seed\",\"{0}\",\"{1}\",true]", csrf, RandomSeed());
            tSeed.Start(tmp);
        }

        public void Seed(string Seed)
        {
            Thread tSeed = new Thread(new ParameterizedThreadStart(Emit));
            tSeed.Start(string.Format("42[\"seed\",\"{0}\",\"{1}\",true]", csrf, Seed));
        }

        public void History(HistoryType Type)
        {
            Thread tSeed = new Thread(new ParameterizedThreadStart(Emit));
            tSeed.Start(string.Format("42[\"history\",\"{0}\",\"{1}\"]", csrf, Type.ToString()));
        }

        public void ChangePassword(string CurrentPassword, string Password)
        {
            Thread tPassword = new Thread(new ParameterizedThreadStart(Emit));
            tPassword.Start(string.Format("42[\"change_password\",\"{0}\",\"{1}\",\"{2}\"]",csrf, CurrentPassword, Password));
        }

        
        public void Chat(string Message)
        {
            Thread tbet = new Thread(new ParameterizedThreadStart(Emit));
            tbet.Start("42[\"chat\",\"" + csrf + "\",\"" + Message + "\"]");
        }

        public void Roll(long Betid)
        {
            Thread tRoll = new Thread(new ParameterizedThreadStart(Emit));
            tRoll.Start(string.Format("42[\"roll\",\"{0}\",\"{1}\"]", csrf,Betid));
        }

        public void Ping()
        {
            Thread tRoll = new Thread(new ParameterizedThreadStart(Emit));
            tRoll.Start(string.Format("42[\"ping\",\"{0}\",\"ping\"]", csrf));
        }

        public void SetSettings(SettingsType_Numeric Type, decimal Value)
        {
            string Message = "";
            switch (Type)
            {
                case SettingsType_Numeric.Chat_Minimum_Change: Message = "chat_min_change"; this.Settings.chat_min_change = (double)Value; break;
                case SettingsType_Numeric.Chat_Minimum_Risk: Message = "chat_min_risk"; this.Settings.chat_min_risk = (double)Value; break;

                case SettingsType_Numeric.Minimum_Change: Message = "min_change"; this.Settings.min_change = (double)Value; break;
                case SettingsType_Numeric.Minimum_Risk: Message = "min_risk"; this.Settings.min_risk = (double)Value; break;
                case SettingsType_Numeric.Roll_Delay: Message = "roll_delay"; this.Settings.roll_delay = (double)Value; break;
                case SettingsType_Numeric.Max_Double: Message = "max_double"; this.Settings.max_double = (double)Value; break;
                
            }
            Thread tSettings = new Thread(new ParameterizedThreadStart(Emit));
            tSettings.Start(string.Format("42[\"setting\",\"{0}\",\"float\",\"{1}\",\"{2}\"]", csrf, Message, Value));
        }
        public void SetSettings(SettingsType_String Type, string Value)
        {
            string Message = "";
            string type = "text";
            switch (Type)
            {

                case SettingsType_String.Chat_Watch_Player: Message = "chat_watch_player"; this.Settings.chat_watch_player = Value; break;
                case SettingsType_String.Watch_Player: Message = "watch_player"; this.Settings.watch_player = Value; break;
                case SettingsType_String.Email: Message = "emailaddr"; break;
                case SettingsType_String.Emergency_Address: Message = "btcaddr"; type = "addr"; this.Settings.btcaddr = Value; break;
            }

            Thread tSettings = new Thread(new ParameterizedThreadStart(Emit));
            tSettings.Start(string.Format("42[\"setting\",\"{0}\",\"{3}\",\"{1}\",\"{2}\"]", csrf, Message, Value, type));
        }
        public void SetSettings( SettingsType_Boolean Type, bool Value)
        {
            string Message = "";
            switch (Type)
            {
                case SettingsType_Boolean.Alarm: Message = "alarm"; this.Settings.alarm = Value; break;
                case SettingsType_Boolean.AllBetsMe: Message = "allbetsme"; this.Settings.allbetsme = Value; break;
                case SettingsType_Boolean.AutoInvest: Message = "autoinvest"; this.Settings.autoinvest = Value; break;
                case SettingsType_Boolean.Chatstake: Message = "chatstake"; this.Settings.chatstake = Value; break;
                case SettingsType_Boolean.MuteChat: Message = "mutechat"; this.Settings.mutechat = Value; break;
                case SettingsType_Boolean.Shortcuts: Message = "shortcuts"; this.Settings.shortcuts = Value; break;
            }
            Thread tSettings = new Thread(new ParameterizedThreadStart(Emit));
            tSettings.Start(string.Format("42[\"setting\",\"{0}\",\"bool\",\"{1}\",\"{2}\"]", csrf, Message, Value?"1":"0"));
        }
        int emitlevel = 0;
        private void Emit(object Message)
        {
            if (Client.State == WebSocketState.Open)
            Client.Send(Message as string);
            //inconnection = true;
            /*try
            {
                string tmpstr = Message as string;
                tmpstr = tmpstr.Length +":"+ tmpstr;
                var hwrEmit = (HttpWebRequest)HttpWebRequest.Create(host + "/socket.io/?EIO=3&transport=polling&t=" + CurrentDate()+"-"+(reqId++)+ "&sid=" + xhrval);
                if (Proxy != null)
                    hwrEmit.Proxy = Proxy;
                if (logging)
                    writelog(Message as string);
                hwrEmit.CookieContainer = request.CookieContainer;
                hwrEmit.UserAgent = "JDCAPI - " + UserAgent;
                hwrEmit.Referer = host;
                hwrEmit.Method = "POST";
                hwrEmit.ContentLength = tmpstr.Length;
                using (var writer = new StreamWriter(hwrEmit.GetRequestStream()))
                {
                    string writestring = tmpstr;
                    writer.Write(writestring);
                }
                string sEmitResponse = "";
                try
                {
                    HttpWebResponse EmitResponse = (HttpWebResponse)hwrEmit.GetResponse();
                    sEmitResponse = new StreamReader(EmitResponse.GetResponseStream()).ReadToEnd();
                }
                catch
                {
                    reqId--;
                }
                if (!string.IsNullOrEmpty(sEmitResponse))
                {
                    if (logging)
                        writelog(sEmitResponse);
                    StartPorcessing(sEmitResponse);
                    emitlevel = 0;
                }
                //return false;
            }
            catch (Exception e)
            {
                reqId--;
                if (logging)
                    writelog("Failed emit! " + e.Message);
                if (emitlevel++<5)
                    Emit(Message);
                //return true;
            }*/
            //inconnection = false;
        }

        #endregion

        #region Events
        public delegate void dTipComfirm(string auth, string id, string amount, string message);
        public event dTipComfirm OnConfirmTip;
        

        //non message chat event
        public delegate void dOnChatInfo(string Message);
        public event dOnChatInfo OnChatInfo;

        //on Receiving a Tip
        public delegate void dOnTipReceived(ReceivedTip Tip);
        public event dOnTipReceived OnTipReceived;

        //on staking clam
        public delegate void dOnStake(Stake Staked);
        public event dOnStake OnStake;

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

        /// <summary>
        /// Triggers when connected to site and old bet results are sent to client
        /// </summary>
        public event dOnBet OnOldBets;

        //triggers on successfull invest
        public delegate void dInvest(Invest InvestResult);
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
        public delegate void dInvestError(string InvestError);
        public event dInvestError OnInvestError;

        //on divest_error happens when divest failed, for whatever reason
        public delegate void dDivestError(string DivestError);
        public event dDivestError OnDivestError;
        
        //various Google Auth stuff, skipping for now, will implement when i have figured out what each does

        //on wdaddr - gets the new withdraw address after it has been set
        public delegate void dWDAddress(string WDaddress);
        public event dWDAddress OnWDAddress;

        //on balance - gets new balance after events like withdraw, invest, divest. 
        //Will be called when bet result is received as well
        public delegate void dBalance (decimal Balance);
        public event dBalance OnBalance;


        //max profit - Happens when max profit changes due to large bets, investing or divesting
        public delegate void dMaxProfit(decimal MaxProfit);
        public event dMaxProfit OnMaxProfit;

        //on shash - happens when server seed is changed with randomize, returns new server seed hash
        public delegate void dShash(string secretHash);
        public event dShash OnSecretHash;


        //on seed - happens when client seed is successfully changed
        public delegate void dSeed(string Seed);
        public event dSeed OnClientSeed;

        //on bad_seed - happens when client seed is NOT successfully changed
        public delegate void dBadSeed(string Message);
        public event dBadSeed OnBadClientSeed;

        //on nonce - afaik this happens when ever a bet is made, returns the new nonce
        public delegate void dNonce(int Nonce);
        public event dNonce OnNonce;

        //on address - called when a user requests a deposit address, returns the address,
        //i think the url to a qrcode image and a confs param, no idea what it is
        public delegate void dAddress(Address Address);
        public event dAddress OnAddress;

        //on new_client_seed - happens after a randomize, returns the old server seed, old server hash, old client 
        //seed, old nonce and new secret hash
        public delegate void dNewClientSeed(SeedInfo SeedInfo);
        public event dNewClientSeed OnNewClientSeed;

        //on jderror - important shit this, returns most errors you can get while doing stuff
        public delegate void dJDError(string Error);
        public event dJDError OnJDError;

        //on jd message, called for messages, not sure wich messages, but messages
        public delegate void dJDMessage(string Message);
        public event dJDMessage OnJDMessage;

        //on form_error - not really sure when this is called
        public delegate void dFormError(string Error);
        public event dFormError OnFormError;

        //on login Error triggers when you give incorrect login details, i assume? 
        public delegate void dLoginError(string Error);
        public event dLoginError OnLoginError;

        //on wins - is called with balance after a bet, only notifies you of how many winning bets YOU made
        //doesn't include any info on balance or profit or such stuff
        public delegate void dWins(long Wins);
        public event dWins OnWins;

        //on lossess - same as on wins, just for lossess
        public delegate void dLossess(long Lossess);
        public event dLossess OnLossess;

        //on pong, doesn't seem to really mean anything, only goes into console log. might of might not implement
        public delegate void dPong();
        public event dPong OnPong;

        //on History, triggers after a history call for deposits,withdraw,invest or commission history
        public delegate void dOnHistory(History History);
        public event dOnHistory OnHistory;
        

        //on roll - gets the result of a roll requested by user
        public delegate void dRoll(Roll roll);
        public event dRoll OnRoll;

        public delegate void dLoginEnd(bool Connected);
        public event dLoginEnd LoginEnd;

        //on ga setup
        public delegate void dSetupGa(SetupGa GaSetup);
        public event dSetupGa OnSetupGa;

        //on ga code ok
        public delegate void dGaCodeOK(GoogleAuthSettings GASettings);
        public event dGaCodeOK OnGaCodeOk;
        
        //on ga_info
        public delegate void dGaInfo(GoogleAuthSettings Setup);
        public event dGaInfo OnGaInfo;

        //on ga code ok
        public delegate void dGaDone();
        public event dGaDone onGaDone;

        //on init, after login or connecting
        public delegate void dOnInit(init Init);
        public event dOnInit OnInit;

        //on offset, triggers when a user resets profit
        public delegate void dOnOffset(decimal Offest);
        public event dOnOffset OnOffset;
        #endregion

        //used for logging and debugging stuff
        //will probably get removed
        bool writing = false;
        public bool logging = false;
        void writelog(string msg)
        {
            try
            {
                writing = true;
                using (StreamWriter sq = File.AppendText("jdcapilog.txt"))
                {
                    sq.WriteLine(DateTime.Now.ToString() + " " + msg);
                    
                }
                
            }
            catch
            {
                
            }
            writing = false;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }

    
}
