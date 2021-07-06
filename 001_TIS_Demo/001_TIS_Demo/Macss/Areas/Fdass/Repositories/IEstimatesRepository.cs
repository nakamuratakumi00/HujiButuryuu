using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.Models;
using Macss.Areas.Fdass.ViewModels;

namespace Macss.Areas.Fdass.Repositories
{
    public interface IEstimatesRepository
    {
        Task<IEnumerable<EstimatesViewModel>> GetAllEstimatesAsync(string id);

        Task<EstimatesCKViewModel> EstimatesCKViewModel(string id);

        Task<IEnumerable<KisyuAExcelViewModel>> KisyuAExcelViewModel(string id);

        Task<IEnumerable<HinCodExcelViewModel>> HinCodExcelViewModel(string id);


    }
}