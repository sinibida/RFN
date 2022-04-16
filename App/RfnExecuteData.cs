using System;
using System.Linq;

namespace Rfn.App
{
    public enum JobType { 
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
        public JobType Type { get; }
        public object[] Args { get; }

        public RfnExecuteData(JobType type, params object[] args)
        {
            Type = type;
            Args = args;
        }

        public override string ToString()
        {
            return $"RfnExecuteData({Type}, [{string.Join(", ", Args)}])";
        }

        protected bool Equals(RfnExecuteData other)
        {
            return Type == other.Type && Args.SequenceEqual(other.Args);
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
                return ((int) Type * 397) ^ (Args != null ? Args.GetHashCode() : 0);
            }
        }
    }
}