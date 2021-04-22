using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using stellar_dotnet_sdk;
using stellar_dotnet_sdk.requests;
using stellar_dotnet_sdk.responses.operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Domain.Models;
using TDS.Domain.Services.Abstract;
using TDS.Infrastructure.Models;
using TDS.Infrastructure.Repositories;

namespace TDS.Infrastructure.Services
{
    public class AccountDiscoveringService : IAccountDiscoveringService
    {
        private readonly IRepository<Payment, Guid> _paymentRepository;
        private readonly IRepository<Models.Account, Guid> _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AccountDiscoveringService(IMapper mapper, IConfiguration configuration, IRepository<Payment, Guid> paymentRepository, IRepository<Models.Account, Guid> accountRepository)
        {
            _configuration = configuration;
            _paymentRepository = paymentRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<List<AccountDto>> GetAccountList()
        {
            var result = await _accountRepository.GetAll().ToListAsync();

            return _mapper.Map<List<AccountDto>>(result);
        }

        public async Task UpdateAccountPaymentsAsync(AccountDto inputDto)
        {
            try
            {

                var account = await _accountRepository.GetAsync(inputDto.Id);
                using (var server = new Server(_configuration["serverUrl"]))
                {
                    try
                    {

                        var payments = new List<Payment>();
                        var limit = 10;
                        string cursor = account.Cursor;
                        string transactionType = "payment";

                        var requestBuilder = server.Payments.ForAccount(account.Address)
                            .Order(OrderDirection.ASC)
                            .Limit(limit);
                        if (!string.IsNullOrEmpty(cursor))
                        {
                            requestBuilder.Cursor(cursor);
                        }

                        var result = await requestBuilder.Execute();
                        var filteredREsult = result.Records.Where(x => x.Type == transactionType).Select(x => x as PaymentOperationResponse).ToArray();
                        var getNextCursor = filteredREsult.LastOrDefault();
                        if (getNextCursor != null)
                        {
                            cursor = getNextCursor.PagingToken;
                            account.Cursor = cursor;
                            account.LastDiscoveredAt = DateTime.UtcNow;
                        }

                        foreach (var payment in filteredREsult)
                        {
                            try
                            {
                                await _paymentRepository.InsertAsync(new Payment()
                                {
                                    PaymentId = payment.Id.ToString(),
                                    From = payment.From,
                                    To = payment.To,
                                    PagingToken = payment.PagingToken,
                                    SourceAccount = payment.SourceAccount,
                                    TransactionHash = payment.TransactionHash,
                                    TransactionSuccessful = payment.TransactionSuccessful,
                                    AssetCode = payment.AssetCode,
                                    ActualCreatedAt = DateTime.Parse(payment.CreatedAt),

                                    Amount = double.Parse(payment.Amount)
                                });

                                await _paymentRepository.SaveChangesAsync();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                        if (getNextCursor != null)
                        {
                            cursor = getNextCursor.PagingToken;
                            account.Cursor = cursor;
                            account.LastDiscoveredAt = DateTime.UtcNow;
                        }

                        await _accountRepository.UpdateAsync(account);
                        await _accountRepository.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        throw;
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}