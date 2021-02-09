using MainEnvironment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Interfaces
{
    public interface ILogRepo
    {
        Task<bool> AddLog(ILogModel log);
    }
}
