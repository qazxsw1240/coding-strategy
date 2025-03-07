using CodingStrategy.Entities.Runtime.Abnormality;

using NUnit.Framework;

namespace CodingStrategy.Tests
{
    public class AbnormalityLoaderTests
    {
        [Test]
        public void TestAbnormalityLoader()
        {
            AbnormalityProfile abnormality = AbnormalityLoader.Load("Adware");
            Assert.AreEqual(abnormality.Name, "애드웨어");
        }
    }
}
