using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.Models;
using Macss.ViewModels;

namespace Macss.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountInformation>> GetAllUsersAsync();
        Task<IEnumerable<AccountRoleViewModel>> GetAccountRolesAsync(string id );
        Task<IEnumerable<MAccount>> FindByCodeAsync(string code);

        Task<IEnumerable<(string field, string message)>> UpdateUserAsync(string id, AccountViewModel user, string loginUser);
        Task<IEnumerable<(string field, string message)>> CreateUserAsync(CreateAccountViewModel user);

        Task UpdateFailureCountAsync(string userID, int count);
        Task<List<(string field, string message)>> ConfirmPassword(string password, int mode);

        Task<int> GetPasswordControlValueAsync(string kbn, int defaultValue);

        Task<IEnumerable<TPasswordHistory>> GetPasswordHistoryAsync(string accountId, bool orderF);
    }
}