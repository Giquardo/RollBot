using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RollBotApi.Models;

public class SerieTag
{
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    [BsonRepresentation(BsonType.ObjectId)]
    public string Name { get; set; } = string.Empty;
}
