using System;
using System.Collections.Generic;
using System.Text;

namespace SharedContract
{
    public interface IOrderAccepted
    {
        string OrderId { get; }
    }
}
