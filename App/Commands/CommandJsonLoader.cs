using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

        public Dictionary<string, Type> CommandsTypes { get; set; }

        public CommandJsonLoader(Dictionary<string, Type> commandsTypes)
        {
            CommandsTypes = commandsTypes;
        }

        public CommandJsonLoader() : this(new Dictionary<string, Type>())
        {
        }

        public RfnCommand[] JsonFileToCommands(string path)
        {
            var json = File.ReadAllText(path, Encoding.UTF8);
            var ret = from JObject obj in JArray.Parse(json)
                select GetCommandFromElement(obj);
            return ret.ToArray();
        }

        public RfnCommand[] JsonStringToCommands(string str) =>
            JArrayToCommands(JArray.Parse(str));

        public RfnCommand[] JArrayToCommands(JArray doc) =>
            (from JObject commandObj in doc select GetCommandFromElement(commandObj)).ToArray();

        public RfnCommand GetCommandFromElement(JObject commandObj)
        {
            if (!commandObj.ContainsKey("name"))
                throw new CommandJsonLoaderException(ExcCommandNoName);
            var name = commandObj["name"].ToString();

            if (!commandObj.ContainsKey("type"))
                throw new CommandJsonLoaderException(string.Format(ExcCommandNoType, name));
            var type = commandObj["type"].ToString();

            RfnCommand cmd;
            if (CommandsTypes.TryGetValue(type, out Type t))
            {
                if (typeof(RfnCommand).IsAssignableFrom(t))
                    cmd = (RfnCommand) Activator.CreateInstance(t);
                else
                    throw new InvalidCastException("CommandTypes have type that is not derived from RfnCommand.");
            }
            else
            {
                throw new CommandJsonLoaderException(
                    string.Format(ExcCommandUnknownType, name, type));
            }

            var propType = cmd.GetPropertyType();
            cmd.SetProperty(commandObj.ContainsKey("properties")
                ? commandObj["properties"].ToObject(propType)
                : propType.IsValueType
                    ? Activator.CreateInstance(propType)
                    : null);
            cmd.Alias = commandObj.ContainsKey("alias")
                ? commandObj["alias"].ToObject<string[]>()
                : new string[] { };
            cmd.Name = name;
            return cmd;
        }
    }
}