using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RollBotApi.Models;

namespace RollBotApi.Models;
public class User
{
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    [BsonRepresentation(BsonType.String)]
    public string DiscordId { get; set; } = string.Empty;

    public List<Card> Cards { get; set; } = new List<Card>();
    //public List<Pack> Packs { get; set; } = new List<Pack>();
}
