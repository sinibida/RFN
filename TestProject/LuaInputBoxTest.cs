using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLua;
using Rfn.App;
using Rfn.App.Commands;
using Rfn.App.InputBoxes;

namespace TestProject
{
    [TestClass]
    public class LuaInputBoxTest
    {
        [TestMethod]
        [TestCategory("TDD")]
        public void LuaSimpleInputBoxTest()
        {
            var box = new LuaInputBox("lua")
            {
                ScriptText = "return 0.8"
            };
            Assert.AreEqual(0.8, box.GetProbability(""));
            // Exceptions doesn't exist in lua
            //Assert.ThrowsException<NLua.Exceptions.LuaException>(
            //    () => state.DoString("inf = 1 / 0.0"));
        }

        [TestMethod]
        [TestCategory("TDD")]
        public void LuaFuncInputBoxTest()
        {
            var box = new LuaInputBox("lua")
            {
                ScriptText = "return math.min(string.len(input) / 10.0, 1.0)"
            };

            Assert.AreEqual(0.2, box.GetProbability("he"));
            Assert.AreEqual(0.5, box.GetProbability("Boom!"));
            Assert.AreEqual(1.0, box.GetProbability("Damn Long Input"));
            // Exceptions doesn't exist in lua
            //Assert.ThrowsException<NLua.Exceptions.LuaException>(
            //    () => state.DoString("inf = 1 / 0.0"));
        }
    }
}