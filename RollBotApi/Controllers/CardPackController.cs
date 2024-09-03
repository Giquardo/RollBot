using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RollBotApi.Models;
using RollBotApi.Services;
using RollBotApi.Repositories;
using RollBotApi.DataContext;
using RollBotApi.DTOs;
using AutoMapper;

[ApiController]
[Route("api/[controller]")]
public class CardPackController : ControllerBase
{
    private readonly ILoggingService _loggingService;
    private readonly ICardPackService _cardPackService;
    private readonly IMapper _mapper;

    public CardPackController(ILoggingService loggingService, ICardPackService cardPackService, IMapper mapper)
    {
        _loggingService = loggingService;
        _cardPackService = cardPackService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> BuyPackAsync([FromBody] BuyPackDto buyPackDto)
    {
        try
        {
            _loggingService.LogInformation($"CardPack Controller: Buying pack for user with Discord id {buyPackDto.DiscordId}");
            var cardPack = await _cardPackService.BuyPackAsync(buyPackDto.DiscordId, buyPackDto.PackType);
            return Ok(cardPack);
        }
        catch (Exception ex)
        {
            _loggingService.LogError($"CardPack Controller: Error buying pack: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}
