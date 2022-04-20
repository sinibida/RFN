using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Rfn.App.Commands;

namespace Rfn.App.InputBoxes.Lua
{
    public class LuaInputBoxJsonLoader
    {
        public const string ConfigName = "inputboxes.json";

        public const string ExcCommandNoName =
            "There is a unnamed command. A name is required for a command.";

        public const string ExcCommandNoPath =
            "Command '{0}' doesn't have path property.";

        public const string ExcCommandNoKey =
            "Command '{0}' doesn't have key property.";

        public const string ExcCommandUnknownType =
            "Command '{0}' have unknown type property {1}.";

        public LuaInputBoxJsonLoader()
        {
        }

        public LuaInputBox[] LoadInputBoxes(string path)
        {
            var configPath = Path.Combine(path, ConfigName);
            var res =
                from JObject boxInfo in JArray.Parse(File.ReadAllText(configPath, Encoding.UTF8))
                select GetLuaInputBoxFromElement(boxInfo, path);
            return res.ToArray();
        }

        public LuaInputBox GetLuaInputBoxFromElement(JObject boxInfo, string basePath)
        {
            if (!boxInfo.ContainsKey("name"))
                throw new CommandJsonLoaderException(ExcCommandNoName);
            var name = boxInfo["name"].ToString();

            if (!boxInfo.ContainsKey("path"))
                throw new CommandJsonLoaderException(string.Format(ExcCommandNoPath, name));
            var path = boxInfo["path"].ToString();

            if (!boxInfo.ContainsKey("key"))
                throw new CommandJsonLoaderException(string.Format(ExcCommandNoKey, name));
            var key = boxInfo["key"].ToString();

            var order = boxInfo.ContainsKey("order") ? boxInfo["order"].Value<int>() : 10000;

            var box = new LuaInputBox
            {
                Name = name,
                Key = key,
                Order = order,
                ScriptText = File.ReadAllText(Path.Combine(basePath, path))
            };

            return box;
        }
    }
}