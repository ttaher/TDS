using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Domain.Models;
using TDS.Domain.Services.Abstract;
using TDS.Infrastructure.Models;
using TDS.Infrastructure.Repositories;

namespace TDS.Infrastructure.Services
{
    public class AccountInsertHandler : IAccountInsertHandler
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Account, Guid> _accountRepository;
        public AccountInsertHandler(IRepository<Account, Guid> accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _mapper = mapper;

        }
        public async Task<AccountDto> InsertAccount(AccountDto inputDto)
        {

            var account = new Account() { Address = inputDto.Address };
            await _accountRepository.InsertAsync(account);
            await _accountRepository.SaveChangesAsync();
            return _mapper.Map<AccountDto>(account);
        }


        public async Task<List<AccountDto>> InsertAccountList(List<AccountDto> inputDto)
        {
            var list = new List<AccountDto>();
            foreach (var account in inputDto)
            {
                list.Add(await InsertAccount(account));
            }

            return list;
        }
    }
}
