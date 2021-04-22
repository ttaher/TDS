using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TDS.Domain.Services.Abstract;

namespace TDS.API
{
    public class DiscoveringPaymentHostedService : BackgroundService, IDisposable
    {
        public IServiceProvider Services;

        private readonly ILogger<DiscoveringPaymentHostedService> _logger;
        private readonly IConfiguration _configuration;
        private IAccountDiscoveringService _accountDiscoveringService;

        public DiscoveringPaymentHostedService(IServiceProvider services, IConfiguration configuration, ILogger<DiscoveringPaymentHostedService> logger)
        {
            Services = services;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Account discovering Hosted Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            while (true)
            {

                try
                {
                    _logger.LogInformation("Account discovering process started");
                    using (var scope = Services.CreateScope())
                    {
                        _accountDiscoveringService =
                            scope.ServiceProvider.GetRequiredService<IAccountDiscoveringService>();
                        var accountToList = await _accountDiscoveringService.GetAccountList();
                        if (accountToList.Any())
                        {
                            var options = new ParallelOptions()
                            {
                                MaxDegreeOfParallelism = 2
                            };
                            foreach (var account in accountToList)
                            {
                                await _accountDiscoveringService.UpdateAccountPaymentsAsync(account);
                            }
                        }
                        _logger.LogInformation("Account discovering process finished");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Account discovering process is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
