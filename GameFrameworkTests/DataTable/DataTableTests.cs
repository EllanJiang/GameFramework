//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameFrameworkTests.DataTable
{
    [TestClass]
    public sealed class DataTableTests
    {
        private IDataTableManager m_DataTableManager = null;

        [TestInitialize]
        public void TestInitialize()
        {
            m_DataTableManager = GameFrameworkEntry.GetModule<IDataTableManager>();
            Assert.IsNotNull(m_DataTableManager);
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
