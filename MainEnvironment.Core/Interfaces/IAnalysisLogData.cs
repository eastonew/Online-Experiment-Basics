using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Interfaces
{
    public interface IAnalysisLogData
    {
        DateTime Date { get; set; }
        Guid Id { get; set; }
        string Message { get; set; }
        Guid ParticipantId { get; set; }

    }
}
