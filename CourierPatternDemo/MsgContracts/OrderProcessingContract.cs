using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUlid;

namespace CourierPatternDemo.MsgContracts
{
    public interface IOrderRequest
    {
        NUlid.Ulid OrderId { get; }

        string AccountId { get; }

        string ProductId { get; }

        uint ProductAmount { get; }

        decimal TotalCost { get; }
    }

    public class OrderRequest : IOrderRequest
    {
        public Ulid OrderId { get; set; }

        public string AccountId { get; set; }

        public string ProductId { get; set; }

        public uint ProductAmount { get; set; }

        public decimal TotalCost { get; set; }
    }

    public interface IOrderResult
    {
        Ulid OrderId { get; }

        string ShoppingId { get; }

        string ShoppingStatus { get; }
    }

    public class OrderResult : IOrderResult
    {
        public Ulid OrderId { get; set; }

        public string ShoppingId { get; set; }

        public string ShoppingStatus { get; set; }
    }
}
