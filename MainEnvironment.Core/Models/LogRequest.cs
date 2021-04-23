using MainEnvironment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Models
{
    public class LogRequest : ILogRequest
    {
        public List<ILogModel> Logs { get; set; }
    }
}
