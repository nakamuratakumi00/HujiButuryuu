using System.Data.Entity;

namespace Macss.Database
{
    public class MacssDbContextBase : DbContext
    {
        public MacssDbContextBase(string nameOrConnectionString) : base(nameOrConnectionString) { }
    }
}
