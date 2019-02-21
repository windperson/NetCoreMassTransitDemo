using System;

namespace AspNetCoreDemo.MsgContracts
{
    public interface ISendOrder
    {
        string OrderId { get; }
        DateTime OrderDate { get; }
        decimal OrderAmount { get; }
    }

    public class SendOrder : ISendOrder
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderAmount { get; set; }
    }
}