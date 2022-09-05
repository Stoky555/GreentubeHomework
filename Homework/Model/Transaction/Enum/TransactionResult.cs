using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Homework.Model.Transaction.Enum
{
    public enum TransactionResult
    {
        [BsonRepresentation(BsonType.String)]
        Failed,
        [BsonRepresentation(BsonType.String)]
        Succeeded
    }
}
