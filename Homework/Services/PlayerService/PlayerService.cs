using Homework.Model.Mongo;
using Homework.Model.Player;
using Homework.Model.ServiceResponse;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Reflection;

namespace Homework.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IMongoCollection<PlayerModel> _playerCollection;

        public PlayerService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _playerCollection = database.GetCollection<PlayerModel>(mongoDBSettings.Value.PlayerCollection);
        }

        /// <summary>
        /// Used for creating of a player
        /// </summary>
        /// <param name="player">It needs player model which is his new guid and starting balance</param>
        /// <returns>Whether it was successful or failed</returns>
        public async Task<ServiceResponse<object>> CreateAsync(PlayerModel personModel)
        {
            ServiceResponse<object> serviceResponse = new ServiceResponse<object>();

            try
            {
                var data = await _playerCollection.FindAsync(player => player.Guid == personModel.Guid);
                if(data != null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Player with the same guid already exists.";
                    return serviceResponse;
                }

                await _playerCollection.InsertOneAsync(personModel);
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// Used for getting player balance
        /// </summary>
        /// <param name="guid">Player guid</param>
        /// <returns>Player model with his guid and balance</returns>
        public async Task<ServiceResponse<PlayerModel>> GetAsync(Guid guid)
        {
            ServiceResponse<PlayerModel> serviceResponse = new ServiceResponse<PlayerModel>();

            try
            {
                var data = await _playerCollection.FindAsync(player => player.Guid == guid);
             
                serviceResponse.Data = data.FirstOrDefault();
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
