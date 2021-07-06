using Macss.Models;
using Macss.ViewModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Macss.Repositories
{
    public interface IUseStatusRepository
    {
        Task<IEnumerable<TUseStatus>> GetSelectUseStatusAsync(string SessionId, string AccountId);

        Task<bool> UpdateUseStatusAsync(string SessionId, string AccountId, string TitleName, string ActionName, string ControllerName);

        Task<bool> DeleteUseStatusAsync(string SessionId, string AccountId);

        Task<IEnumerable<UseStatusViewModel>> GetAllUseStatusAsync();
    }
}