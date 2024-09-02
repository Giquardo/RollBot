namespace RollBotApi.DTOs
{
    public class CreateSerieDto
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
    }
}