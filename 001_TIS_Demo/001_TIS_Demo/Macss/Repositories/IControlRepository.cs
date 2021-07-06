using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.Models;

namespace Macss.Repositories
{
    public interface IControlRepository
    {
        Task<IEnumerable<MControl>> GetDataBySectionAsync(string section);

        Task UpdateDataValueAsync(string password, string v1, string passKetaFrom, string passKetaTo, string v2, object p1, object p2, object p3, string userID);
    }
}