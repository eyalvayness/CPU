using NUnit.Framework;

namespace CPUTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //Assert.Fail();
            Assert.Pass();
        }

        [Test]
        public void Test2()
        {
            Assert.Fail();
            //Assert.Pass();
        }
    }
}