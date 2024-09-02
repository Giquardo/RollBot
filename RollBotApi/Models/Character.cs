using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RollBotApi.Models;

public class Character
{
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    [BsonRepresentation(BsonType.String)]
    public string Name { get; set; } = string.Empty;
    public ObjectId SerieId { get; set; } = ObjectId.Empty;
}
