using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Interfaces
{
    public interface IDownloadAppService
    {
        Task<bool> CheckIfUserCanDownload(Guid experimentId, Guid participantId, Guid downloadToken);
    }
}
