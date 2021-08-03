using MainEnvironment.Core.Helpers;
using MainEnvironment.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Services
{
    public class RandomParticipantGroupService : IParticipantGroupService
    {
        public int AssignGroupToParticipant(int totalGroups)
        {
            //TODO - maybe need to check to ensure that groups are being evenly spread
            int groupId = 0;
            if(totalGroups > 0)
            {
                groupId = RandomHelper.GetNext(1, totalGroups);
            }
            return groupId;
        }
    }
}
