using Moonlit.Data;

namespace Ecard
{
    public static class DatabaseExtensions
    {
        public static DatabaseInstance OpenInstance(this Database database)
        {
            return new DatabaseInstance(database);
        }
    }
}