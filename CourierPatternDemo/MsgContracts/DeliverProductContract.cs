using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUlid;

namespace CourierPatternDemo.MsgContracts
{
    public interface IDeliverProductArgument
    {
        NUlid.Ulid PurchaseTransactionId { get; }

        string CustomerId { get; }

        string ProductId { get; }

        uint ProductAmount { get; }
    }

    public class DeliverProductArgument : IDeliverProductArgument
    {
        public Ulid PurchaseTransactionId { get; set; }

        public string CustomerId { get; set; }

        public string ProductId { get; set; }

        public uint ProductAmount { get; set; }
    }

    public interface IDeliverProductLog
    {
        string CustomerId { get; }

        string ProductId { get; }

        uint ProductAmount { get; }
    }

    public class DeliverProductLog : IDeliverProductLog
    {
       public string CustomerId { get; set; }

       public string ProductId { get; set; }

       public uint ProductAmount { get; set; }
    }
}
