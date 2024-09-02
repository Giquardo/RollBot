using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace RollBotApi.Models
{
    public class Serie
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<ObjectId> CharacterIds { get; set; } = new List<ObjectId>();

        public List<ObjectId> TagIds { get; set; } = new List<ObjectId>();
    }
}