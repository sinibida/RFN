using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rfn.App.InputBoxes
{
    public class UriInputBox : ConditionInputBox
    {
        private const string RegexPattern =
            @"^\s*(?:https:\/\/|http:\/\/)?(?:www.)?\w+\.(?:com|org|net|io)(?:\/.+)?\s*$";

        public override bool CheckCondition(string value) => Regex.IsMatch(value, RegexPattern);

        public override string GetKey()
        {
            return "__uri";
        }
    }
}