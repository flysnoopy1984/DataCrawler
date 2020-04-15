using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Util
{
    public static class PinYinUtil
    {
        public static string  ToPinYin(this string str)
        {
            string result = string.Empty;
            foreach (char item in str)
            {
                try
                {
                    ChineseChar cc = new ChineseChar(item);
                    if (cc.Pinyins.Count > 0 && cc.Pinyins[0].Length > 0)
                    {
                        string temp = cc.Pinyins[0].ToString();
                        result += temp.Substring(0, temp.Length - 1);
                    }
                }
                catch (Exception)
                {
                    result += item.ToString();
                }
            }
            return result;
        }
    }
}
