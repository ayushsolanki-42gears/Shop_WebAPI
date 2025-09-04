using System.Threading.Tasks;
using MongoDB.Driver;

namespace MyWebApiApp.Data
{
    public class LogRepository
    {
        private readonly IMongoCollection<LogModel> _logs;

        public LogRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            var database = client.GetDatabase("ShopLogs");
            _logs = database.GetCollection<LogModel>("Logs");
        }

        #region insert log
        public void InsertLog(LogModel log)
        {
            _logs.InsertOne(log);
        }
        #endregion

        #region get all logs
        public IEnumerable<LogModel> GetAllLogs()
        {
            return _logs.Find(_ => true).ToList();
        }
        #endregion

        #region get all logs by user
        public IEnumerable<LogModel> GetLogsByUser(string userId)
        {
            return _logs.Find(l => l.UserID == userId).ToList();
        }
        #endregion

        #region get logs by event type
        public IEnumerable<LogModel> GetLogsByEvent(string eventType)
        {
            return _logs.Find(l => l.EventType == eventType).ToList();
        }
        #endregion

        #region get log by id
        public LogModel? GetLogById(string id)
        {
            return _logs.Find(l => l.Id == id).FirstOrDefault();
        }
        #endregion

        #region Get All Event Types
        public List<string> GetAllEventTypes()
        {
            return _logs
                .Distinct<string>("EventType",FilterDefinition<LogModel>.Empty)
                .ToList();
        }
        #endregion
    }
}