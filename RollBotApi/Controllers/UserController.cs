using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using RollBotApi.Models;
using RollBotApi.Services;
using RollBotApi.Repositories;
using RollBotApi.DTOs;

namespace RollBotApi.Controllers;
[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ILoggingService _loggingService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(ILoggingService loggingService, IUserService userService, IMapper mapper)
    {
        _loggingService = loggingService;
        _userService = userService;
        _mapper = mapper;

    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        _loggingService.LogInformation("User Controller: Getting all users");
        var users = await _userService.GetUsers();
        var userDtos = _mapper.Map<List<ReturnUserDto>>(users);
        return Ok(userDtos);
    }

    [HttpGet("{discordId}")]
    public async Task<IActionResult> GetUser(string discordId)
    {
        _loggingService.LogInformation($"User Controller: Getting user with id {discordId}");
        var user = await _userService.GetUser(discordId);
        if (user == null)
        {
            return NotFound(new { Message = $"User with id {discordId} not found." });
        }
        var userDto = _mapper.Map<ReturnUserDto>(user);
        return Ok(userDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] NewUserDto newUserDto)
    {
        _loggingService.LogInformation($"User Controller: Creating user with id {newUserDto.DiscordId}");
        var existingUser = await _userService.GetUser(newUserDto.DiscordId);
        if (existingUser != null)
        {
            return Conflict(new { Message = $"User with id {newUserDto.DiscordId} already exists." });
        }

        var user = _mapper.Map<User>(newUserDto);
        var createdUser = await _userService.CreateUser(user);
        return CreatedAtAction(nameof(GetUser), new { discordId = createdUser.DiscordId }, createdUser);
    }

    [HttpPut("{discordId}")]
    public async Task<IActionResult> UpdateUser(string discordId, [FromBody] User user)
    {
        _loggingService.LogInformation($"User Controller: Updating user with id {discordId}");
        var existingUser = await _userService.GetUser(discordId);
        if (existingUser == null)
        {
            return NotFound(new { Message = $"User with id {discordId} not found." });
        }

        await _userService.UpdateUser(discordId, user);
        return NoContent();
    }

    [HttpDelete("{discordId}")]
    public async Task<IActionResult> DeleteUser(string discordId)
    {
        _loggingService.LogInformation($"User Controller: Deleting user with id {discordId}");
        var existingUser = await _userService.GetUser(discordId);
        if (existingUser == null)
        {
            return NotFound(new { Message = $"User with id {discordId} not found." });
        }

        await _userService.DeleteUser(discordId);
        return NoContent();
    }



}
