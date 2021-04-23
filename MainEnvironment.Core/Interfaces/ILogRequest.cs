using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Interfaces
{
    public interface ILogRequest
    {
        List<ILogModel> Logs { get; set; }
    }
}
