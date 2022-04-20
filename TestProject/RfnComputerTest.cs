using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rfn.App;
using Rfn.App.Commands;
using Rfn.App.InputBoxes;
using Rfn.App.InputBoxes.Lua;

namespace TestProject
{
    [TestClass]
    public class RfnComputerTest
    {
        private const string JsonText = @"[
{
    ""name"": ""searchGoogle"",
    ""alias"": [""google"", ""g"", ""__sentence"", ""ㅎ""],
    ""type"": ""openUri"",
    ""properties"": {
        ""urlFormat"": ""https://www.google.com/search?q={0}""
    }
},
{
    ""name"": ""searchEnDict"",
    ""alias"": [""endict"", ""e"", ""ㄷ""],
    ""type"": ""openUri"",
    ""properties"": {
        ""urlFormat"": ""https://en.dict.naver.com/#/search?query={0}""
    }
},
{
    ""name"": ""searchEnKoDict"",
    ""alias"": [""enkodict"", ""ek"", ""__eword"", ""다""],
    ""type"": ""openUri"",
    ""properties"": {
        ""urlFormat"": ""https://en.dict.naver.com/#/search?query={0}""
    }
},
{
    ""name"": ""searchKoKoDict"",
    ""alias"": [""enkodict"", ""kk"", ""__kword""],
    ""type"": ""openUri"",
    ""properties"": {
        ""urlFormat"": ""https://ko.dict.naver.com/#/search?query={0}""
    }
},
{
    ""name"": ""openCLC"",
    ""alias"": [ ""__clc"" ],
    ""type"": ""openUri"",
    ""properties"": {
      ""uriFormatArgCount"": 0,
      ""uriFormat"": ""https://clc.chosun.ac.kr/ilos/main/main_form.acl""
    }
},
{
    ""name"": ""quit"",
    ""alias"": [""quit"", ""q""],
    ""type"": ""tryQuit""
}
]";

        public RfnComputer Computer { get; }

        public RfnComputerTest()
        {
            Computer = new RfnComputer();

            Computer.InputBoxes.Add(new EquationInputBox());
            Computer.InputBoxes.Add(new UriInputBox());
            Computer.InputBoxes.Add(new SentenceInputBox());
            Computer.InputBoxes.Add(new EnglishWordInputBox());
            Computer.InputBoxes.Add(new KoreanWordInputBox());
            Computer.InputBoxes.Add(new LuaInputBox()
            {
                Key = "__clc",
                ScriptText = "if (input == \"clc\") then return 1.0 else return 0.0 end",
                Name = "CLC",
                Order = 10000
            });

            var loader = new CommandJsonLoader(new Dictionary<string, Type>()
            {
                {"openUri", typeof(OpenUriCommand)},
                {"tryQuit", typeof(TryQuitCommand)},
                {"reloadConfigs", typeof(ReloadConfigsCommand)},
            });
            Computer.Commands = new RfnCommandList( loader.JsonStringToCommands(JsonText));
        }

        [TestMethod]
        [TestCategory("TDD")]
        public void RawInputTest()
        {
            Assert.AreEqual(
                "__kword",
                Computer.GetTagFromBody("한글"));
            Assert.AreEqual(
                "__eword",
                Computer.GetTagFromBody("English"));
            Assert.AreEqual(
                "__sentence",
                Computer.GetTagFromBody("This is a sentence"));
            Assert.AreEqual(
                "__eword",
                Computer.GetTagFromBody("ThisWordMostlyConsistsOf영어"));
            Assert.AreEqual(
                "__eq",
                Computer.GetTagFromBody("sinx"));
            Assert.AreEqual(
                "__uri",
                Computer.GetTagFromBody("naver.com"));
            Assert.AreEqual(
                "__uri",
                Computer.GetTagFromBody("https://www.youtube.com/watch?v=QPkgYl1e2DI"));
        }

        [TestMethod]
        [TestCategory("TDD")]
        public void ComputeBodyTest()
        {
            var dat = Computer.Compute("한글");
            Assert.AreEqual(
                "searchKoKoDict",
                dat.Command.Name);
            Assert.IsTrue(dat.Args.SequenceEqual(new[] { "한글" }));

            dat = Computer.Compute("hello");
            Assert.AreEqual(
                "searchEnKoDict",
                dat.Command.Name);
            Assert.IsTrue(dat.Args.SequenceEqual(new[] { "hello" }));

            dat = Computer.Compute("hello world");
            Assert.AreEqual(
                "searchGoogle",
                dat.Command.Name);
            Assert.IsTrue(dat.Args.SequenceEqual(new[] { "hello", "world" }));
        }

        [TestMethod]
        [TestCategory("TDD")]
        public void ComputeTagAndBodyTest()
        {
            var dat = Computer.Compute("g;apple");
            Assert.AreEqual(
                "searchGoogle",
                dat.Command.Name);
            Assert.IsTrue(dat.Args.SequenceEqual(new[] { "apple" }));

            dat = Computer.Compute("q;");
            Assert.AreEqual(
                "quit",
                dat.Command.Name);
            Assert.IsTrue(dat.Args.SequenceEqual(Array.Empty<string>()));

            dat = Computer.Compute("e;hello world");
            Assert.AreEqual(
                "searchEnDict",
                dat.Command.Name);
            Assert.IsTrue(dat.Args.SequenceEqual(new[] { "hello", "world" }));

            dat = Computer.Compute(";q");
            Assert.AreEqual(
                "quit",
                dat.Command.Name);
            Assert.IsTrue(dat.Args.SequenceEqual(Array.Empty<string>()));

            dat = Computer.Compute("lol;g");
            Assert.AreEqual(
                "searchGoogle",
                dat.Command.Name);
            Assert.IsTrue(dat.Args.SequenceEqual(new[] { "lol" }));

            dat = Computer.Compute("ㅎ;데미안");
            Assert.AreEqual(
                "searchGoogle",
                dat.Command.Name);
            Assert.IsTrue(dat.Args.SequenceEqual(new[] { "데미안" }));
        }

        [TestMethod]
        [TestCategory("Validate")]
        public void LuaInputBoxTest()
        {
            var dat = Computer.Compute("clc");
            Assert.AreEqual(
                "openCLC",
                dat.Command.Name);
            Assert.IsTrue(dat.Args.SequenceEqual(new[] {"clc"}));
        }
    }
}