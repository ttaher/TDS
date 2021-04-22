using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Domain.Services.Abstract;
using TDS.Infrastructure.Models;
using TDS.Infrastructure.Repositories;

namespace TDS.Infrastructure.Services
{
    public class AccountQueryHandler : IAccountQueryHandler
    {
        private readonly IRepository<Payment, Guid> _paymentRepository;
        private readonly IRepository<Models.Account, Guid> _accountRepository;
        public AccountQueryHandler(IRepository<Payment, Guid> paymentRepository, IRepository<Models.Account, Guid> accountRepository)
        {
            _paymentRepository = paymentRepository;
            _accountRepository = accountRepository;
        }
        public async Task<Dictionary<string, double>> GetWalletBalance(string address)
        {
            var result = new Dictionary<string, double>();
            var accountExists = await _accountRepository.GetAll().Where(x => x.Address == address).FirstOrDefaultAsync();
            if (accountExists == null)
            {
                return null;
            }

            var credit = await _paymentRepository.GetAll()
                .Where(p => p.To == address)
                .GroupBy(P => P.From)
                .Select((g) => new { Amount = g.Sum(gp => gp.Amount), Account = g.Key }).ToListAsync();


            var debit = await _paymentRepository.GetAll()
                .Where(p => p.From == address)
                .GroupBy(P => P.To)
                .Select((g) => new { Amount = -1*g.Sum(gp => gp.Amount), Account = g.Key }).ToListAsync();
    
            credit.AddRange(debit);

            var aggregatedTransferred = credit
                .GroupBy(g => g.Account)
                .Select((g) => new {Amount = -g.Sum(gp => gp.Amount), Account = g.Key}).ToList();


            return aggregatedTransferred.ToDictionary(x=>x.Account,x=>x.Amount);
        }
    }
}
