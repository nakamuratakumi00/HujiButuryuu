using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.Models;
using Macss.Areas.Fdass.ViewModels;
using Macss.Areas.Fdass.Models;

namespace Macss.Areas.Fdass.Repositories
{
    public interface IMatujimeKanriRepository
    {

        Task<IEnumerable<THokanMatujimeKanri>> FindByMonthAsync(string month);

        Task<MatujimeKanriViewModel> GetDispData(string yyyyMm);

        Task<MatujimeKanriViewModel> GetDispData();
    }
}