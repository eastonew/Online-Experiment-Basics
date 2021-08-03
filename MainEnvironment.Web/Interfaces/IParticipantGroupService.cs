using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Interfaces
{
    public interface IParticipantGroupService
    {
        public int AssignGroupToParticipant(int totalGroups);
    }
}
