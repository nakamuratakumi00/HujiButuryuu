using Macss.Areas.Tass.Models;
using Macss.Areas.Tass.ViewModels;
using Macss.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Macss.Areas.Tass.Repositories
{
    public interface IHinmeiRepositorie
    {
        Task<IEnumerable<HinmeiInformation>> GetHinmeisAsync(HinmeiSerch hinmei);

        Task<IEnumerable<HinmeiViewModel>> GetHinmeiAsync(string hinmeiCd);

        decimal GetHincod();

        Task<IEnumerable<MTorihikisaki>> GetMTorihikisakiAsync();

        Task<IEnumerable<MKishu>> GetMKishuAsync();

        Task InsertHinmeiAsync(HinmeiViewModel hinmei, string loginUser);

        Task UpdateHinmeiAsync(HinmeiViewModel hinmei, string loginUser);

        Task DeleteHinmeiAsync(HinmeiViewModel hinmei, string loginUser);

        Task<IEnumerable<v_unsou_hinmei>> GetHinmeiExcelAsync(string hinCod);

        Task<IEnumerable<HinmeiFileInformation>> GetHinmeiExcelAsync(HinmeiSerch hinmei);
    }
}