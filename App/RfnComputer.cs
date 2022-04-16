using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfn.App
{
    public class RfnComputer
    {
        private static readonly string[] WolframAlphaIgnore =
        {
            "derivative of",
            "derivative",
            "derivation",
            "d/dx",
            "arcsin",
            "arccos",
            "arctan",
            "sinh",
            "cosh",
            "tanh",
            "sin",
            "cos",
            "tan",
            "x",
            "e",
        };

        //private static bool IsKoDictTag(string tag) => tag == "k" || tag == "K" || tag == "ㅏ";
        private static bool IsEnDictTag(string tag) => tag == "e" || tag == "E" || tag == "ㄷ";

        private static bool IsWolframAlphaTag(string tag) => tag == "w" || tag == "W" || tag == "ㅈ";

        private static bool IsGoogleTag(string tag) => tag == "g" || tag == "G" || tag == "ㅎ";
        
        private static bool IsTokiPonaTag(string tag) => tag == "t" || tag == "T" || tag == "ㅅ";

        public static bool IsKoreanNotEnglish(string input)
        {
            return KoreanProb(input) > EnglishProb(input);
        }

        public static bool IsEquation(string input)
        {
            return EquationProb(input) > KoreanProb(input) + EnglishProb(input);
        }

        private static bool IsUriString(string body)
        {
            return body.EndsWith(".com") ||
                   body.EndsWith(".net") ||
                   body.EndsWith(".org");
        }

        public static double KoreanProb(string input)
        {
            var formattedInput = TidyStringForProbFunction(input);

            if (formattedInput.Length == 0)
                return 1;

            return (double) formattedInput.Count(c => '가' <= c && c <= '힣') / formattedInput.Length;
        }

        public static double EnglishProb(string input)
        {
            var formattedInput = TidyStringForProbFunction(input);

            if (formattedInput.Length == 0)
                return 1;

            return (double) formattedInput.Count(c => 'a' <= c && c <= 'z') / formattedInput.Length;
        }

        public static double EquationProb(string input, bool wolframAlpha = true)
        {
            var formattedInput = TidyStringForProbFunction(input);

            if (formattedInput.Length == 0)
                return 1;

            string ignoredInput;
            int add = 0;

            if (wolframAlpha)
            {
                ignoredInput = input.ToLower();
                foreach (var s in WolframAlphaIgnore)
                {
                    ignoredInput = ignoredInput.Replace(s, "");
                }
                add = input.Length - ignoredInput.Length;
                ignoredInput = TidyStringForProbFunction(ignoredInput);


            }
            else
            {
                ignoredInput = formattedInput;
            }

            return (double)(ignoredInput.Count(c =>
                '0' <= c && c <= '9' ||
                c == '+' || c == '-' ||
                c == '*' || c == '/' ||
                c == '^' ||
                c == '(' || c == ')'
            ) + add) / formattedInput.Length;
        }

        private static string TidyStringForProbFunction(string input)
        {
            return new string(input.ToLower().Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        public RfnExecuteData Compute(string input)
        {
            if (input.StartsWith(":"))
            {
                var dat = RfnExecuteDataFromCommand(input.Substring(1));
                if (dat != null)
                    return dat;
            }
            if (!input.Contains(';'))
            {
                return RfnExecuteDataFromBody(input);
            }
            
            var tag = GetTagAndBody(input, out var body);
            return string.IsNullOrEmpty(tag) ? RfnExecuteDataFromBody(body) : RfnExecuteDataFromTagBody(tag, body);
        }

        private static string GetTagAndBody(string raw, out string body)
        {
            var split = raw.Split(';');
            string tag = "";

            if (Tag(out tag, out body, IsGoogleTag, "g", split))
                return tag;
            if (Tag(out tag, out body, IsWolframAlphaTag, "w", split))
                return tag;
            if (Tag(out tag, out body, IsEnDictTag, "e", split))
                return tag;
            if (Tag(out tag, out body, IsTokiPonaTag, "t", split))
                return tag;

            body = raw;
            return "";
        }

        private static bool Tag(out string tag, out string body, Predicate<string> condition, string targetTag, string[] split)
        {
            if (condition(split.First()))
            {
                tag = targetTag;
                body = string.Join(";", split.Skip(1));
                return true;
            }

            if (condition(split.Last()))
            {
                tag = targetTag;
                body = string.Join(";", split.Take(split.Length - 1));
                return true;
            }

            tag = "";
            body = "";
            return false;
        }

        private static RfnExecuteData RfnExecuteDataFromBody(string body)
        {
            if(IsUriString(body))
                return new RfnExecuteData(JobType.OpenWebSite, new UriBuilder(body).Uri.AbsoluteUri);
            if (IsEquation(body))
                return new RfnExecuteData(JobType.WolframAlpha, body);
            if (!body.Contains(" ")) // Single word
            {
                return IsKoreanNotEnglish(body)
                    ? new RfnExecuteData(JobType.SearchKoKoDict, body)
                    : new RfnExecuteData(JobType.SearchEnKoDict, body);
            }

            return new RfnExecuteData(JobType.SearchWeb, body);
        }

        private static RfnExecuteData RfnExecuteDataFromCommand(string body)
        {
            var split = body.Split(' ');
            if (split.Length == 0)
                return null;
            var rest = split.Length > 1 ? body.Substring(body.IndexOf(' ')) : string.Empty;

            switch (split[0].ToLower())
            {
                case "d":
                case "drv":
                case "deriv":
                case "derivative":
                    if (rest == string.Empty)
                        return new RfnExecuteData(JobType.WolframAlpha, body);
                    return new RfnExecuteData(
                        JobType.WolframAlpha,
                        $"Derivative of {rest}");
                case "q":
                case "quit":
                    return new RfnExecuteData(JobType.Quit);
                case "g":
                case "google":
                    return new RfnExecuteData(JobType.SearchWeb, body);
                default:
                    return null;
            }
        }

        private static RfnExecuteData RfnExecuteDataFromTagBody(string tag, string body)
        {
            switch (tag)
            {
                case "e":
                    return IsKoreanNotEnglish(body)
                        ? new RfnExecuteData(JobType.SearchKoEnDict, body)
                        : new RfnExecuteData(JobType.SearchEnEnDict, body);
                case "g":
                    return new RfnExecuteData(JobType.SearchWeb, body);
                case "w":
                    return new RfnExecuteData(JobType.WolframAlpha, body);
                case "t":
                    return new RfnExecuteData(JobType.SearchTokiPona, body);
                default:
                    return new RfnExecuteData(JobType.SearchWeb, body);
            }
        }
    }
}