using System.Collections.Generic;
using System.Threading.Tasks;
using Macss.Models;
using Macss.ViewModels;

namespace Macss.Repositories
{
    public interface ILogRepository
    {
        Task<IEnumerable<LogViewModel>> GetLogByConditionAsync(LogViewModel logModel);

        void CreateLogHisory(TLogHistory appGroup);

    }
}