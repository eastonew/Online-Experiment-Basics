using MainEnvironment.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Models
{
    public class LogModel : ILogModel
    {
        public string ApiKey { get; set; }
        private DateTime _logDate;

        public string Message { get; set; }
        public long AddedDateTicks { get; private set; }
        public string ParticipantId { get; set; }
        public DateTime LogDate
        {
            get
            {
                return _logDate;
            }
            set
            {
                AddedDateTicks = value.Ticks;
                _logDate = value;
            }
        }
        public List<LogDetail> AdditionalValues { get; set; }

        public LogModel()
        {
            this.AdditionalValues = new List<LogDetail>();
        }

        public override string ToString()
        {
            //use a prefix to allow easy searching in the logs
            return $"[Aston]: {JsonConvert.SerializeObject(this, Formatting.Indented)}";
        }
    }
}
