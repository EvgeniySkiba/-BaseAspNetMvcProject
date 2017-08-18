using BaseApp.BusinessLogic.Abstract;
using BaseApp.BusinessLogic.Concrete;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.BusinessLogic
{
    public class BusinessLogicModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ClientBusinessLogicInterface>().To<ClientBusinessLogic>();
        }
    }
}
