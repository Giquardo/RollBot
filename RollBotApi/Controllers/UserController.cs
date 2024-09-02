using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RollBotApi.Models;
using RollBotApi.Services;
using RollBotApi.Repositories;


namespace RollBotApi.Controllers;
[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ILoggingService _loggingService;
    private readonly IUserService _userService;

    public UserController(ILoggingService loggingService, IUserService userService)
    {
        _loggingService = loggingService;
        _userService = userService;

    }

    [HttpGet]
    public async Task<IEnumerable<User>> GetUsers()
    {
        _loggingService.LogInformation("User Controller: Getting all users");
        return await _userService.GetUsers();
    }

    [HttpGet("{discordId}")]
    public async Task<User> GetUser(string discordId)
    {
        _loggingService.LogInformation($"User Controller: Getting user with id {discordId}");
        return await _userService.GetUser(discordId);
    }

    [HttpPost]
    public async Task<User> CreateUser([FromBody] User user)
    {
        _loggingService.LogInformation($"User Controller: Creating user with id {user.DiscordId}");
        return await _userService.CreateUser(user);
    }

    [HttpPut("{discordId}")]
    public async Task UpdateUser(string discordId, [FromBody] User user)
    {
        _loggingService.LogInformation($"User Controller: Updating user with id {discordId}");
        await _userService.UpdateUser(discordId, user);
    }

    [HttpDelete("{discordId}")]
    public async Task DeleteUser(string discordId)
    {
        _loggingService.LogInformation($"User Controller: Deleting user with id {discordId}");
        await _userService.DeleteUser(discordId);
    }



}
