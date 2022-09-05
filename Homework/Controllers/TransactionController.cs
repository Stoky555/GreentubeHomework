﻿using Homework.Services;
using Microsoft.AspNetCore.Mvc;

namespace Homework.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : Controller
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionService _transactionService;

        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        [HttpPut("{guid:guid}")]
        public async Task<IActionResult> Win(decimal amount, [FromBody] Guid guid)
        {
            return Ok(_transactionService.WinAsync(amount, guid));
        }

        [HttpPut("{guid:guid}")]
        public async Task<IActionResult> Stake(decimal amount, [FromBody] Guid guid)
        {
            return Ok(_transactionService.StakeAsync(amount, guid));
        }

        [HttpPut("{guid:guid}")]
        public async Task<IActionResult> Deposit(decimal amount, [FromBody] Guid guid)
        {
            return Ok(_transactionService.DepositAsync(amount, guid));
        }
    }
}