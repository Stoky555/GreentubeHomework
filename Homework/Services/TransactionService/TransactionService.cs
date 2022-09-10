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
                var data = await _playerCollection.FindAsync(player => player.Guid == playerGuid);
                var player = data.FirstOrDefault();

                player.Balance += amount;

                await _playerCollection.ReplaceOneAsync(x => x.Guid == playerGuid, player);
                serviceResponse.Message = "Accepted";

                transaction.TransactionResult = TransactionResult.Succeeded;
                await _transactionCollection.InsertOneAsync(transaction);

                return serviceResponse;
            }
            catch (Exception ex)
            {
                return await TransactionFailed(ex.Message, transaction);
            }
        }

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
                var data = await _playerCollection.FindAsync(player => player.Guid == playerGuid);
                var player = data.FirstOrDefault();
                if (player.Balance-amount < 0) 
                {
                    transaction.TransactionResult = TransactionResult.Failed;
                    await _transactionCollection.InsertOneAsync(transaction);

                    serviceResponse.Success = false;
                    serviceResponse.Message = "Rejected";
                    return serviceResponse;
                }

                player.Balance -= amount;

                await _playerCollection.ReplaceOneAsync(x => x.Guid == playerGuid, player);
                serviceResponse.Message = "Accepted";

                transaction.TransactionResult = TransactionResult.Succeeded;
                await _transactionCollection.InsertOneAsync(transaction);

                return serviceResponse;
            }
            catch (Exception ex)
            {
                return await TransactionFailed(ex.Message, transaction);
            }
        }

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
                var data = await _playerCollection.FindAsync(player => player.Guid == playerGuid);
                var player = data.FirstOrDefault();

                player.Balance += amount;

                await _playerCollection.ReplaceOneAsync(x => x.Guid == playerGuid, player);
                serviceResponse.Message = "Accepted";

                transaction.TransactionResult = TransactionResult.Succeeded;
                await _transactionCollection.InsertOneAsync(transaction);

                return serviceResponse;
            }
            catch (Exception ex)
            {
                return await TransactionFailed(ex.Message, transaction);
            }
        }

        private async Task<ServiceResponse<string>> TransactionFailed(string errorMessage, TransactionModel transaction)
        {
            transaction.TransactionResult = TransactionResult.Failed;
            transaction.ErrorMessage = errorMessage;
            await _transactionCollection.InsertOneAsync(transaction);

            var result = new ServiceResponse<string>();
            result.Success = false;
            result.Message = errorMessage;

            return result;
        }
    }
}


