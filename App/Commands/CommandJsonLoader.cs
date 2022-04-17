using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rfn.App.Commands
{
    public class CommandJsonLoader
    {
        public const string ExcCommandNoName =
            "There is a unnamed command. A name is required for a command.";

        public const string ExcCommandNoType =
            "Command '{0}' doesn't have type property.";

        public const string ExcCommandUnknownType =
            "Command '{0}' have unknown type property {1}.";

        public static RfnCommand GetCommandFromElement(JObject commandObj)
        {
            if (!commandObj.ContainsKey("name"))
                throw new CommandJsonLoaderException(ExcCommandNoName);
            var name = commandObj["name"].ToString();

            if (!commandObj.ContainsKey("type"))
                throw new CommandJsonLoaderException(string.Format(ExcCommandNoType, name));
            var type = commandObj["type"].ToString();

            RfnCommand cmd;
            switch (type)
            {
                case "openUri":
                    cmd = new OpenUriCommand();
                    break;
                case "tryQuit":
                    cmd = new TryQuitCommand();
                    break;
                default:
                    throw new CommandJsonLoaderException(
                        string.Format(ExcCommandUnknownType, name, type));
            }

            var propType = cmd.GetPropertyType();
            cmd.SetProperty(commandObj.ContainsKey("properties")
                ? commandObj["properties"].ToObject(propType)
                : propType.IsValueType ? Activator.CreateInstance(propType) : null);
            cmd.Alias = commandObj.ContainsKey("alias")
                ? commandObj["alias"].ToObject<string[]>()
                : new string[] { };
            cmd.Name = name;
            return cmd;
        }

        public IEnumerable<RfnCommand> JsonStringToCommands(string str) =>
            JArrayToCommands(JArray.Parse(str));

        public IEnumerable<RfnCommand> JArrayToCommands(JArray doc) =>
            from JObject commandObj in doc select GetCommandFromElement(commandObj);
    }
}