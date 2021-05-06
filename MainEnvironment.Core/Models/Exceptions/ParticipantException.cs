using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Models.Exceptions
{
    /// <summary>
    /// Specialised exception for issues relating to the participant, allows for easier handling 
    /// </summary>
    public class ParticipantException : Exception
    {
        public Guid ParticipantId { get; set; }
        public ParticipantException(string message, Guid participantId): base(message)
        {
            ParticipantId = participantId;
        }
    }
}
