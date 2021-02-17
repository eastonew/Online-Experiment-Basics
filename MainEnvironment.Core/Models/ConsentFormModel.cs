using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Models
{
    public class ConsentFormModel
    {
        public string ParticipantId { get; set; }
        public List<ConsentClauseModel> Clauses { get; set; }

        public ConsentFormModel()
        {
            this.Clauses = new List<ConsentClauseModel>();
        }
    }
}
