using NLog;
using System;

namespace DataCrawler.Util
{
    public static class NLogUtil
    {
        private static Logger _FileLogger = LogManager.GetLogger("DataCrawlerInfoLog");
        private static Logger _FileErrorLogger = LogManager.GetLogger("DataCrawlerErrorLog");

        public static void InfoTxt(string txt,bool isPrint = true)
        {
            try
            {
                _FileLogger.Info(txt);
                if (isPrint) Console.WriteLine(txt);
            }
            catch (Exception ex)
            {

            }


        }

        public static void ErrorTxt(string txt, bool isPrint = true)
        {
            try
            {
                _FileErrorLogger.Error(txt);
                if (isPrint) Console.WriteLine(txt);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
