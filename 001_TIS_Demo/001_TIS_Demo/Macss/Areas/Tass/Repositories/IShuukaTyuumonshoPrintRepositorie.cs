using Macss.Areas.Tass.Models;
using Macss.Areas.Tass.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Macss.Areas.Tass.Repositories
{
    public interface IShuukaTyuumonshoPrintRepositorie
    {
        Task<DispData> GetDispData(string LoginUser);
        Task<IEnumerable<ShuukaTyuumonshoPrintInformation>> GetSearchAsync(ShuukaTyuumonshoPrintSerch Tyuumonshyo, string groupcd);

        Task UPDShuukaTyuumonsho(string[] sNo, string[] sDt, string loginUser);

        Task<IEnumerable<ShuukaTyuumonshoPrintData>> ShuukaTyuumonshoPrint(string[] sNo, string[] sDt, string loginUser);

    }
}
