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
    public interface IPrintRepository
    {
        Task<PrintViewModel> GetDispData();
        Task<IEnumerable<PcCodeErrorListPrint>> PcCodeErrorListPrint();
        Task<IEnumerable<KisyuMoreList>> KisyuMoreList();
        Task<IEnumerable<NyuSyukoKurikosiList>> NyuSyukoKurikosiList();
        Task<IEnumerable<SekiyusakiShukeiList>> SekiyusakiShukeiList();
        Task<IEnumerable<SoukoKisyuAList>> SoukoKisyuAList();
        Task<IEnumerable<SoukoHinCodList>> SoukoHinCodList();
        Task<IEnumerable<SoukoSiyouryoList>> SoukoSiyouryoList();
        Task<IEnumerable<KyotenbetuList>> KyotenbetuList();
        Task<IEnumerable<THokanSeikyuKyoten>> FpsKyotenExcel();

    }
}