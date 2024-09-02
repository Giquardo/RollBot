using MongoDB.Bson;

namespace RollBotApi.DTOs;
public class CreateCharacterDto
{
    public string Name { get; set; } = string.Empty;
    public string SerieId { get; set; } = string.Empty;
}
