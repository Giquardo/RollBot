using MongoDB.Driver;
using RollBotApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using RollBotApi.Services;
using RollBotApi.DataContext;


namespace RollBotApi.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsers();
    Task<User> GetUser(string discordId);
    Task<User> CreateUser(User user);
    Task UpdateUser(string discordId, User user);
    Task DeleteUser(string discordId);
}

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _usersCollection;
    private readonly ILoggingService _loggingService;

    public UserRepository(IMongoContext context, ILoggingService loggingService)
    {
        _usersCollection = context.UsersCollection;
        _loggingService = loggingService;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        _loggingService.LogInformation("User Repository: Getting all users");
        return await _usersCollection.Find(user => true).ToListAsync();
    }

    public async Task<User> GetUser(string discordId)
    {
        _loggingService.LogInformation($"User Repository: Getting user with id {discordId}");
        return await _usersCollection.Find(user => user.DiscordId == discordId).FirstOrDefaultAsync();
    }

    public async Task<User> CreateUser(User user)
    {
        _loggingService.LogInformation($"User Repository: Creating user with id {user.DiscordId}");
        await _usersCollection.InsertOneAsync(user);
        return user;
    }

    public async Task UpdateUser(string discordId, User user)
    {
        _loggingService.LogInformation($"User Repository: Updating user with id {discordId}");
        await _usersCollection.ReplaceOneAsync(user => user.DiscordId == discordId, user);
    }

    public async Task DeleteUser(string discordId)
    {
        _loggingService.LogInformation($"User Repository: Deleting user with id {discordId}");
        await _usersCollection.DeleteOneAsync(user => user.DiscordId == discordId);
    }

}