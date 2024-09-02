using MongoDB.Driver;
using RollBotApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using RollBotApi.Services;
using RollBotApi.DataContext;
using MongoDB.Bson;

namespace RollBotApi.Repositories;

public interface ISerieRepository
{
    Task<IEnumerable<Serie>> GetSeriesAsync();
    Task<Serie> GetSerieByIdAsync(string id);
    Task<Serie> GetSerieByNameAsync(string name);
    Task<Serie> CreateSerieAsync(Serie serie);
    Task UpdateSerieAsync(string id, Serie serie);
    Task DeleteSerieAsync(string id);
    Task UpdateSerieAsync(FilterDefinition<Serie> filter, UpdateDefinition<Serie> update);
}

public class SerieRepository : ISerieRepository
{
    private readonly IMongoCollection<Serie> _seriesCollection;
    private readonly ILoggingService _loggingService;

    public SerieRepository(IMongoContext context, ILoggingService loggingService)
    {
        _seriesCollection = context.SeriesCollection;
        _loggingService = loggingService;
    }

    public async Task<IEnumerable<Serie>> GetSeriesAsync()
    {
        _loggingService.LogInformation("Serie Repository: Getting all series");
        return await _seriesCollection.Find(serie => true).ToListAsync();
    }

    public async Task<Serie> GetSerieByIdAsync(string id)
    {
        _loggingService.LogInformation($"Serie Repository: Getting serie with id {id}");
        var objectId = new ObjectId(id);
        return await _seriesCollection.Find(serie => serie.Id == objectId).FirstOrDefaultAsync();
    }

    public async Task<Serie> GetSerieByNameAsync(string name)
    {
        _loggingService.LogInformation($"Serie Repository: Getting serie with name {name}");
        return await _seriesCollection.Find(serie => serie.Name == name).FirstOrDefaultAsync();
    }

    public async Task<Serie> CreateSerieAsync(Serie serie)
    {
        _loggingService.LogInformation($"Serie Repository: Creating serie with id {serie.Id}");
        await _seriesCollection.InsertOneAsync(serie);
        return serie;
    }

    public async Task UpdateSerieAsync(string id, Serie serie)
    {
        _loggingService.LogInformation($"Serie Repository: Updating serie with id {id}");
        var objectId = new ObjectId(id);
        await _seriesCollection.ReplaceOneAsync(s => s.Id == objectId, serie);
    }

    public async Task DeleteSerieAsync(string id)
    {
        _loggingService.LogInformation($"Serie Repository: Deleting serie with id {id}");
        var objectId = new ObjectId(id);
        await _seriesCollection.DeleteOneAsync(serie => serie.Id == objectId);
    }

    public async Task UpdateSerieAsync(FilterDefinition<Serie> filter, UpdateDefinition<Serie> update)
    {
        await _seriesCollection.UpdateOneAsync(filter, update);
    }

}