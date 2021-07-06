using Macss.Areas.Tass.Models;
using Macss.Areas.Tass.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Macss.Areas.Tass.Repositories
{
    public interface IShuukaRuisekiRepositorie
    {

        Task<IEnumerable<TUnsouShuukaJiseki2>> CheckShuukaRuisekiAsync(ShuukaRuisekiViewModel entry);

        Task UpdateShuukaRuisekiAsync(ShuukaRuisekiViewModel entry, string loginUser);

    }
}