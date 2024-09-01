using System.Collections.Generic;
using System.Threading.Tasks;
using RollBotApi.Repositories;
using RollBotApi.Models;
using RollBotApi.Services;

namespace RollBotApi.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers();
    Task<User> GetUser(string id);
    Task<User> CreateUser(User user);
    Task UpdateUser(string id, User user);
    Task DeleteUser(string id);
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

    public async Task<User> GetUser(string id)
    {
        _loggingService.LogInformation($"User Service: Getting user with id {id}");
        return await _userRepository.GetUser(id);
    }

    public async Task<User> CreateUser(User user)
    {
        _loggingService.LogInformation($"User Service: Creating user with id {user.DiscordId}");
        return await _userRepository.CreateUser(user);
    }

    public async Task UpdateUser(string id, User user)
    {
        _loggingService.LogInformation($"User Service: Updating user with id {id}");
        await _userRepository.UpdateUser(id, user);
    }

    public async Task DeleteUser(string id)
    {
        _loggingService.LogInformation($"User Service: Deleting user with id {id}");
        await _userRepository.DeleteUser(id);
    }
}