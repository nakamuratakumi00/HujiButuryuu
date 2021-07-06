using Macss.Areas.Tass.Models;
using Macss.Areas.Tass.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Macss.Areas.Tass.Repositories
{
    public interface ITodokesakiRepositorie
    {
        Task<IEnumerable<TodokesakiInformation>> GetTodokesakisAsync(TodokesakiSerch todokesaki);

        Task<IEnumerable<TodokesakiViewModel>> GetTodokesakiAsync(string TdkCod);

        decimal GetTdkcod();

        Task<IEnumerable<MUnsouPostalcode>> GetMUnsouPostalcodeAsync();

        Task InsertTodokesakiAsync(TodokesakiViewModel todokesaki, string loginUser);

        Task UpdateTodokesakiAsync(TodokesakiViewModel todokesaki, string loginUser);

        Task DeleteTodokesakiAsync(TodokesakiViewModel todokesaki, string loginUser);

        Task<IEnumerable<v_unsou_todokesaki>> GetTodokesakiExcelAsync(string TdkCod);

        Task<IEnumerable<TodokesakiFileInformation>> GetTodokesakiExcelAsync(TodokesakiSerch todokesaki);
    }
}