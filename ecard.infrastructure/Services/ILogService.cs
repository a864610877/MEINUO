using Ecard.Models;
using Ecard.Infrastructure;

namespace Ecard.Services
{
    public interface ILogService
    {
        QueryObject<Log> Query(LogRequest request);
        void Create(Log item);
        Log GetById(int id);
        void Update(Log item);
        void Delete(Log item);
        DataTables<Log> NewQuery(LogRequest request);
    }
}