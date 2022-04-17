using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rfn.App;
using Rfn.App.Commands;

namespace TestProject
{
    [TestClass]
    public class UriCommandTest
    {
        [TestMethod]
        [TestCategory("Validate")]
        public void GetFormatArgCountTest()
        {
            Assert.AreEqual(0, OpenUriCommand.GetFormatArgCount("Hello!"));
            Assert.AreEqual(1, OpenUriCommand.GetFormatArgCount("{0}"));
            Assert.AreEqual(2, OpenUriCommand.GetFormatArgCount("{1} is {0}"));
            Assert.AreEqual(4, OpenUriCommand.GetFormatArgCount("{3} is {1} and {0}"));
            Assert.AreEqual(0, OpenUriCommand.GetFormatArgCount("{{0}}"));
        }

        [TestMethod]
        [TestCategory("Validate")]
        public void PopulateArgsTest()
        {
            Assert.IsTrue(Array.Empty<string>().SequenceEqual(
                OpenUriCommand.PopulateArgs(Array.Empty<string>(), 0)));
            Assert.IsTrue(new[] {"Hi!"}.SequenceEqual(
                OpenUriCommand.PopulateArgs(new[] {"Hi!"}, 1)));
            Assert.IsTrue(new[] {"Hello", "World"}.SequenceEqual(
                OpenUriCommand.PopulateArgs(new[] {"Hello", "World"}, 2)));
            Assert.IsTrue(new[] { "Hello World" }.SequenceEqual(
                OpenUriCommand.PopulateArgs(new[] { "Hello", "World" }, 1)));
            Assert.IsTrue(new[] { "Nice", "to meet you" }.SequenceEqual(
                OpenUriCommand.PopulateArgs(new[] { "Nice", "to", "meet", "you" }, 2)));
            Assert.ThrowsException<RfnCommandExecutionException>(
                () => OpenUriCommand.PopulateArgs(new[] {"not", "enough", "args"}, 4));
        }
    }
}