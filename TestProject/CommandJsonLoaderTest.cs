using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rfn.App.Commands;

namespace TestProject
{
    [TestClass]
    public class CommandJsonLoaderTest
    {
        private const string JsonText = @"[
{
    ""name"": ""searchGoogle"",
    ""alias"": [""google"", ""g"", ""__sentence"", ""ㅎ""],
    ""type"": ""openUri"",
    ""properties"": {
        ""uriFormat"": ""https://www.google.com/search?q={0}""
    }
},
{
    ""name"": ""searchEnDict"",
    ""alias"": [""endict"", ""e"", ""ㄷ""],
    ""type"": ""openUri"",
    ""properties"": {
        ""uriFormat"": ""https://en.dict.naver.com/#/search?query={0}""
    }
},
{
    ""name"": ""searchEnKoDict"",
    ""alias"": [""enkodict"", ""ek"", ""__eword"", ""다""],
    ""type"": ""openUri"",
    ""properties"": {
        ""uriFormat"": ""https://en.dict.naver.com/#/search?query={0}""
    }
},
{
    ""name"": ""searchKoKoDict"",
    ""alias"": [""enkodict"", ""kk"", ""__kword"", """"],
    ""type"": ""openUri"",
    ""properties"": {
        ""uriFormat"": ""https://ko.dict.naver.com/#/search?query={0}""
    }
},
{
    ""name"": ""quit"",
    ""alias"": [""quit"", ""q""],
    ""type"": ""tryQuit""
}
]";

        [TestMethod]
        [TestCategory("Validate")]
        public void JsonLoadingTest()
        {
            var loader = new CommandJsonLoader();

            var cmds = loader.JsonStringToCommands(JsonText).ToList();
            Assert.AreEqual(5, cmds.Count);

            var searchGoogle = cmds.First(x => x.Name == "searchGoogle");
            Assert.IsNotNull(searchGoogle);
            Assert.IsInstanceOfType(searchGoogle, typeof(OpenUriCommand));
            var searchGoogleCmd = (OpenUriCommand)searchGoogle;
            Assert.AreEqual("https://www.google.com/search?q={0}", searchGoogleCmd.Properties.UriFormat);
            Assert.AreEqual(-1, searchGoogleCmd.Properties.UriFormatArgCount);

            var quit = cmds.First(x => x.Name == "quit");
            Assert.IsNotNull(quit);
            Assert.IsInstanceOfType(quit, typeof(TryQuitCommand));
        }
    }
}