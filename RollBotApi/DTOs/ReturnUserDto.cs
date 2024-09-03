using RollBotApi.Models;

namespace RollBotApi.DTOs
{
    public class ReturnUserDto
    {
        public string DiscordId { get; set; } = string.Empty;
        public int Balance { get; set; } = 0;
        public List<CardPack> CardPacks { get; set; } = new List<CardPack>();
    }
}