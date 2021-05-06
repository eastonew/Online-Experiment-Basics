using MainEnvironment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Models
{
    public abstract class AnalysisLogDataBase : IAnalysisLogData
    {
        public DateTime Date { get; set; }
        public Guid Id { get; set; }
        public string Message { get; set; }
        public Guid ParticipantId { get; set; }
    }
}
