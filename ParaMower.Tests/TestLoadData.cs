using System.Diagnostics.SymbolStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParaMower.Sample;

namespace ParaMower.Tests
{
    [TestClass]
    public class TestLoadData
    {
        [TestMethod]
        public void TestLoadInValidData()
        {
            var data = ParamConvertor.Load<SampleSchema>(new[]
            {
                "-m", ModeTag.Offline.ToString(), "-c", "", "-f", AudioFormat.Mp3.ToString(), "-q",
                QualityLevel.Low.ToString()
            });

            Assert.IsFalse(data.IsValid());
            Assert.AreEqual(ModeTag.Offline, data.ModeTag);
            Assert.AreEqual(AudioFormat.Mp3, data.OutputFormat);
            Assert.AreEqual(QualityLevel.Low, data.OutputQuality);
        }

        [TestMethod]
        public void TestLoadValidData()
        {
            var data = ParamConvertor.Load<SampleSchema>(new[]
            {
                "-m", ModeTag.Offline.ToString(), "-c", "config.json", "-f", AudioFormat.Mp3.ToString(), "-q",
                QualityLevel.Low.ToString()
            });

            Assert.IsTrue(data.IsValid());
            Assert.AreEqual(ModeTag.Offline, data.ModeTag);
            Assert.AreEqual(AudioFormat.Mp3, data.OutputFormat);
            Assert.AreEqual(QualityLevel.Low, data.OutputQuality);
        }
    }
}
