using System.Collections.Generic;
using System.Threading.Tasks;
using RollBotApi.Repositories;
using RollBotApi.Models;
using RollBotApi.Services;

namespace RollBotApi.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers();
    Task<User> GetUser(string discordId);
    Task<User> CreateUser(User user);
    Task UpdateUser(string discordId, User user);
    Task DeleteUser(string discordId);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILoggingService _loggingService;

    public UserService(IUserRepository userRepository, ILoggingService loggingService)
    {
        _userRepository = userRepository;
        _loggingService = loggingService;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        _loggingService.LogInformation("User Service: Getting all users");
        return await _userRepository.GetUsers();
    }

    public async Task<User> GetUser(string discordId)
    {
        _loggingService.LogInformation($"User Service: Getting user with id {discordId}");
        return await _userRepository.GetUser(discordId);
    }

    public async Task<User> CreateUser(User user)
    {
        _loggingService.LogInformation($"User Service: Creating user with id {user.DiscordId}");
        return await _userRepository.CreateUser(user);
    }

    public async Task UpdateUser(string discordId, User user)
    {
        _loggingService.LogInformation($"User Service: Updating user with id {discordId}");
        await _userRepository.UpdateUser(discordId, user);
    }

    public async Task DeleteUser(string discordId)
    {
        _loggingService.LogInformation($"User Service: Deleting user with id {discordId}");
        await _userRepository.DeleteUser(discordId);
    }
}