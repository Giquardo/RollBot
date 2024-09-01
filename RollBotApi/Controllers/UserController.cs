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


}
