using System;

namespace MainEnvironment.Database
{
    public class Log
    {
        public Guid Id { get; set; }
        public string LogMessage { get; set; }
        public DateTime LogDate { get; set; }
        public string AdditionalDetails { get; set; }
        public Guid ParticipantId { get; set; }
        public virtual Participant Participant { get; set; }
    }
}
