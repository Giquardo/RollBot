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
    Task<User> GetUser(string id);
    Task<User> CreateUser(User user);
    Task UpdateUser(string id, User user);
    Task DeleteUser(string id);
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

    public async Task<User> GetUser(string id)
    {
        _loggingService.LogInformation($"User Repository: Getting user with id {id}");
        return await _usersCollection.Find(user => user.DiscordId == id).FirstOrDefaultAsync();
    }

    public async Task<User> CreateUser(User user)
    {
        _loggingService.LogInformation($"User Repository: Creating user with id {user.DiscordId}");
        await _usersCollection.InsertOneAsync(user);
        return user;
    }

    public async Task UpdateUser(string id, User user)
    {
        _loggingService.LogInformation($"User Repository: Updating user with id {id}");
        await _usersCollection.ReplaceOneAsync(user => user.DiscordId == id, user);
    }

    public async Task DeleteUser(string id)
    {
        _loggingService.LogInformation($"User Repository: Deleting user with id {id}");
        await _usersCollection.DeleteOneAsync(user => user.DiscordId == id);
    }
}