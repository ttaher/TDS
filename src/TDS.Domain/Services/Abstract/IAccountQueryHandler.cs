using System.Collections.Generic;
using System.Threading.Tasks;

namespace TDS.Domain.Services.Abstract
{
    public interface IAccountQueryHandler
    {
        Task<Dictionary<string, double>> GetWalletBalance(string address);
    }
}
