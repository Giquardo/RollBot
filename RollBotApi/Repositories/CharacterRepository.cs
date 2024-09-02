using MongoDB.Driver;
using RollBotApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using RollBotApi.Services;
using RollBotApi.DataContext;
using MongoDB.Bson;
using RollBotApi.DTOs;


namespace RollBotApi.Repositories;

public interface ICharacterRepository
{
    Task<IEnumerable<Character>> GetCharacters();
    Task<Character> GetCharacterById(string id);
    Task<List<CharacterDisplayDto>> GetCharacterNamesByIdsAsync(List<string> ids);
    Task<Character> CreateCharacter(Character character);
    Task UpdateCharacter(string id, Character character);
    Task DeleteCharacter(string id);
}

public class CharacterRepository : ICharacterRepository
{
    private readonly IMongoCollection<Character> _charactersCollection;
    private readonly ILoggingService _loggingService;

    public CharacterRepository(IMongoContext context, ILoggingService loggingService)
    {
        _charactersCollection = context.CharactersCollection;
        _loggingService = loggingService;
    }

    public async Task<IEnumerable<Character>> GetCharacters()
    {
        _loggingService.LogInformation("Character Repository: Getting all characters");
        return await _charactersCollection.Find(character => true).ToListAsync();
    }

    public async Task<Character> GetCharacterById(string id)
    {
        _loggingService.LogInformation($"Character Repository: Getting character with id {id}");
        var objectId = new ObjectId(id);
        return await _charactersCollection.Find(character => character.Id == objectId).FirstOrDefaultAsync();
    }

    public async Task<List<CharacterDisplayDto>> GetCharacterNamesByIdsAsync(List<string> ids)
    {
        _loggingService.LogInformation("Character Repository: Getting character names by ids");
        var objectIds = ids.Select(id => new ObjectId(id)).ToList();
        var characters = await _charactersCollection.Find(c => objectIds.Contains(c.Id)).ToListAsync();

        return characters.Select(c => new CharacterDisplayDto
        {
            Id = c.Id.ToString(),
            Name = c.Name
        }).ToList();
    }

    public async Task<Character> CreateCharacter(Character character)
    {
        _loggingService.LogInformation($"Character Repository: Creating character with id {character.Id}");
        await _charactersCollection.InsertOneAsync(character);
        return character;
    }

    public async Task UpdateCharacter(string id, Character character)
    {
        _loggingService.LogInformation($"Character Repository: Updating character with id {id}");
        var objectId = new ObjectId(id);
        await _charactersCollection.ReplaceOneAsync(character => character.Id == objectId, character);
    }

    public async Task DeleteCharacter(string id)
    {
        _loggingService.LogInformation($"Character Repository: Deleting character with id {id}");
        var objectId = new ObjectId(id);
        await _charactersCollection.DeleteOneAsync(character => character.Id == objectId);
    }

}