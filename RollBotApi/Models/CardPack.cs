using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace RollBotApi.Models
{
    public enum PackType
    {
        Normal = 0,
        Jumbo = 1,
        Huge = 2
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic
    }

    public class CardPack
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public PackType PackType { get; set; } = PackType.Normal;
        public int Price { get; set; } = 5;
        public int CardCount { get; set; } = 3;
        public Rarity Rarity { get; set; } = Rarity.Common;
        [BsonRepresentation(BsonType.String)]
        public string DiscordId { get; set; } = string.Empty;

        private static readonly Dictionary<Rarity, double> RarityChances = new Dictionary<Rarity, double>
        {
            { Rarity.Common, 0.45 },       // Decreased significantly
            { Rarity.Uncommon, 0.3 },      // Increased to provide a balanced step-up
            { Rarity.Rare, 0.15 },         // Increased to reflect mid-level rarity
            { Rarity.Epic, 0.05 },         // Increased slightly
            { Rarity.Legendary, 0.035 },   // Increased to balance overall chance
            { Rarity.Mythic, 0.015 }       // Increased to give a very rare but attainable chance
        };


        public CardPack(PackType packType = PackType.Normal)
        {
            PackType = packType;
            InitializePackProperties();
            Rarity = GetRandomRarity();
        }

        private void InitializePackProperties()
        {
            switch (PackType)
            {
                case PackType.Normal:
                    Price = 5;
                    CardCount = 3;
                    break;
                case PackType.Jumbo:
                    Price = 10;
                    CardCount = 7;
                    break;
                case PackType.Huge:
                    Price = 20;
                    CardCount = 15;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Rarity GetRandomRarity()
        {
            var random = new Random();
            double roll = random.NextDouble();
            double cumulative = 0.0;

            foreach (var rarityChance in RarityChances)
            {
                cumulative += rarityChance.Value;
                if (roll < cumulative)
                {
                    return rarityChance.Key;
                }
            }

            return Rarity.Common; // Default to common if something goes wrong
        }
    }
}