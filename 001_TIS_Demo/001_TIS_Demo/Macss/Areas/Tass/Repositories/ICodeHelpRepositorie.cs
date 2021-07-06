using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static Macss.Areas.Tass.ViewModels.CodeHelpViewModels;

namespace Macss.Areas.Tass.Repositories
{
    public interface ICodeHelpRepositorie
    {
        //得意先
        Task<IEnumerable<TokuisakiInformation>> GetTokuisakiAsync(TokuisakiSerch tokuisaki);
        //仕入先
        Task<IEnumerable<SiiresakiInformation>> GetSiiresakiAsync(SiiresakiSerch siiresaki);
        //届先
        Task<IEnumerable<TodokesakihelpInformation>> GetTodokesakiAsync(TodokesakihelpSerch todokesaki);
        //品名
        Task<IEnumerable<HinmeisakiInformation>> GetHinmeiAsync(HinmeisakiSerch hinmei);
        //出荷場所
        Task<IEnumerable<ShukkabashoInformation>> GetShukkabashoAsync(ShukkabashoSerch shukkabasho);
        //運送区分
        Task<IEnumerable<UnsokbnInformation>> GetUnsokbnAsync(UnsokbnSerch unsokbn);
        //運送方法
        Task<IEnumerable<UnsohohoInformation>> GetUnsohohoAsync(UnsohohoSerch unsohoho);
        //郵便番号
        Task<IEnumerable<YubinbangoInformation>> GetYubinbangoAsync(YubinbangoSerch unsokbn);
        // パターンマスタ
        Task<IEnumerable<TyuumonshoPatternInformation>> GetPatternAsync(string SyuknoMae);

    }
}