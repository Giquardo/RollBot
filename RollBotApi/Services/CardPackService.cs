using System.Collections.Generic;
using System.Threading.Tasks;
using RollBotApi.Models;
using RollBotApi.Repositories;
using RollBotApi.Services;

namespace RollBotApi.Services;

public interface ICardPackService
{
    Task<CardPack> BuyPackAsync(string discordId, PackType packType);
    Task<List<CardPack>> GetCardPacksAsync(string discordId);
}

public class CardPackService : ICardPackService
{
    private readonly ICardPackRepository _cardPackRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILoggingService _loggingService;

    public CardPackService(ICardPackRepository cardPackRepository, ILoggingService loggingService, IUserRepository userRepository)
    {
        _cardPackRepository = cardPackRepository;
        _loggingService = loggingService;
        _userRepository = userRepository;
    }

    public async Task<CardPack> BuyPackAsync(string discordId, PackType packType)
    {
        _loggingService.LogInformation($"CardPack Service: Buying pack for user with Discord id {discordId}");

        var user = await _userRepository.GetUser(discordId);
        var cardPack = new CardPack(packType);

        if (user.Balance < cardPack.Price)
        {
            _loggingService.LogError("CardPack Service: User does not have enough balance to buy pack");
            throw new System.Exception("User does not have enough balance to buy pack");
        }
        cardPack.DiscordId = user.DiscordId;

        await _cardPackRepository.CreateCardPack(cardPack);

        user.Balance -= cardPack.Price;
        user.CardPacks.Add(cardPack);

        await _userRepository.UpdateUser(discordId, user);

        return cardPack;
    }

    public async Task<List<CardPack>> GetCardPacksAsync(string discordId)
    {
        _loggingService.LogInformation($"CardPack Service: Getting card packs for user with Discord id {discordId}");
        var user = await _userRepository.GetUser(discordId);
        return user.CardPacks;
    }
}
