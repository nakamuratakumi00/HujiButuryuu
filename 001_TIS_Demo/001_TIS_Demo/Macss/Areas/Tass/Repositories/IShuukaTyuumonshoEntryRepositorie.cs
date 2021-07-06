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
    public interface IShuukaTyuumonshoEntryRepositorie
    {
        Task<IEnumerable<EntryInformation>> GetShuukaTyuumonshosAsync(EntrySerch Search);

        Task<IEnumerable<ShuukaTyuumonshoEntryViewModel>> GetShuukaTyuumonshoAsync(string Syukno, string Cdate);
        Task<IEnumerable<ShuukaTyuumonshoEntryMeisaiViewModel>> GetShuukaTyuumonshoMeisaiAsync(string Syukno, string Cdate);

        Task<IEnumerable<MUnsouUnsoukousu>> GetMUnsoukousuAsync();

        decimal GetSyknoAto(string SyuknoMae);

        Task<IEnumerable<ShuukaTyuumonshoEntryViewModel>> GetShuukaTyuumonshoPattern(string Sykno2, string Inscod);

        Task<IEnumerable<MTorihikisaki>> GetMTorihikisakiAsync();

        Task<IEnumerable<ShuukaTyuumonshoEntryViewModel>> GetVTodokesakiAsync();

        Task<IEnumerable<MUnsouPostalcode>> GetMUnsouPostalcodeAsync();

        Task<IEnumerable<ShuukaTyuumonshoEntryMeisaiViewModel>> GetHinmeiAsync();

        Task<IEnumerable<MKishu>> GetMKishuAsync();

        Task<IEnumerable<MUnsouUnsouhouhou>> GetMUnsouUnsouhouhouAsync();

        Task<IEnumerable<MUnsouUnsoukubun>> GetMUnsouUnsoukubunAsync();

        Task UpdateShuukaTyuumonshoAsync(string mode, ShuukaTyuumonshoEntryViewModel entry, List<ShuukaTyuumonshoEntryMeisaiViewModel> meisai, string loginUser);
    }
}