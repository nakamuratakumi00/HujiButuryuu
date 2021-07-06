using Macss.Areas.Tass.Models;
using Macss.Areas.Tass.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Macss.Areas.Tass.ViewModels.ShuukaTehaiViewModels;

namespace Macss.Areas.Tass.Repositories
{
    public interface IShuukaTehaiRepositorie
    {
        Task<IEnumerable<ShuukaTehaiInformation>> GetShuukaInformationAsync(ShuukaTehaiSerch todokesaki, bool ReplicationFlg);

        Task<IEnumerable<UnsouShuukaTehaiAll>> GetShuukaAllAsync(ShuukaTehaiSerch serch);

        Task<ShuukaTehaiViewModels> GetShuukaMeisaiAsync(string Syukno, string Taisho, string Dataym);
    }

}