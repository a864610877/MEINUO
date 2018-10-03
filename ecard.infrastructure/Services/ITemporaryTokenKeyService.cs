using Ecard.Models;

namespace Ecard.Services
{
    public interface ITemporaryTokenKeyService
    {
        QueryObject<TemporaryTokenKey> QueryByUser(int tokenKeyType, string userName);
        void Create(TemporaryTokenKey item);
        TemporaryTokenKey GetById(int id);
        void Update(TemporaryTokenKey item);
        void Delete(TemporaryTokenKey item);
        TemporaryTokenKey GetToken(string token, int tokenType);
    }
}