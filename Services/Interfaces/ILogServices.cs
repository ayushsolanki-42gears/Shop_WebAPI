namespace MyWebApiApp.Services.Interfaces
{
    public interface ILogServices
    {
        IEnumerable<LogModel> GetAllLogs();
        IEnumerable<LogModel> GetLogsByUser(int userId);
        IEnumerable<LogModel> GetLogsByEvent(string eventType);
        LogModel? GetLogById(string id);
        void InsertLog(string eventType, string description, string userId);

        List<string> GetEventTypes();
    }
}