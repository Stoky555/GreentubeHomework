using Homework.Services;
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

        /// <summary>
        /// Player won an amount which is deposited to his balance.
        /// </summary>
        /// <param name="amount">Amount to add to player balance</param>
        /// <param name="playerGuid">Player guid</param>
        /// <returns>Whether a transaction was successful or failed.</returns>
        [HttpPut("Win/{guid:guid}")]
        public async Task<IActionResult> Win(decimal amount, [FromBody] Guid guid)
        {
            return Ok(await _transactionService.WinAsync(amount, guid));
        }

        /// <summary>
        /// Player staked an amount which is substracted from his balance.
        /// </summary>
        /// <param name="amount">Amount to stake</param>
        /// <param name="playerGuid">Player guid</param>
        /// <returns>Whether a transaction was successful or failed.</returns>
        [HttpPut("Stake/{guid:guid}")]
        public async Task<IActionResult> Stake(decimal amount, [FromBody] Guid guid)
        {
            return Ok(await _transactionService.StakeAsync(amount, guid));
        }

        /// <summary>
        /// Deposit an amount to a player.
        /// </summary>
        /// <param name="amount">Amount to deposit to a player</param>
        /// <param name="playerGuid">Player guid</param>
        /// <returns>Whether a transaction was successful or failed.</returns>
        [HttpPut("Deposit/{guid:guid}")]
        public async Task<IActionResult> Deposit(decimal amount, [FromBody] Guid guid)
        {
            return Ok(await _transactionService.DepositAsync(amount, guid));
        }
    }
}
