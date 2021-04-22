using AutoMapper;
using TDS.Domain.Models;
using TDS.Infrastructure.Models;

namespace TDS.Infrastructure.Mapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, AccountDto>();
        }
    }
}
