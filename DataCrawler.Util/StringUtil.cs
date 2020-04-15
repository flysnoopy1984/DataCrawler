using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Util
{
    public static class StringUtil
    {
        /// <summary>
        /// 字符串提取数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string  getNumberOnly(this string str)
        {
            return System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9,.]+", "");
        }

    }
}
