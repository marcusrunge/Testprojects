using EchoComponent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestEcho
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestEchoClass()
        {
            var echoClass = new EchoClass();
            Assert.AreEqual(0, echoClass.CreateAndTestSocket());
        }
    }
}
