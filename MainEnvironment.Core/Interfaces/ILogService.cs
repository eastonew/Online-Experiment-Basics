using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MainEnvironment.Core.Interfaces
{
    public interface ILogService
    {
        Task<bool> LogDetails(ILogModel logModel);
    }
}
