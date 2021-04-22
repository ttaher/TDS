using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Domain.Models;

namespace TDS.Domain.Services.Abstract
{
    public interface IAccountInsertHandler
    {
        Task<AccountDto> InsertAccount(AccountDto inputDto);
        Task<List<AccountDto>> InsertAccountList(List<AccountDto> inputDto);
    }
}
