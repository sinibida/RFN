using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rfn.App.InputBoxes;

namespace TestProject
{
    [TestClass]
    public class InputBoxesTest
    {
		//Duh
        [TestMethod]
        [TestCategory("TDD")]
        public void KoreanInputBoxTest()
        {
            var box = new KoreanWordInputBox();
            Assert.AreEqual(box.GetKey(), "__kword");
            string value;

            value = "English";
            Assert.AreEqual(0.0, box.GetProbability(value));
            value = "한글";
            Assert.AreEqual(1.0, box.GetProbability(value));
            value = "한E";
            Assert.AreEqual(0.5, box.GetProbability(value), 0.01);
            value = "Not a word";
            Assert.AreEqual(0.0, box.GetProbability(value));
            value = "";
            Assert.ThrowsException<InputValueEmptyException>(() => box.GetProbability(value));
        }

        [TestMethod]
        [TestCategory("TDD")]
        public void EnglishInputBoxTest()
        {
            var box = new EnglishWordInputBox();
            Assert.AreEqual("__eword", box.GetKey());
            string value;

            value = "English";
            Assert.AreEqual(1.0, box.GetProbability(value));
            value = "한글";
            Assert.AreEqual(0.0, box.GetProbability(value));
            value = "한E";
            Assert.AreEqual(0.5, box.GetProbability(value), 0.01);
            value = "Not a word";
            Assert.AreEqual(0.0, box.GetProbability(value));
            value = "";
            Assert.ThrowsException<InputValueEmptyException>(() => box.GetProbability(value));
        }

        [TestMethod]
        [TestCategory("TDD")]
        public void SentenceInputBoxTest()
        {
            var box = new SentenceInputBox();
            Assert.AreEqual("__sentence", box.GetKey());
            string value;

            value = "English";
            Assert.AreEqual(0.0, box.GetProbability(value));
            value = "한글";
            Assert.AreEqual(0.0, box.GetProbability(value));
            value = "한E";
            Assert.AreEqual(0.0, box.GetProbability(value));
            value = "Not a word";
            Assert.AreEqual(1.0, box.GetProbability(value));
            value = "";
            Assert.ThrowsException<InputValueEmptyException>(() => box.GetProbability(value));
        }

        [TestMethod]
        [TestCategory("TDD")]
        public void UriInputBoxTest()
        {
            var box = new UriInputBox();
            Assert.AreEqual("__uri", box.GetKey());
            string value;

            value = "not a uri";
            Assert.AreEqual(0.0, box.GetProbability(value));
            value = "never.com";
            Assert.AreEqual(1.0, box.GetProbability(value));
            value = "mytube.org";
            Assert.AreEqual(1.0, box.GetProbability(value));
            value = "twittter.net";
            Assert.AreEqual(1.0, box.GetProbability(value));
            value = "githoob.io";
            Assert.AreEqual(1.0, box.GetProbability(value));
            value = "https://stackoverflow.com/questions/3334138/combine-return-and-switch";
            Assert.AreEqual(1.0, box.GetProbability(value));
            value = "dotCom but with space.com";
            Assert.AreEqual(0.0, box.GetProbability(value));
            value = "";
            Assert.ThrowsException<InputValueEmptyException>(() => box.GetProbability(value));
        }
    }
}
