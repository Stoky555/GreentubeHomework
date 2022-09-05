using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Homework.Model.Transaction
{
    public enum TransactionType
    {
        [BsonRepresentation(BsonType.String)]
        Win,
        [BsonRepresentation(BsonType.String)]
        Deposit,
        [BsonRepresentation(BsonType.String)]
        Stake
    }
}
