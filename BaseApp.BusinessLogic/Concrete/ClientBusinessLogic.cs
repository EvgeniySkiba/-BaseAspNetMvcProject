using BaseApp.BusinessLogic.Abstract;
using BaseAsp.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.BusinessLogic.Concrete
{
    public class ClientBusinessLogic : GenericBusinessLogic, ClientBusinessLogicInterface
    {
        private readonly IAccountTypeRepository _accountRepository;


        public ClientBusinessLogic(IAccountTypeRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
    }
}
