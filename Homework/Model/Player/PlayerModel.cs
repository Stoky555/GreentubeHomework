using MongoDB.Bson.Serialization.Attributes;

namespace Homework.Model.Player
{
    public class PlayerModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid Guid { get; set; }
        [BsonElement("balance")]
        public decimal Balance { get; set; }
    }
}
