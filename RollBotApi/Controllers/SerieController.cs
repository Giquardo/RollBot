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
public class SerieController : ControllerBase
{
    private readonly ILoggingService _loggingService;
    private readonly ICharacterSerieService _characterSerieService;
    private readonly IMapper _mapper;

    public SerieController(ILoggingService loggingService, ICharacterSerieService characterSerieService, IMapper mapper)
    {
        _loggingService = loggingService;
        _characterSerieService = characterSerieService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetSeriesAsync()
    {
        try
        {
            _loggingService.LogInformation("Serie Controller: Getting all series");
            var series = await _characterSerieService.GetSeriesAsync();
            var seriesDto = _mapper.Map<List<DisplaySerieDto>>(series);

            foreach (var serieDto in seriesDto)
            {
                var serie = series.First(s => s.Id.ToString() == serieDto.Id);
                serieDto.Characters = await _characterSerieService.GetCharacterNamesByIdsAsync(serie.CharacterIds.Select(id => id.ToString()).ToList());
                serieDto.Tags = await _characterSerieService.GetTagNamesByIdsAsync(serie.TagIds.Select(id => id.ToString()).ToList());
            }

            return Ok(seriesDto);
        }
        catch (Exception ex)
        {
            _loggingService.LogError($"Serie Controller: Error getting all series: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetSerieByNameAsync(string name)
    {
        _loggingService.LogInformation($"Serie Controller: Getting serie with name {name}");
        var serie = await _characterSerieService.GetSerieByNameAsync(name);
        var serieDto = _mapper.Map<DisplaySerieDto>(serie);
        return Ok(serieDto);
    }

    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetSerieByIdAsync(string id)
    {
        _loggingService.LogInformation($"Serie Controller: Getting serie with id {id}");
        var serie = await _characterSerieService.GetSerieByIdAsync(id);
        var serieDto = _mapper.Map<DisplaySerieDto>(serie);
        return Ok(serieDto);
    }

    [HttpPost]
    public async Task<Serie> CreateSerie([FromBody] CreateSerieDto request)
    {
        _loggingService.LogInformation($"Serie Controller: Creating serie with name {request.Name}");

        var serie = _mapper.Map<Serie>(request);

        return await _characterSerieService.CreateSerieAsync(serie, request.Tags);
    }

    [HttpPost("batch")]
    public async Task<IEnumerable<Serie>> CreateSeriesBatch([FromBody] CreateSeriesBatchDto request)
    {
        _loggingService.LogInformation("Serie Controller: Creating batch of series");

        var series = new List<Serie>();

        foreach (var serieDto in request.Series)
        {
            var serie = _mapper.Map<Serie>(serieDto);
            var createdSerie = await _characterSerieService.CreateSerieAsync(serie, serieDto.Tags);
            series.Add(createdSerie);
        }
        return series;
    }


    [HttpPut("id/{id}")]
    public async Task UpdateSerieAsync(string id, [FromBody] Serie serie)
    {
        _loggingService.LogInformation($"Serie Controller: Updating serie with id {id}");
        await _characterSerieService.UpdateSerieAsync(id, serie);
    }

    [HttpDelete("id/{id}")]
    public async Task<IActionResult> DeleteSerieAsync(string id)
    {
        _loggingService.LogInformation($"Serie Controller: Deleting serie with id {id}");
        await _characterSerieService.DeleteSerieAsync(id);
        return NoContent();
    }
}