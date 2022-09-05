using Homework.Model.Mongo;
using Homework.Model.Player;
using Homework.Model.ServiceResponse;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
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

        public async Task<ServiceResponse<object>> CreateAsync(PlayerModel personModel)
        {
            ServiceResponse<object> serviceResponse = new ServiceResponse<object>();

            try
            {
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
