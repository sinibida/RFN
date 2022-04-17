using System;
using System.Collections.Generic;

namespace Rfn.App.Commands
{
    public class TryQuitCommand : RfnCommand
    {
        public TryQuitCommandProperties Properties { get; set; }

        public override void Execute(string[] args)
        {
            RfnSession.Instance.TryQuit();
        }

        public override Type GetPropertyType()
        {
            return typeof(TryQuitCommandProperties);
        }

        public override void SetProperty(object prop)
        {
            Properties = (TryQuitCommandProperties)prop;
        }
    }
}