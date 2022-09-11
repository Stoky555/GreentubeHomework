using Homework.Model.ServiceResponse;

namespace Homework.Services
{
    public interface ITransactionService
    {
        /// <summary>
        /// Player won an amount which is deposited to his balance.
        /// </summary>
        /// <param name="amount">Amount to add to player balance</param>
        /// <param name="playerGuid">Player guid</param>
        /// <returns>Whether a transaction was successful or failed.</returns>
        Task<ServiceResponse<string>> WinAsync(decimal amount, Guid playerGuid);

        /// <summary>
        /// Deposit an amount to a player.
        /// </summary>
        /// <param name="amount">Amount to deposit to a player</param>
        /// <param name="playerGuid">Player guid</param>
        /// <returns>Whether a transaction was successful or failed.</returns>
        Task<ServiceResponse<string>> DepositAsync(decimal amount, Guid playerGuid);

        /// <summary>
        /// Player staked an amount which is substracted from his balance.
        /// </summary>
        /// <param name="amount">Amount to stake</param>
        /// <param name="playerGuid">Player guid</param>
        /// <returns>Whether a transaction was successful or failed.</returns>
        Task<ServiceResponse<string>> StakeAsync(decimal amount, Guid playerGuid);
    }
}


