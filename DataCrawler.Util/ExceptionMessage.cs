using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Util
{
    public class ExceptionProxyConnect:Exception
    {
        public ExceptionProxyConnect(string msg) : base(msg)
        {
            ProxyManager.RemoveProxyHostCache();
        }
    }
}
