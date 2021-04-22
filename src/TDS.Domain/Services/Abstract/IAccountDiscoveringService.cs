using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Domain.Models;

namespace TDS.Domain.Services.Abstract
{
    public interface IAccountDiscoveringService
    {
        Task<List<AccountDto>> GetAccountList();
        Task UpdateAccountPaymentsAsync(AccountDto account);
    }
}
