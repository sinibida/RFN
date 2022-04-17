using System;
using System.Collections.Generic;
using Windows.Globalization.DateTimeFormatting;
using Windows.Media.Capture.Frames;

namespace Rfn.App.Commands
{
    public abstract class RfnCommand
    {
        public string Name { get; set; }
        public string[] Alias { get; set; }

        public abstract void Execute(string[] args);
        public abstract Type GetPropertyType();
        public abstract void SetProperty(object prop);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RfnCommand) obj);
        }

        protected bool Equals(RfnCommand other)
        {
            return Name == other.Name && Equals(Alias, other.Alias);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name != null ? Name.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Alias != null ? Alias.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}