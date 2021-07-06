using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macss.Areas.Fdass.Repositories
{
    interface IBackUpRepository
    {

        Task<bool> BackUpData(string month, string prossesingId, string menuName, string loginUser, System.Web.Mvc.ModelStateDictionary modelError);

        Task<bool> Test(string month);
    }
}
