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

        [HttpGet("{guid:guid}")]
        public async Task<IActionResult> GetPlayerBalance([FromRoute] Guid guid)
        {
            return Ok(await _playerService.GetAsync(guid));
        }

        [HttpPost(Name = "CreatePlayer")]
        public async Task<IActionResult> CreatePlayer(PlayerModel player)
        {
            return Ok(await _playerService.CreateAsync(player));
        }
    }
}