using System.Collections.Generic;
using System.Threading.Tasks;
using Macss.Models;

namespace Macss.Repositories
{
    public interface IMShukkabashoRepository
    {

        Task<IEnumerable<MShukkabasho>> GetAllShukkabashosAsync();

    }
}