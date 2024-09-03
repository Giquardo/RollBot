using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RollBotApi.Models;
using RollBotApi.Configuration;


namespace RollBotApi.DataContext;


public interface IMongoContext
{

    IMongoClient Client { get; }
    IMongoDatabase Database { get; }
    IMongoCollection<User> UsersCollection { get; }
    IMongoCollection<Serie> SeriesCollection { get; }
    IMongoCollection<Character> CharactersCollection { get; }
    IMongoCollection<SerieTag> TagsCollection { get; }
    IMongoCollection<CardPack> CardPacksCollection { get; }
    //IMongoCollection<Card> CardsCollection { get; }
}

public class MongoContext : IMongoContext
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly DatabaseSettings _settings;

    public IMongoClient Client
    {
        get
        {
            return _client;
        }
    }
    public IMongoDatabase Database => _database;

    public MongoContext(IOptions<DatabaseSettings> dbOptions)
    {
        _settings = dbOptions.Value;
        _client = new MongoClient(_settings.ConnectionString);
        _database = _client.GetDatabase(_settings.DatabaseName);
    }

    public IMongoCollection<User> UsersCollection
    {
        get
        {
            return _database.GetCollection<User>(_settings.UsersCollection);
        }
    }

    public IMongoCollection<Serie> SeriesCollection
    {
        get
        {
            return _database.GetCollection<Serie>(_settings.SeriesCollection);
        }
    }

    public IMongoCollection<Character> CharactersCollection
    {
        get
        {
            return _database.GetCollection<Character>(_settings.CharactersCollection);
        }
    }

    public IMongoCollection<SerieTag> TagsCollection
    {
        get
        {
            return _database.GetCollection<SerieTag>(_settings.TagsCollection);
        }
    }

    public IMongoCollection<CardPack> CardPacksCollection
    {
        get
        {
            return _database.GetCollection<CardPack>(_settings.CardPacksCollection);
        }
    }

    // public IMongoCollection<Card> CardsCollection
    // {
    //     get
    //     {
    //         return _database.GetCollection<Card>(_settings.CardsCollection);
    //     }
    // }

}
