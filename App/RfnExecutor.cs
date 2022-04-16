using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Rfn.App.Properties;

namespace Rfn.App
{
    public class RfnExecutor
    {
        private const string SearchGoogleFormat = "https://www.google.com/search?q={0}";
        private const string WolframAlphaFormat = "https://www.wolframalpha.com/input?i={0}";
        private const string EnKoDictFormat = "https://en.dict.naver.com/#/search?query={0}";
        private const string KoEnDictFormat = "https://en.dict.naver.com/#/search?query={0}";
        private const string KoKoDictFormat = "https://ko.dict.naver.com/#/search?query={0}";
        private const string EnEnDictFormat = "https://dict.naver.com/enendict/#/search?query={0}";
        private const string TokiPonaFormat = "https://en.wiktionary.org/wiki/Appendix:Toki_Pona/{0}";

        private static void FormatWebsiteAndOpen(string format, string query)
        {
            Process.Start(string.Format(format, HttpUtility.UrlEncode(query)));
        }

        public void Run(RfnExecuteData dat)
        {
            switch (dat.Type)
            {
                case JobType.SearchEnKoDict:
                    FormatWebsiteAndOpen(EnKoDictFormat, dat.Args[0].ToString());
                    break;
                case JobType.SearchKoKoDict:
                    FormatWebsiteAndOpen(KoKoDictFormat, dat.Args[0].ToString());
                    break;
                case JobType.SearchEnEnDict:
                    FormatWebsiteAndOpen(EnEnDictFormat, dat.Args[0].ToString());
                    break;
                case JobType.SearchKoEnDict:
                    FormatWebsiteAndOpen(KoEnDictFormat, dat.Args[0].ToString());
                    break;
                case JobType.SearchWeb:
                    FormatWebsiteAndOpen(SearchGoogleFormat, dat.Args[0].ToString());
                    break;
                case JobType.WolframAlpha:
                    FormatWebsiteAndOpen(WolframAlphaFormat, dat.Args[0].ToString());
                    break;
                case JobType.Quit:
                    if(AskClose())
                        Application.Exit();
                    break;
                case JobType.SearchTokiPona:
                    FormatWebsiteAndOpen(TokiPonaFormat, dat.Args[0].ToString());
                    break;
                case JobType.OpenWebSite:
                    Process.Start(dat.Args[0].ToString());
                    break;
                case JobType.Unknown:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static bool AskClose()
        {
            var response = MessageBox.Show(
                Resources.RfnSession_MsgBox_Stop_RFN_Text,
                Resources.RfnSession_MsgBox_Stop_RFN_Caption,
                MessageBoxButtons.YesNo);
            return response == DialogResult.Yes;
        }
    }
}
