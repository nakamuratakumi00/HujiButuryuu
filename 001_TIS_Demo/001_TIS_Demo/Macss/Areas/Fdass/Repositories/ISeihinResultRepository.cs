using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.Models;
using Macss.Areas.Fdass.ViewModels;

namespace Macss.Areas.Fdass.Repositories
{
    public interface ISeihinResultRepository
    {
        Task<SeihinResultVierModel> GetDispData();

        Task<IEnumerable<TankaAutoSetList>> TankaAutoSetList(string dtfrom, string dtto);

    }
}