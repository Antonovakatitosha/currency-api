using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CurrencyApi.Models
{
    public class Currency
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Price")]
        public decimal Price { get; set; }

        [BsonElement("Units")]
        public double Units { get; set; }
    }
}