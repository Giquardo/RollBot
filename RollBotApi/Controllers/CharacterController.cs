using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using RollBotApi.DTOs;
using RollBotApi.Models;
using RollBotApi.Services;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RollBotApi.Controllers
{
    [ApiController]
    [Route("api/characters")]
    public class CharacterController : ControllerBase
    {
        private readonly ILoggingService _loggingService;
        private readonly ICharacterSerieService _characterSerieService;
        private readonly IMapper _mapper;

        public CharacterController(ILoggingService loggingService, ICharacterSerieService characterSerieService, IMapper mapper)
        {
            _loggingService = loggingService;
            _characterSerieService = characterSerieService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<Character>> GetCharactersAsync()
        {
            _loggingService.LogInformation("Character Controller: Getting all characters");
            return await _characterSerieService.GetCharactersAsync();
        }

        [HttpGet("{id}")]
        public async Task<Character> GetCharacterByIdAsync(string id)
        {
            _loggingService.LogInformation($"Character Controller: Getting character with id {id}");
            return await _characterSerieService.GetCharacterByIdAsync(id);
        }

        [HttpPost]
        public async Task<Character> CreateCharacter([FromBody] CreateCharacterDto createCharacterDto)
        {
            _loggingService.LogInformation($"Character Controller: Creating character with name {createCharacterDto.Name}");

            var character = _mapper.Map<Character>(createCharacterDto);
            character.SerieId = new ObjectId(createCharacterDto.SerieId);

            var createdCharacter = await _characterSerieService.CreateCharacterAsync(character);

            // Update the Serie to include the new CharacterId
            await _characterSerieService.AddCharacterToSerieAsync(character.SerieId, createdCharacter.Id);

            return createdCharacter;
        }

        [HttpPost("batch")]
        public async Task<IEnumerable<Character>> CreateCharactersBatch([FromBody] CreateCharactersBatchDto request)
        {
            _loggingService.LogInformation("Character Controller: Creating batch of characters");

            var characters = new List<Character>();

            foreach (var characterDto in request.Characters)
            {
                var character = _mapper.Map<Character>(characterDto);
                character.SerieId = new ObjectId(characterDto.SerieId);
                var createdCharacter = await _characterSerieService.CreateCharacterAsync(character);
                await _characterSerieService.AddCharacterToSerieAsync(character.SerieId, createdCharacter.Id);
                characters.Add(createdCharacter);
            }

            return characters;
        }

        [HttpPut("{id}")]
        public async Task UpdateCharacter(string id, [FromBody] Character request)
        {
            _loggingService.LogInformation($"Character Controller: Updating character with id {id}");

            await _characterSerieService.UpdateCharacterAsync(id, request);
        }

        [HttpDelete("{id}")]
        public async Task DeleteCharacter(string id)
        {
            _loggingService.LogInformation($"Character Controller: Deleting character with id {id}");

            var character = await _characterSerieService.GetCharacterByIdAsync(id);
            if (character != null)
            {
                await _characterSerieService.DeleteCharacterAsync(id);
                await _characterSerieService.RemoveCharacterFromSerieAsync(character.SerieId, character.Id);
            }
        } 
    }
}