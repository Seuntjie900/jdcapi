using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JDCAPI
{
    public class History
    {
        public DepositHistory[] deposit { get; set; }
        public WithdrawHistory[] withdraw { get; set; }
        public InvestHistory[] invest { get; set; }
        public CommissionHistory[] commission { get; set; }
        public HistoryType Type
        {
            get 
            {
                if (deposit != null)
                {
                    return HistoryType.deposit;
                }
                else if (withdraw != null )
                {
                    return HistoryType.withdraw;
                }
                else if (invest != null)
                {
                    return HistoryType.invest;
                }
                else 
                {
                    return HistoryType.commission;
                }
                
            }
        }
    }

    public class DepositHistory
    {
        public double amount { get; set; }
        public string note { get; set; }
        public long eid { get; set; }
        public string date { get; set; }
    }

    public class WithdrawHistory
    {
        public double amount { get; set; }
        public string address { get; set; }
        public string note { get; set; }
        public string txid { get; set; }
        public long eid { get; set; }
        public string date { get; set; }
    }

    public class InvestHistory
    {
        public double amount { get; set; }
        public double commission { get; set; }
        public decimal share_old { get; set; }
        public decimal share_new { get; set; }
        public decimal base_old { get; set; }
        public decimal base_new { get; set; }
        public decimal prin_old { get; set; }
        public decimal prin_new { get; set; }
        public long eid { get; set; }
        public string date { get; set; }
    }

    public class CommissionHistory
    {
        public decimal profit { get; set; }

        public decimal commission { get; set; }
        public string reason { get; set; }
        public long eid { get; set; }
        public string date { get; set; }
    }
    public enum HistoryType
    {
        deposit, withdraw, invest, commission
    }
}
