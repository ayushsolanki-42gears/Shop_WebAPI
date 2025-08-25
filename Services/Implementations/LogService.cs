using MyWebApiApp.Data;
using MyWebApiApp.Services.Interfaces;

namespace MyWebApiApp.Services.Implementations
{
    public class LogService : ILogServices
    {
        private readonly LogRepository _logRepository;

        public LogService(LogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        #region insert log
        public void InsertLog(string eventType, string description, string userId)
        {
            var log = new LogModel
            {
                EventType = eventType,
                Description = description,
                UserID = userId
            };

            _logRepository.InsertLog(log);
        }
        #endregion

        #region get all logs
        public IEnumerable<LogModel> GetAllLogs()
        {
            return _logRepository.GetAllLogs();
        }
        #endregion

        #region get logs by event
        public IEnumerable<LogModel> GetLogsByEvent(string eventType)
        {
            return _logRepository.GetLogsByEvent(eventType);
        }
        #endregion

        #region get logs by user
        public IEnumerable<LogModel> GetLogsByUser(int userId)
        {
            return _logRepository.GetLogsByUser(userId.ToString());
        }
        #endregion

        #region get log by id
        public LogModel? GetLogById(string id)
        {
            return _logRepository.GetLogById(id);
        }
        #endregion

        #region get all event types
        public List<string> GetEventTypes()
        {
            return _logRepository.GetAllEventTypes();
        }
        #endregion
    }
}