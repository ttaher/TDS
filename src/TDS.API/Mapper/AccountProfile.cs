using AutoMapper;
using TDS.API.Requests;
using TDS.Domain.Models;

namespace TDS.API.Mapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<UploadWalletAddressesRequest, AccountDto>();
        }
    }
}
