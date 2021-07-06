using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Macss.Models;
using Macss.ViewModels;

namespace Macss.Repositories
{
    public class MShukkabashoRepository
    {

        private readonly ApplicationDB dbContext;

        public MShukkabashoRepository(ApplicationDB db)
        {
            this.dbContext = db;
        }

        public async Task<IEnumerable<MShukkabasho>> GetAllShukkabashosAsync()
        {
            return await dbContext.MShukkabasho.OrderBy(x => x.Sybcod).ToListAsync();
        }

    }
}