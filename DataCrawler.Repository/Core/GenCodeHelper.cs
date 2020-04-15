using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Repository.Core
{
    public static class GenCodeHelper
    {

        public const string Plan_FromDouBanTagUrls = "DBFTU";
        /// <summary>
        /// BookCode 资源简写+资源Id
        /// </summary>
        /// <param name="sourceAbbr"></param>
        /// <param name="sourceCode"></param>
        /// <returns></returns>
        public static string Book_Code(string sourceAbbr,string sourceCode)
        {
            return $"{sourceAbbr}_{sourceCode}";
        }

        /// <summary>
        /// Person Code
        /// </summary>
        /// <param name="pName"></param>
        /// <returns></returns>
        public static string Person_Code(string pName)
        {
            return pName;
        }

        /// <summary>
        /// 栏目Code+年月日
        /// </summary>
        /// <param name="SecCode"></param>
        /// <returns></returns>
        public static string DataSection_BatchNo(string SecCode)
        {
            var d = DateTime.Now;
            return $"{SecCode}_{d.Year}{d.Month}{d.Day}";
        }

        
    }
}
