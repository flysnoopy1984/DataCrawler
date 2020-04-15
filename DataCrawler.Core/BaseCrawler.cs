using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DataCrawler.Core
{
    public class BaseCrawler
    {
        public static string GetHtmlText(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return Regex.Match(value, "(?<=>).*?(?=</a>)").Value;
        }



    }
}
