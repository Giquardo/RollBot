using MongoDB.Driver;
using RollBotApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using RollBotApi.Services;
using RollBotApi.DataContext;
using MongoDB.Bson;
using RollBotApi.DTOs;

namespace RollBotApi.Repositories;

public interface ITagRepository
{
    Task<IEnumerable<SerieTag>> GetTagsAsync();
    Task<SerieTag> GetTagByIdAsync(string id);
    Task<List<TagDisplayDto>> GetTagNamesByIdsAsync(List<string> ids);
    Task<SerieTag> CreateTagAsync(SerieTag tag);
    Task UpdateTagAsync(string id, SerieTag tag);
    Task DeleteTagAsync(string id);
    Task<SerieTag> GetTagByNameAsync(string name);
}


public class TagRepository : ITagRepository
{
    private readonly IMongoCollection<SerieTag> _tagsCollection;
    private readonly ILoggingService _loggingService;

    public TagRepository(IMongoContext context, ILoggingService loggingService)
    {
        _tagsCollection = context.TagsCollection;
        _loggingService = loggingService;
    }

    public async Task<IEnumerable<SerieTag>> GetTagsAsync()
    {
        _loggingService.LogInformation("Tag Repository: Getting all tags");
        return await _tagsCollection.Find(tag => true).ToListAsync();
    }

    public async Task<SerieTag> GetTagByIdAsync(string id)
    {
        _loggingService.LogInformation($"Tag Repository: Getting tag with id {id}");
        return await _tagsCollection.Find<SerieTag>(tag => tag.Id.ToString() == id).FirstOrDefaultAsync();
    }

    public async Task<List<TagDisplayDto>> GetTagNamesByIdsAsync(List<string> ids)
    {
        _loggingService.LogInformation("Tag Repository: Getting tag names by ids");
        var tags = await _tagsCollection.Find(t => ids.Contains(t.Id.ToString())).ToListAsync();

        return tags.Select(t => new TagDisplayDto
        {
            Id = t.Id.ToString(),
            Name = t.Name
        }).ToList();
    }

    public async Task<SerieTag> CreateTagAsync(SerieTag tag)
    {
        _loggingService.LogInformation($"Tag Repository: Creating tag with name {tag.Name}");
        await _tagsCollection.InsertOneAsync(tag);
        return tag;
    }

    public async Task UpdateTagAsync(string id, SerieTag tag)
    {
        _loggingService.LogInformation($"Tag Repository: Updating tag with id {id}");
        await _tagsCollection.ReplaceOneAsync(t => t.Id.ToString() == id, tag);
    }

    public async Task DeleteTagAsync(string id)
    {
        _loggingService.LogInformation($"Tag Repository: Deleting tag with id {id}");
        await _tagsCollection.DeleteOneAsync(tag => tag.Id.ToString() == id);
    }

    public async Task<SerieTag> GetTagByNameAsync(string name)
    {
        _loggingService.LogInformation($"Tag Repository: Getting tag with name {name}");
        return await _tagsCollection.Find<SerieTag>(tag => tag.Name == name).FirstOrDefaultAsync();
    }
}
