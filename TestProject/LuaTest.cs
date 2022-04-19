using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLua;
using Rfn.App;
using Rfn.App.Commands;

namespace TestProject
{
    [TestClass]
    public class LuaTest
    {
        [TestMethod]
        [TestCategory("Scratch")]
        public void LuaExceptionTest()
        {
            var state = new Lua();
            var e = Assert.ThrowsException<NLua.Exceptions.LuaScriptException>(
                () => state.DoString("3cheeseBurger"));
            Console.WriteLine("Good");
            // Exceptions doesn't exist in lua
            //Assert.ThrowsException<NLua.Exceptions.LuaException>(
            //    () => state.DoString("inf = 1 / 0.0"));
        }
    }
}