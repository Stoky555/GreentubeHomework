using Homework.Model.Mongo;
using Homework.Model.Player;
using Homework.Model.ServiceResponse;
using Homework.Model.Transaction;
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

            try
            {
                await _transactionCollection.InsertOneAsync(new TransactionModel
                {
                    PlayerGuid = playerGuid,
                    TransactionType = TransactionType.Deposit,
                    CreatedAt = DateTime.Now,
                    Amount = amount
                });
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                throw;
            }
        }

        public async Task<ServiceResponse<string>> StakeAsync(decimal amount, Guid playerGuid)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

            try
            {
                await _transactionCollection.InsertOneAsync(new TransactionModel
                {
                    PlayerGuid = playerGuid,
                    TransactionType = TransactionType.Deposit,
                    CreatedAt = DateTime.Now,
                    Amount = amount
                });
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                throw;
            }
        }

        public async Task<ServiceResponse<string>> WinAsync(decimal amount, Guid playerGuid)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

            try
            {
                await _transactionCollection.InsertOneAsync(new TransactionModel
                {
                    PlayerGuid = playerGuid,
                    TransactionType = TransactionType.Deposit,
                    CreatedAt = DateTime.Now,
                    Amount = amount
                });
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                throw;
            }
        }
    }
}


