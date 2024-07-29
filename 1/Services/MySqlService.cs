using _1.Interfaces;
using _1.DbModels;

namespace _1.Services
{
    public class MySqlService : IDataContextService
    {
        private Ispr2438IbragimovaDm1Context _dbContext;

        public MySqlService(Ispr2438IbragimovaDm1Context dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
