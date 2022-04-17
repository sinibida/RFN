using System;
using System.Linq;
using Rfn.App.Commands;

namespace Rfn.App
{
    [Obsolete]
    public enum JobType
    {
        Unknown,
        SearchWeb,
        SearchEnKoDict,
        SearchKoKoDict,
        SearchKoEnDict,
        SearchEnEnDict,
        WolframAlpha,
        Quit,
        OpenWebSite,
        SearchTokiPona
    }

    public class RfnExecuteData
    {
        public RfnCommand Command { get; }
        public string[] Args { get; }

        public RfnExecuteData(RfnCommand cmd, params string[] args)
        {
            Command = cmd;
            Args = args;
        }

        public override string ToString()
        {
            return $"RfnExecuteData({Command.Name}, [{string.Join(", ", Args)}])";
        }

        protected bool Equals(RfnExecuteData other)
        {
            return Command.Equals(other.Command) && Args.SequenceEqual(other.Args);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((RfnExecuteData) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Command.GetHashCode() * 397) ^ (Args != null ? Args.GetHashCode() : 0);
            }
        }
    }
}