using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace DataCrawler.Util
{
    public enum PlantSource
    {
        FeiZhu = 3,
        BeiKe = 0,
        JiGuang =1,
        ZhiMa =2,
       
    }

    public static class ProxyManager
    {
        private static IMemoryCache _MemoryCache;
        private const string keyHttpProxy = "httpProxy";

        private static string BeiKeUrl = "http://getip.beikeruanjian.com/getip/?user_id=20200416101038691896&token=6JqgZha2SEQ5x0kR&server_id=1821&num=1&protocol=1&format=json&jsonipport=1&jsonexpiretime=1&jsoncity=1&jsonisp=1&dr=0&province=1&city=1";
        private static string JiGuangUrl = "http://d.jghttp.golangapi.com/getip?num=10&type=1&pro=&city=0&yys=0&port=1&pack=20503&ts=0&ys=0&cs=0&lb=1&sb=0&pb=4&mr=1&regions=";
        private static string ZhiMaUrl = "http://http.tiqu.alicdns.com/getip3?num=20&type=1&pro=&city=0&yys=0&port=1&pack=92851&ts=0&ys=0&cs=0&lb=1&sb=0&pb=4&mr=1&regions=&gm=4";
        private static string FeiZhuUrl = "http://120.79.85.144/index.php/api/entry?method=proxyServer.tiqu_api_url&packid=1&fa=0&fetch_key=&groupid=0&qty=10&time=1&port=1&format=txt&ss=1&css=&pro=&city=&dt=1&usertype=20";
                              
                        
        private static Stack<string> _proxyHosts;

        public static Stack<PlantSource> _stPlantSources;

        //public static void ReadConfig(IConfiguration configuration)
        //{
        //    BeiKeUrl = configuration["Proxys:BeiKe"];
        //    JiGuangUrl = configuration["Proxys:JiGuang"];
        //    ZhiMaUrl = configuration["Proxys:ZhiMa"];
        //    FeiZhuUrl = configuration["Proxys:FeiZhu"];
        //}
      
        static ProxyManager()
        {
            var op = new MemoryCacheOptions();
            _MemoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            _proxyHosts = new Stack<string>();
            _stPlantSources = new Stack<PlantSource>();
            //初始化代理平台
            var pss = Enum.GetValues(typeof(PlantSource));
            foreach(var v in pss)
            {
                
                PlantSource ps = (PlantSource)Enum.Parse(typeof(PlantSource),v.ToString());
                _stPlantSources.Push(ps);
            }
        }

        public static void RemoveProxyHostCache()
        {
            _MemoryCache.Remove(keyHttpProxy);
        }
        public static string getProxyHost(int cacheSec = 300)
        {
            string result = "";

            if (_MemoryCache.TryGetValue(keyHttpProxy, out result))
            {
              //  if(TestProxy(result))
                    return result;
            }

            string host = getAvaliableHost();
            if (string.IsNullOrEmpty(host))
                return "";

    
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(cacheSec);
            _MemoryCache.Set(keyHttpProxy, host, options);

            var platform = _stPlantSources.Peek();
            NLogUtil.InfoTxt($"Find New Proxy Host:{platform}_{host}");
            return host;
        }

        private static void runProxyPlatform()
        {
            while(_stPlantSources.Count>0)
            {
                var platform = _stPlantSources.Peek();
                NLogUtil.InfoTxt($"切换 {platform} 代理平台");
                bool result=true;

                switch (platform)
                {
                    case PlantSource.BeiKe:
                        result = AddBeiKeHost();
                        break;
                    case PlantSource.FeiZhu:
                        result = AddFeiZhuHost();
                        break;
                    case PlantSource.JiGuang:
                        result = AddJiGuangHost(); 
                        break; 
                    case PlantSource.ZhiMa:
                        result = AddZhiMaHost();
                        break;
                }
                if (!result) 
                    _stPlantSources.Pop();
                else 
                    return;
            }
            
        }

        private static string getAvaliableHost()
        {
           
            //InitProxyAddList_89ip();
            if(_proxyHosts.Count == 0)
            {
                runProxyPlatform();
                if (_proxyHosts.Count == 0) return "";     
            }
               
            while (_proxyHosts.Count > 0)
            {
                var host = _proxyHosts.Pop();
                var testResult = TestProxy(host);
                if (testResult)
                    return host;
            }
            if (_stPlantSources.Count > 0)
                runProxyPlatform();


            return "";

        }

        /// <summary>
        /// 飞猪  http://120.79.85.144/index.php/api/entry?method=proxyServer.tiqu_api_url&packid=1&fa=0&fetch_key=&groupid=0&qty=10&time=1&port=1&format=txt&ss=1&css=&pro=&city=&dt=0&usertype=20
        /// </summary>
        /// <returns></returns>
        public static bool AddFeiZhuHost()
        {

            HttpClient httpClient = new HttpClient();
            var rep = httpClient.GetAsync(FeiZhuUrl);
            var json = rep.Result.Content.ReadAsStringAsync();
            var list = json.Result.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            bool result = true;
            try
            {
                if (list.Length == 1)
                {
                    JObject j = JObject.Parse(list[0]);
                    if (Convert.ToBoolean(j["success"]) == false)
                        result = false;
                }
            }
            catch
            {
                result = false;
            }
            if (result)
            {
                foreach (var host in list)
                {
                    _proxyHosts.Push(host);
                }
            }
            else
            {
                NLogUtil.InfoTxt("【飞猪 IP已用完】");
            }

            return result;

        }

        /// <summary>
        /// 芝麻Host http://http.tiqu.alicdns.com/getip3?num=5&type=1&pro=&city=0&yys=0&port=1&pack=92851&ts=0&ys=0&cs=0&lb=1&sb=0&pb=4&mr=1&regions=&gm=4
        /// </summary>
        /// <returns></returns>
        public static bool AddZhiMaHost()
        {

            HttpClient httpClient = new HttpClient();
            var rep = httpClient.GetAsync(ZhiMaUrl);
            var json = rep.Result.Content.ReadAsStringAsync();
            var list = json.Result.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            bool result = true;
            try
            {
                if (list.Length == 1)
                {
                    JObject j = JObject.Parse(list[0]);
                    if (Convert.ToBoolean(j["success"]) == false)
                        result = false;
                }
            }
            catch
            {
                result = false;
            }
            if (result)
            {
                foreach (var host in list)
                {
                    _proxyHosts.Push(host);
                }
            }
            else
            {
                NLogUtil.InfoTxt("【芝麻 IP已用完】");
            }

            return result;

        }

        /// <summary>
        /// 极光 http://d.jghttp.golangapi.com/getip?num=20&type=1&pro=&city=0&yys=0&port=1&pack=20503&ts=0&ys=0&cs=0&lb=1&sb=0&pb=4&mr=1&regions=
        /// </summary>
        public static bool AddJiGuangHost()
        {

            HttpClient httpClient = new HttpClient();
            var rep = httpClient.GetAsync(JiGuangUrl);
            var json = rep.Result.Content.ReadAsStringAsync();
            var list = json.Result.Split("\r\n",StringSplitOptions.RemoveEmptyEntries);
            bool result = true ;
            try
            {
                if(list.Length == 1)
                {
                    JObject j = JObject.Parse(list[0]);
                    if (Convert.ToBoolean(j["success"]) == false)
                        result = false;
                }
            }
            catch
            {
                result = false;
            }
            if(result)
            {
                foreach (var host in list)
                {
                    _proxyHosts.Push(host);
                }
            }
            else
            {
                NLogUtil.InfoTxt("【极光 IP已用完】");
            }
           
            return result;
          
        }

        /// <summary>
        ///  贝壳 http://h.beikeruanjian.com/get/ jackysong@edifier
        /// </summary>
        public static bool AddBeiKeHost()
        {
            bool result = true;
            HttpClient httpClient = new HttpClient();
            var rep =  httpClient.GetAsync(BeiKeUrl);
            rep.Wait();
          //  rep.Status == System.Threading.Tasks.TaskStatus.
            var json = rep.Result.Content.ReadAsStringAsync();
            JObject j = null;
            try
            {
                j= JObject.Parse(json.Result);
                var status = j["status"].ToString();
                if (status == "success")
                {
                    var v = j["data"][0]["ipport"].ToString();
                    _proxyHosts.Push(v);
                }
                else
                    result = false;
            }
            catch(Exception ex)
            {
                NLogUtil.ErrorTxt($"AddBeiKeHost Json Prarse Error:{ex.Message}");
                result = false;
               
            }
            if (result == false)
                NLogUtil.InfoTxt("【贝壳 IP已用完】");




            return result;
           
        }

     //   public static void 

        public static bool TestProxy(string proxyHost)
        {
            WebProxy proxyObject = new WebProxy(proxyHost);//str为IP地址 port为端口号
            HttpWebRequest ReqProxy = (HttpWebRequest)WebRequest.Create("http://www.baidu.com/");
            ReqProxy.Proxy = proxyObject; //设置代理 
            try
            {
                HttpWebResponse Resp = (HttpWebResponse)ReqProxy.GetResponse();
            }
            catch(Exception ex)
            {
                string e = ex.Message;
                return false;
            }
            return true;
           
        }

        /// <summary>
        /// http://www.89ip.cn/tqdl.html?api=1&num=60&port=&address=&isp=
        /// </summary>
        /// <returns></returns>
        public static void InitProxyAddList_89ip()
        {
            string url = "http://www.89ip.cn/tqdl.html?api=1&num=60&port=&address=&isp=";
            List<string> list = new List<string>();
            HtmlWeb htmlWeb = new HtmlWeb();
            htmlWeb.OverrideEncoding = Encoding.UTF8;
            HtmlDocument htmlDoc = htmlWeb.Load(url);

           var nodes = htmlDoc.DocumentNode.SelectNodes("//br");
            foreach(var n in nodes)
            {
                if(n.PreviousSibling.Name == "#text")
                {
                    _proxyHosts.Push(n.PreviousSibling.InnerText.Trim());
                  //  list.Add(n.PreviousSibling.InnerText.Trim());
                }
               // var tn  = n.PreviousSibling;
            }

           // return list;
        }


        /// <summary>
        /// 测试有效期
        /// </summary>
        /// <returns></returns>
        public static string TestCache()
        {
            string result = "";

            if (_MemoryCache.TryGetValue(keyHttpProxy, out result)) return result;

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(5);
            result = "aaa";
            _MemoryCache.Set(keyHttpProxy, result, options);

            return result;
        }
    }
}
