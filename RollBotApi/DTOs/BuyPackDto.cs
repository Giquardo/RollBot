using RollBotApi.Models;
using System.Text.Json.Serialization;

namespace RollBotApi.DTOs
{
    public class BuyPackDto
    {
        public string DiscordId { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PackType PackType { get; set; } = PackType.Normal;
    }
}