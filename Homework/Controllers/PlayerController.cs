using Homework.Model.Player;
using Homework.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Homework.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly IPlayerService _playerService;

        public PlayerController(ILogger<PlayerController> logger, IPlayerService playerService)
        {
            _logger = logger;
            _playerService = playerService;
        }

        /// <summary>
        /// Used for getting player balance
        /// </summary>
        /// <param name="guid">Player guid</param>
        /// <returns>Player model with his guid and balance</returns>
        [HttpGet("{guid:guid}")]
        public async Task<IActionResult> GetPlayerBalance([FromRoute] Guid guid)
        {
            return Ok(await _playerService.GetAsync(guid));
        }

        /// <summary>
        /// Used for creating of a player
        /// </summary>
        /// <param name="player">It needs player model which is his new guid and starting balance</param>
        /// <returns>Whether it was successful or failed</returns>
        [HttpPost(Name = "CreatePlayer")]
        public async Task<IActionResult> CreatePlayer(PlayerModel player)
        {
            return Ok(await _playerService.CreateAsync(player));
        }
    }
}