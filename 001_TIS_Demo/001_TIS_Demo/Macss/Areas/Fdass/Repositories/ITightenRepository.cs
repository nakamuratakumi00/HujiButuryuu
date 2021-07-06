using System.Collections.Generic;
using System.Threading.Tasks;
using Macss.Areas.Fdass.ViewModels;

namespace Macss.Areas.Fdass.Repositories
{
    public interface ITightenRepository
    {
        Task<TightenViewModel> GetDispData(string yyyyMm);

        Task<bool> IsMatujimeKanriStatus(string month, System.Web.Mvc.ModelStateDictionary error);

        Task<bool> ReadFile(string month, string loginUser, System.Web.Mvc.ModelStateDictionary error);

        Task<bool> SetSeikyuData(string month, string prossesingId, string menuName, string loginUser, System.Web.Mvc.ModelStateDictionary error);

        Task<bool> SetStatus(string month, string status, string loginUser, System.Web.Mvc.ModelStateDictionary error);
    }

}