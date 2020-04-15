using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCrawler.Model;
using DataCrawler.Model.MiddleObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataCrawler.API.Controllers
{
   
    public class BaseController : ControllerBase
    {
        public string toJson(List<BookDetail_middle> list)
        {
            string r;
            try
            {
                return JsonConvert.SerializeObject(list);
                //foreach(var item in list)
                //{
                //    //JsonConvert.SerializeObject()

                //}
                //return "";
            }
            catch(Exception ex)
            {
                return "转换Json失败";
            }
        }
    }
}