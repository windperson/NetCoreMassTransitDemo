using System;
using System.Collections.Generic;
using System.Text;

namespace SharedContract
{
    public interface OrderAccepted
    {
        string OrderId { get; }
    }
}
