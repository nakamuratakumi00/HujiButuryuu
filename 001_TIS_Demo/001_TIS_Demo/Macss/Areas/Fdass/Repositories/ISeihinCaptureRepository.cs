using Macss.Areas.Fdass.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Macss.Areas.Fdass.Repositories
{
    public interface ISeihinCaptureRepository
    {

        Task<bool> GetSeihin(SeihinCaptureViewModel model, string loginUser, System.Web.Mvc.ModelStateDictionary error);

    }
}