using Microsoft.VisualStudio.TestTools.UnitTesting;
using BaseAsp.DAL.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseAsp.DataEntitied;
using BaseAsp.DataEntitied.Entities;
using BaseAsp.DAL.Abstract;

namespace BaseAsp.DAL.Concrete.Tests
{
    [TestClass()]
    public class AccountTypeRepositoryTests
    {
        private static BaseAppEntities  _dbContext;      
        private static AccountType _testAccountType;

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            _dbContext = new BaseAppEntities();
            _testAccountType = _dbContext.Set<AccountType>().Add(new AccountType { ID =0 , Description = "Testing account type name" });
            _dbContext.SaveChanges();
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {           
            _dbContext.AccountTypes.Remove(_testAccountType);
            _dbContext.SaveChanges();
        }

        [TestMethod()]
        public void AccountTypeRepositoryTest()
        {
            Assert.Fail();
        }

        [TestInitialize()]
        public void MyTestInitialize()
        {
        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
        }

        [TestMethod()]    
        public void AddAccountsTypeTest()
        {            
            IDbContext dbContextInterface = new BaseAppEntities(); 
            AccountTypeRepository target = new AccountTypeRepository(dbContextInterface);


            AccountType currentAccountType = target.Find(i => i.ID == _testAccountType.ID, false).FirstOrDefault();

            Assert.AreEqual(_testAccountType.Description, currentAccountType.Description);
        }

    }
}