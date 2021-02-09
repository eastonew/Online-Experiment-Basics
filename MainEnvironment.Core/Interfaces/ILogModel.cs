using MainEnvironment.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Interfaces
{
    public interface ILogModel
    {
        string ApiKey { get; set; }
        string Message { get; set; }
        long AddedDateTicks { get; }
        DateTime LogDate { get; set; }
        string ParticipantId { get; set; }

        List<LogDetail> AdditionalValues { get; set; }
    }
}
