using MainEnvironment.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Database
{
    public class Participant
    {
        public Guid Id { get; set; }
        public string ExternalParticipantId { get; set; }
        public Guid ExperimentId { get; set; }
        public virtual Experiment ExperimentDetails { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string ApiKey { get; set; }
        public DateTime? KeyExpirationDate { get; set; }
        public bool ConsentFormAccepted { get; set; }
        public DateTime? ConsentFormAcceptedDate { get; set; }
        public EquipmentTypeEnum EquipmentType { get; set; }
        public bool DownloadedEnvironment { get; set; }
        public Guid? DownloadToken { get; set; }
        public string UniqueCode { get; set; }
    }
}
