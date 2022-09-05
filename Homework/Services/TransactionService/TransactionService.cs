using Homework.Model.Mongo;
using Homework.Model.Player;
using Homework.Model.ServiceResponse;
using Homework.Model.Transaction;
using Homework.Model.Transaction.Enum;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

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

                //TODO: deposit an amount to player

                transaction.TransactionResult = TransactionResult.Succeeded;
                await _transactionCollection.InsertOneAsync(transaction);

                return serviceResponse;
            }
            catch (Exception ex)
            {
                transaction.TransactionResult = TransactionResult.Failed;
                transaction.ErrorMessage = ex.Message;
                await _transactionCollection.InsertOneAsync(transaction);

                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
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
                    serviceResponse.Message = "User does not have enough balance to stake that amount.";
                    return serviceResponse;
                }

                //TODO: stake an amount from player

                transaction.TransactionResult = TransactionResult.Succeeded;
                await _transactionCollection.InsertOneAsync(transaction);

                return serviceResponse;
            }
            catch (Exception ex)
            {
                transaction.TransactionResult = TransactionResult.Failed;
                transaction.ErrorMessage = ex.Message;
                await _transactionCollection.InsertOneAsync(transaction);

                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
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
                transaction.TransactionResult = TransactionResult.Succeeded;
                await _transactionCollection.InsertOneAsync(transaction);

                //TODO: deposit an amount to player

                return serviceResponse;
            }
            catch (Exception ex)
            {
                transaction.TransactionResult = TransactionResult.Failed;
                transaction.ErrorMessage = ex.Message;
                await _transactionCollection.InsertOneAsync(transaction);

                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }
    }
}


