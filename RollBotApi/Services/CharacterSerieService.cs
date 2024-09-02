using RollBotApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using RollBotApi.Services;
using RollBotApi.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System;
using RollBotApi.DTOs;


namespace RollBotApi.Services;
public interface ICharacterSerieService
{
    // Character methods
    Task<IEnumerable<Character>> GetCharactersAsync();
    Task<Character> GetCharacterByIdAsync(string id);
    Task<List<CharacterDisplayDto>> GetCharacterNamesByIdsAsync(List<string> ids);
    Task<Character> CreateCharacterAsync(Character character);
    Task UpdateCharacterAsync(string id, Character character);
    Task DeleteCharacterAsync(string id);

    // Serie methods
    Task<IEnumerable<Serie>> GetSeriesAsync();
    Task<Serie> GetSerieByIdAsync(string id);
    Task<Serie> GetSerieByNameAsync(string name);
    Task<Serie> CreateSerieAsync(Serie serie, List<string> tagNames);
    Task UpdateSerieAsync(string id, Serie serie);
    Task DeleteSerieAsync(string id);

    // Tag methods
    Task<IEnumerable<SerieTag>> GetTagsAsync();
    Task<SerieTag> GetTagByIdAsync(string id);
    Task<List<TagDisplayDto>> GetTagNamesByIdsAsync(List<string> ids);
    Task<SerieTag> CreateTagAsync(SerieTag tag);
    Task UpdateTagAsync(string id, SerieTag tag);
    Task DeleteTagAsync(string id);
    Task<SerieTag> GetTagByNameAsync(string name);

    // Other methods as necessary
    Task AddCharacterToSerieAsync(ObjectId serieId, ObjectId characterId);
    Task RemoveCharacterFromSerieAsync(ObjectId serieId, ObjectId characterId);
}


public class CharacterSerieService : ICharacterSerieService
{
    private readonly ICharacterRepository _characterRepository;
    private readonly ISerieRepository _serieRepository;
    private readonly ITagRepository _tagRepository;
    private readonly ILoggingService _loggingService;

    public CharacterSerieService(ICharacterRepository characterRepository, ISerieRepository serieRepository, ILoggingService loggingService, ITagRepository tagRepository)
    {
        _characterRepository = characterRepository;
        _serieRepository = serieRepository;
        _loggingService = loggingService;
        _tagRepository = tagRepository;
    }

    // Character methods
    public async Task<IEnumerable<Character>> GetCharactersAsync()
    {
        _loggingService.LogInformation("CharacterSerie Service: Getting all characters");
        return await _characterRepository.GetCharacters();
    }

    public async Task<Character> GetCharacterByIdAsync(string id)
    {
        _loggingService.LogInformation($"CharacterSerie Service: Getting character with id {id}");
        return await _characterRepository.GetCharacterById(id);
    }

    public async Task<List<CharacterDisplayDto>> GetCharacterNamesByIdsAsync(List<string> ids)
    {
        _loggingService.LogInformation("CharacterSerie Service: Getting character names by ids");
        return await _characterRepository.GetCharacterNamesByIdsAsync(ids);
    }

    public async Task<Character> CreateCharacterAsync(Character character)
    {
        _loggingService.LogInformation($"CharacterSerie Service: Creating character with id {character.Id}");
        return await _characterRepository.CreateCharacter(character);
    }

    public async Task UpdateCharacterAsync(string id, Character character)
    {
        _loggingService.LogInformation($"CharacterSerie Service: Updating character with id {id}");
        await _characterRepository.UpdateCharacter(id, character);
    }

    public async Task DeleteCharacterAsync(string id)
    {
        _loggingService.LogInformation($"CharacterSerie Service: Deleting character with id {id}");
        await _characterRepository.DeleteCharacter(id);
    }

    // Serie methods
    public async Task<IEnumerable<Serie>> GetSeriesAsync()
    {
        _loggingService.LogInformation("CharacterSerie Service: Getting all series");
        return await _serieRepository.GetSeriesAsync();
    }

