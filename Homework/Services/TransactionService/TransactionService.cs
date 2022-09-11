using Homework.Model.Mongo;
using Homework.Model.Player;
using Homework.Model.ServiceResponse;
using Homework.Model.Transaction;
using Homework.Model.Transaction.Enum;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Numerics;
using System.Transactions;

namespace Homework.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMongoCollection<TransactionModel> _transactionCollection;
        private readonly IMongoCollection<PlayerModel> _playerCollection;

        public TransactionService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _transactionCollection = database.GetCollection<TransactionModel>(mongoDBSettings.Value.TransactionCollection);
            _playerCollection = database.GetCollection<PlayerModel>(mongoDBSettings.Value.PlayerCollection);
        }

        /// <summary>
        /// Deposit an amount to a player.
        /// </summary>
        /// <param name="amount">Amount to deposit to a player</param>
        /// <param name="playerGuid">Player guid</param>
        /// <returns>Whether a transaction was successful or failed.</returns>
        public async Task<ServiceResponse<string>> DepositAsync(decimal amount, Guid playerGuid)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            TransactionModel transaction = new TransactionModel
            {
                PlayerGuid = playerGuid,
                CreatedAt = DateTime.Now,
                Amount = amount,
                TransactionType = TransactionType.Deposit
            };

            try
            {
                if (amount <= 0)
                {
                    string errorMessage = "Amount can not be less or equal to 0.";
                    return await TransactionFailed(errorMessage, transaction); ;
                }

                var data = await _playerCollection.FindAsync(player => player.Guid == playerGuid);
                var player = data.FirstOrDefault();

                player.Balance += amount;

                await _playerCollection.ReplaceOneAsync(x => x.Guid == playerGuid, player);
                serviceResponse.Data = "Accepted";

                transaction.TransactionResult = TransactionResult.Succeeded;
                await _transactionCollection.InsertOneAsync(transaction);

                return serviceResponse;
            }
            catch (Exception ex)
            {
                return await TransactionFailed(ex.Message, transaction);
            }
        }

        /// <summary>
        /// Player staked an amount which is substracted from his balance.
        /// </summary>
        /// <param name="amount">Amount to stake</param>
        /// <param name="playerGuid">Player guid</param>
        /// <returns>Whether a transaction was successful or failed.</returns>
        public async Task<ServiceResponse<string>> StakeAsync(decimal amount, Guid playerGuid)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            TransactionModel transaction = new TransactionModel
            {
                PlayerGuid = playerGuid,
                CreatedAt = DateTime.Now,
                Amount = amount,
                TransactionType = TransactionType.Stake
            };

            try
            {
                if(amount <= 0)
                {
                    string errorMessage = "Amount can not be less or equal to 0.";
                    return await TransactionFailed(errorMessage, transaction);
                }

                var data = await _playerCollection.FindAsync(player => player.Guid == playerGuid);
                var player = data.FirstOrDefault();
                if (player.Balance-amount < 0) 
                {
                    string errorMessage = "User does not have enough money.";
                    return await TransactionFailed(errorMessage, transaction);
                }

                player.Balance -= amount;

                await _playerCollection.ReplaceOneAsync(x => x.Guid == playerGuid, player);
                serviceResponse.Data = "Accepted";

                transaction.TransactionResult = TransactionResult.Succeeded;
                await _transactionCollection.InsertOneAsync(transaction);

                return serviceResponse;
            }
            catch (Exception ex)
            {
                return await TransactionFailed(ex.Message, transaction);
            }
        }

        /// <summary>
        /// Player won an amount which is deposited to his balance.
        /// </summary>
        /// <param name="amount">Amount to add to player balance</param>
        /// <param name="playerGuid">Player guid</param>
        /// <returns>Whether a transaction was successful or failed.</returns>
        public async Task<ServiceResponse<string>> WinAsync(decimal amount, Guid playerGuid)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            TransactionModel transaction = new TransactionModel
            {
                PlayerGuid = playerGuid,
                CreatedAt = DateTime.Now,
                Amount = amount,
                TransactionType = TransactionType.Win
            };

            try
            {
                if (amount <= 0)
                {
                    string errorMessage = "Amount can not be less or equal to 0.";
                    return await TransactionFailed(errorMessage, transaction);
                }

                var data = await _playerCollection.FindAsync(player => player.Guid == playerGuid);
                var player = data.FirstOrDefault();

                player.Balance += amount;

                await _playerCollection.ReplaceOneAsync(x => x.Guid == playerGuid, player);
                serviceResponse.Data = "Accepted";

                transaction.TransactionResult = TransactionResult.Succeeded;
                await _transactionCollection.InsertOneAsync(transaction);

                return serviceResponse;
            }
            catch (Exception ex)
            {
                return await TransactionFailed(ex.Message, transaction);
            }
        }

        /// <summary>
        /// Used when transaction failed to fill up needed collections and serviceResponse
        /// </summary>
        /// <param name="errorMessage">Message as why transaction failed</param>
        /// <param name="transaction">Transaction body</param>
        /// <returns>Service response failed transaction.</returns>
        private async Task<ServiceResponse<string>> TransactionFailed(string errorMessage, TransactionModel transaction)
        {
            transaction.TransactionResult = TransactionResult.Failed;
            transaction.ErrorMessage = errorMessage;
            await _transactionCollection.InsertOneAsync(transaction);

            var result = new ServiceResponse<string>();
            result.Success = false;
            result.Message = errorMessage;
            result.Data = "Rejected";

            return result;
        }
    }
}


