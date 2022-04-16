using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rfn.App;

namespace TestProject
{
    [TestClass]
    public class RfnComputerTest
    {
        public RfnComputer Computer { get; }

        public RfnComputerTest()
        {
            Computer = new RfnComputer();
        }

        [TestMethod]
        public void SimpleSearchEnKoDictTest()
        {
            Assert.AreEqual(
                new RfnExecuteData(JobType.SearchEnKoDict, "Configuration"),
                Computer.Compute("Configuration"));
        }

        [TestMethod]
        public void SimpleSearchWebTest()
        {
            Assert.AreEqual(
                new RfnExecuteData(JobType.SearchWeb, "What is love?"),
                Computer.Compute("What is love?"));
        }

        [TestMethod]
        public void SimpleSearchKoKoDictTest()
        {
            Assert.AreEqual(
                new RfnExecuteData(JobType.SearchKoKoDict, "설정"),
                Computer.Compute("설정"));
        }

        [TestMethod]
        public void SimpleSearchKoEnDictTest()
        {
            Assert.AreEqual(
                new RfnExecuteData(JobType.SearchKoEnDict, "설정"),
                Computer.Compute("설정;ㄷ"));
            Assert.AreEqual(
                new RfnExecuteData(JobType.SearchKoEnDict, "설정"),
                Computer.Compute("설정;e"));
            Assert.AreEqual(
                new RfnExecuteData(JobType.SearchKoEnDict, "설정"),
                Computer.Compute("ㄷ;설정"));
            Assert.AreEqual(
                new RfnExecuteData(JobType.SearchKoEnDict, "설정"),
                Computer.Compute("e;설정"));
        }

        [TestMethod]
        public void SimpleSearchEnEnDictTest()
        {
            Assert.AreEqual(
                new RfnExecuteData(JobType.SearchEnEnDict, "Configuration"),
                Computer.Compute("Configuration;ㄷ"));
            Assert.AreEqual(
                new RfnExecuteData(JobType.SearchEnEnDict, "Configuration"),
                Computer.Compute("Configuration;e"));
            Assert.AreEqual(
                new RfnExecuteData(JobType.SearchEnEnDict, "Configuration"),
                Computer.Compute("e;Configuration"));
            Assert.AreEqual(
                new RfnExecuteData(JobType.SearchEnEnDict, "Configuration"),
                Computer.Compute("ㄷ;Configuration"));
        }

        [TestMethod]
        public void SimpleSearchWolframAlphaTest()
        {
            Assert.AreEqual(
                new RfnExecuteData(JobType.WolframAlpha, "Derivative of sinx"),
                Computer.Compute("Derivative of sinx"));
            Assert.AreEqual(
                new RfnExecuteData(JobType.WolframAlpha, "Not an equation"),
                Computer.Compute("Not an equation;w"));
            Assert.AreEqual(
                new RfnExecuteData(JobType.WolframAlpha, "derivative of arcsinx"),
                Computer.Compute("derivative of arcsinx"));
        }

        [TestMethod]
        public void KoreanEnglishProbTest()
        {
            Assert.IsTrue(RfnComputer.IsKoreanNotEnglish("안녕하세요. 이 글은 한국어입니다."));
            Assert.IsFalse(RfnComputer.IsKoreanNotEnglish("Hello. This sentence is written in English."));
            Assert.IsFalse(RfnComputer.IsKoreanNotEnglish("안녕하세요. This sentence is mostly written in English."));
            Assert.IsTrue(RfnComputer.IsKoreanNotEnglish("Hello. 이 문장은 거의 한국어로 입력되어있습니다."));
        }

        [TestMethod]
        public void EquationProbTest()
        {
            Assert.IsTrue(RfnComputer.IsEquation("3+5"));
            Assert.IsTrue(RfnComputer.IsEquation("  9 +   2"));
            Assert.IsTrue(RfnComputer.IsEquation("x^4+4*x^2+x-10"));
            Assert.IsTrue(RfnComputer.IsEquation("+-/*()^fjrks"));
            Assert.IsFalse(RfnComputer.IsEquation("3+5 adsfeifjrks"));
            Assert.IsTrue(RfnComputer.IsEquation("Derivative of 3x"));
            Assert.IsTrue(RfnComputer.IsEquation("sinx + cosx + tanx"));
            Assert.IsTrue(RfnComputer.IsEquation("Derivative of sinx"));
            Assert.IsTrue(RfnComputer.IsEquation("Derivative of e^x"));
        }

        [TestMethod]
        public void OpenWebSiteTest()
        {
            Assert.AreEqual(
                new RfnExecuteData(JobType.OpenWebSite, "http://naver.com/"),
                Computer.Compute("naver.com"));
            Assert.AreNotEqual(
                new RfnExecuteData(JobType.OpenWebSite, "notweb"),
                Computer.Compute("notweb"));
            Assert.AreEqual(
                new RfnExecuteData(JobType.OpenWebSite, "http://www.naver.com/"),
                Computer.Compute("www.naver.com"));
        }
    }
}