    public async Task<Serie> GetSerieByIdAsync(string id)
    {
        _loggingService.LogInformation($"CharacterSerie Service: Getting serie with id {id}");
        return await _serieRepository.GetSerieByIdAsync(id);
    }

    public async Task<Serie> GetSerieByNameAsync(string name)
    {
        _loggingService.LogInformation($"CharacterSerie Service: Getting serie with name {name}");
        return await _serieRepository.GetSerieByNameAsync(name);
    }

    public async Task<Serie> CreateSerieAsync(Serie serie, List<string> tagNames)
    {
        _loggingService.LogInformation($"CharacterSerieService: Creating serie with name {serie.Name}");

        var tagIds = new List<ObjectId>();
        foreach (var tagName in tagNames)
        {
            var tag = await _tagRepository.GetTagByNameAsync(tagName);
            if (tag == null)
            {
                tag = await _tagRepository.CreateTagAsync(new SerieTag { Name = tagName });
            }
            tagIds.Add(tag.Id); // Convert string Id to ObjectId
        }

        var newSerie = new Serie
        {
            Name = serie.Name,
            TagIds = tagIds
        };

        return await _serieRepository.CreateSerieAsync(newSerie);
    }

    public async Task UpdateSerieAsync(string id, Serie serie)
    {
        _loggingService.LogInformation($"CharacterSerie Service: Updating serie with id {id}");
        await _serieRepository.UpdateSerieAsync(id, serie);
    }

    public async Task DeleteSerieAsync(string id)
    {
        _loggingService.LogInformation($"CharacterSerie Service: Deleting serie with id {id}");

        // Retrieve the series to get the associated character IDs
        var serie = await _serieRepository.GetSerieByIdAsync(id);
        if (serie == null)
        {
            _loggingService.LogWarning($"CharacterSerie Service: Serie with id {id} not found");
            return;
        }

        // Delete all characters associated with the series
        foreach (var characterId in serie.CharacterIds)
        {
            await _characterRepository.DeleteCharacter(characterId.ToString());
        }

        // Delete the series
        await _serieRepository.DeleteSerieAsync(id);
    }

    // Tag methods
    public async Task<IEnumerable<SerieTag>> GetTagsAsync()
    {
        return await _tagRepository.GetTagsAsync();
    }

    public async Task<SerieTag> GetTagByIdAsync(string id)
    {
        return await _tagRepository.GetTagByIdAsync(id);
    }

    public async Task<List<TagDisplayDto>> GetTagNamesByIdsAsync(List<string> ids)
    {
        return await _tagRepository.GetTagNamesByIdsAsync(ids);
    }

    public async Task<SerieTag> CreateTagAsync(SerieTag tag)
    {
        return await _tagRepository.CreateTagAsync(tag);
    }

    public async Task UpdateTagAsync(string id, SerieTag tag)
    {
        await _tagRepository.UpdateTagAsync(id, tag);
    }

    public async Task DeleteTagAsync(string id)
    {
        await _tagRepository.DeleteTagAsync(id);
    }

    public async Task<SerieTag> GetTagByNameAsync(string name)
    {
        return await _tagRepository.GetTagByNameAsync(name);
    }

    public async Task AddCharacterToSerieAsync(ObjectId serieId, ObjectId characterId)
    {
        var filter = Builders<Serie>.Filter.Eq(s => s.Id, serieId);
        var update = Builders<Serie>.Update.AddToSet(s => s.CharacterIds, characterId);
        await _serieRepository.UpdateSerieAsync(filter, update);
    }

    public async Task RemoveCharacterFromSerieAsync(ObjectId serieId, ObjectId characterId)
    {
        var filter = Builders<Serie>.Filter.Eq(s => s.Id, serieId);
        var update = Builders<Serie>.Update.Pull(s => s.CharacterIds, characterId);
        await _serieRepository.UpdateSerieAsync(filter, update);
    }
}
