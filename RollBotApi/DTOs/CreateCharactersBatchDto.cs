using RollBotApi.DTOs;

public class CreateCharactersBatchDto
{
    public List<CreateCharacterDto> Characters { get; set; } = new List<CreateCharacterDto>();
}