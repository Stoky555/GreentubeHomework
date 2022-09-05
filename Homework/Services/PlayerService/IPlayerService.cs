using Homework.Model.Player;
using Homework.Model.ServiceResponse;

namespace Homework.Services
{
    public interface IPlayerService
    {
        Task<ServiceResponse<PlayerModel>> GetAsync(Guid guid);
        Task<ServiceResponse<object>> CreateAsync(PlayerModel personModel);
    }
}
