using Homework.Model.Player;
using Homework.Model.ServiceResponse;

namespace Homework.Services
{
    public interface IPlayerService
    {
        /// <summary>
        /// Used for getting player balance
        /// </summary>
        /// <param name="guid">Player guid</param>
        /// <returns>Player model with his guid and balance</returns>
        Task<ServiceResponse<PlayerModel>> GetAsync(Guid guid);

        /// <summary>
        /// Used for creating of a player
        /// </summary>
        /// <param name="player">It needs player model which is his new guid and starting balance</param>
        /// <returns>Whether it was successful or failed</returns>
        Task<ServiceResponse<object>> CreateAsync(PlayerModel personModel);
    }
}
