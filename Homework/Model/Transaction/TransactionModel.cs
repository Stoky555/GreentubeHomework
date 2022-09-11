using Homework.Model.Transaction.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Homework.Model.Transaction
{
    public class TransactionModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid Id { get; set; }
        [BsonElement("playerGuid")]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid PlayerGuid { get; set; }
        [BsonElement("transactionType")]
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public TransactionType TransactionType { get; set; }
        [BsonElement("amount")]
        public decimal Amount { get; set; }
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
        [BsonElement("transactionResult")]
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public TransactionResult TransactionResult { get; set; }
        [BsonElement("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
