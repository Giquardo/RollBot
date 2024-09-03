using MongoDB.Driver;
using RollBotApi.Models;
using RollBotApi.Services;
using RollBotApi.DataContext;


namespace RollBotApi.Repositories;

public interface ICardPackRepository
{
    Task<CardPack> CreateCardPack(CardPack cardPack);
}

public class CardPackRepository : ICardPackRepository
{
    private readonly IMongoCollection<CardPack> _cardPacksCollection;
    private readonly ILoggingService _loggingService;

    public CardPackRepository(IMongoContext context, ILoggingService loggingService)
    {
        _loggingService = loggingService;
        _cardPacksCollection = context.CardPacksCollection;
    }

    public async Task<CardPack> CreateCardPack(CardPack cardPack)
    {
        _loggingService.LogInformation("CardPack Repository: Creating card pack");
        await _cardPacksCollection.InsertOneAsync(cardPack);
        return cardPack;
    }

}