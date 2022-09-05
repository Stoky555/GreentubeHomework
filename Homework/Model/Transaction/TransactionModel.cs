using Homework.Model.Transaction.Enum;
using MongoDB.Bson.Serialization.Attributes;

namespace Homework.Model.Transaction
{
    public class TransactionModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public Guid Id { get; set; }
        [BsonElement("playerGuid")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public Guid PlayerGuid { get; set; }
        [BsonElement("transactionType")]
        public TransactionType TransactionType { get; set; }
        [BsonElement("amount")]
        public decimal Amount { get; set; }
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
        [BsonElement("transactionResult")]
        public TransactionResult TransactionResult { get; set; }
        [BsonElement("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
