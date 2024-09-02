using RollBotApi.DTOs;

namespace RollBotApi.DTOs;
public class DisplaySerieDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<CharacterDisplayDto> Characters { get; set; } = new List<CharacterDisplayDto>();
    public List<TagDisplayDto> Tags { get; set; } = new List<TagDisplayDto>();
}
