using BaseAsp.DAL.Abstract;
using BaseAsp.DataEntitied;
using BaseAsp.DataEntitied.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAsp.DAL.Concrete
{
    public class AccountTypeRepository : GenericRepository<AccountType>, IAccountTypeRepository
    {
        public AccountTypeRepository(IDbContext dbContextInterface) : base(dbContextInterface)
        {
            Debug.WriteLine("AccountTypeRepository() Hash:" + GetHashCode());
        }
    }
}
