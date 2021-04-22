using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using TDS.API.Requests;
using TDS.Domain.Models;
using TDS.Domain.Services.Abstract;

namespace TDS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TdsController : ControllerBase
    {
        private readonly ILogger<TdsController> _logger;
        private readonly IMapper _mapper;
        private readonly IAccountInsertHandler _accountInsertHandler;
        private readonly IAccountDiscoveringService _accountDiscoveringService;
        private readonly IAccountQueryHandler _accountQueryHandler;


        public TdsController(ILogger<TdsController> logger,
            IMapper mapper, IAccountInsertHandler accountInsertHandler,
            IAccountDiscoveringService accountDiscoveringService
            , IAccountQueryHandler accountQueryHandler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _accountInsertHandler = accountInsertHandler ?? throw new ArgumentNullException(nameof(accountInsertHandler));
            _accountQueryHandler = accountQueryHandler ?? throw new ArgumentNullException(nameof(accountQueryHandler));
            _accountDiscoveringService = accountDiscoveringService ?? throw new ArgumentNullException(nameof(accountDiscoveringService));
        }

        [HttpPost("insert")]

        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(ProblemDetails))]

        public Task<IActionResult> UploadWalletAddresses([FromBody] List<UploadWalletAddressesRequest> request)
        {
            return InvokeWithErrorHandling(() => AddWalletAddresses(request));
        }

        [HttpGet("{address}/balance")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable, Type = typeof(ProblemDetails))]

        public Task<IActionResult> GetBalanceByAddress([FromRoute] string address)
        {
            return InvokeWithErrorHandling(() => GetWalletBalanceByAddress(address));
        }

        private async Task<IActionResult> GetWalletBalanceByAddress(string address)
        {
            var data = await _accountQueryHandler.GetWalletBalance(address);
            return Ok(data);
        }

        private async Task<IActionResult> AddWalletAddresses(List<UploadWalletAddressesRequest> request)
        {

            var accountList = _mapper.Map<List<AccountDto>>(request);
            await _accountInsertHandler.InsertAccountList(accountList);

            return Accepted();
        }

        private async Task<IActionResult> InvokeWithErrorHandling(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception exception)
            {
                return HandleCommonException(exception);
            }
        }

        private IActionResult HandleCommonException(Exception ex)
        {
            if (ex.GetType() == typeof(ArgumentNullException))
            {
                return BadRequest();
            }

            _logger.LogError(ex, "Internal Server Error Occurred");

            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
