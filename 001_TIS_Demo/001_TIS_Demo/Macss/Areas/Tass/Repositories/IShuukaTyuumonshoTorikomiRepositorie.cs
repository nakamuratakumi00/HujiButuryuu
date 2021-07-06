using Macss.Areas.Tass.Models;
using Macss.Areas.Tass.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Macss.Areas.Tass.Repositories
{
    public interface IShuukaTyuumonshoTorikomiRepositorie
    {
        int GetTorikomiKouho(TorikomiSerch torikomi);

        Task<IEnumerable<TorikomiInformation>> DispTorikomiKouhoAsync(TorikomiSerch torikomi);

        int InsertTorikomiData(TorikomiSerch torikomi);
    }
}