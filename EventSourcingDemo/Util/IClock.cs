using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcingDemo.Util
{
    public interface IClock
    {
        DateTimeOffset Now { get; }
        DateTimeOffset UtcNow { get; }
    }
}
