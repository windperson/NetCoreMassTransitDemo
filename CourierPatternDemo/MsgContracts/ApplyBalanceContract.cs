using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUlid;

namespace CourierPatternDemo.MsgContracts
{
    public interface ICheckThenWithdrawlBalanceArguemnt
    {
        NUlid.Ulid PurchaseTransactionId { get; }

        string CustomerId { get; }
        decimal CostAmount { get; }
    }

    public class CheckThenWithdrawlBalanceArgument : ICheckThenWithdrawlBalanceArguemnt
    {
        public Ulid PurchaseTransactionId { get; set; }

        public string CustomerId { get; set; }

        public decimal CostAmount { get; set; }
    }

    public interface ICheckThenWithdrawlBalanceLog
    {
        NUlid.Ulid PurchaseTransactionId { get; }

        bool IsSuccess { get; }

        string FailedReason { get; }
        
        ICheckThenWithdrawlBalanceArguemnt WithdrawlContext { get; }
    }

    public class CheckThenWithdrawlBalanceLog : ICheckThenWithdrawlBalanceLog
    {
        public Ulid PurchaseTransactionId { get; set; }

        public bool IsSuccess { get; set; }

        public string FailedReason { get; set; }

        public ICheckThenWithdrawlBalanceArguemnt WithdrawlContext { get; set; }
    }
}
