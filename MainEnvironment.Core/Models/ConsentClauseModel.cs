using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Models
{
    public class ConsentClauseModel
    {
        public Guid Id { get; set; }
        public string Clause { get; set; }
        public bool Accepted { get; set; }
        public DateTime AcceptedDate { get; set; }
    }
}
