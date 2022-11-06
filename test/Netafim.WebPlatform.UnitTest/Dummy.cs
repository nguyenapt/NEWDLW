using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Netafim.WebPlatform.UnitTest
{
    [TestClass]
    public class Dummy
    {
        [TestMethod]
        public void I_can_count_to_10()
        {
            // arrange
            int count = 0;

            // act
            for (int i = 1; i <= 10; i++)
            {
                count++;
                Console.WriteLine(count);
            }

            // assert
            Assert.AreEqual(10, count);
        }
    }
}
