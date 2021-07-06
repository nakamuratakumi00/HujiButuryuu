using Macss.Database;

namespace MacssWeb.Models.Context
{
    public class MacssWebAccountDbContext : MacssDbContext
    {

        public static MacssWebAccountDbContext Create()
        {
            return new MacssWebAccountDbContext();
        }

    }
}