namespace Homework.Model.Mongo
{
    public class MongoDBSettings
    {
        public string ConnectionURI { get; set; } = null;
        public string DatabaseName { get; set; }
        public string PlayerCollection { get; set; }
        public string TransactionCollection { get; set; }
    }
}
