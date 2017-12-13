using GameFramework.DataTable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameFramework.UnitTest.DataTable
{
    [TestClass]
    public class DataTableUnitTest
    {
        private IDataTableManager m_DataTableManager = null;

        [TestInitialize]
        public void TestInitialize()
        {
            m_DataTableManager = GameFrameworkEntry.GetModule<IDataTableManager>();
            Assert.IsNotNull(m_DataTableManager);

            m_DataTableManager.SetResourceManager(null);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            m_DataTableManager = null;
            GameFrameworkEntry.Shutdown();
        }

        [TestMethod]
        public void TestMethod()
        {

        }
    }
}
