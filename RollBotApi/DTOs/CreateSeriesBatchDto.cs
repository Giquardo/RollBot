using System.Collections.Generic;
using RollBotApi.DTOs;

namespace RollBotApi.DTOs;
public class CreateSeriesBatchDto
{
    public List<CreateSerieDto> Series { get; set; } = new List<CreateSerieDto>();
}
