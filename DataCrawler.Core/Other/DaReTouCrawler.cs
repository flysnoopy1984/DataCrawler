using DataCrawler.Model.CaiPiao;
using DataCrawler.Repository;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataCrawler.Core.Other
{
    public class DaReTouCrawler
    {
        private OtherDbRepository _otherDb;
        public DaReTouCrawler(OtherDbRepository otherDb)
        {
            _otherDb = otherDb;
        }
        public void AnalyFile_Html(string filePath)
        {
           string html =  this.ReadFileToHtml(filePath);
            if (string.IsNullOrEmpty(html)) return;
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

           var divList =  htmlDoc.DocumentNode.SelectNodes("//div[@id='result22']/div");
            int num = 1;
            foreach(var div in divList)
            {
                cpDaLeTouData data = new cpDaLeTouData();
                var redPoints = div.SelectNodes("./span[@class='red']");
                data.red1 = Convert.ToInt32(redPoints[0].InnerText);
                data.red2 = Convert.ToInt32(redPoints[1].InnerText);
                data.red3 = Convert.ToInt32(redPoints[2].InnerText);
                data.red4 = Convert.ToInt32(redPoints[3].InnerText);
                data.red5 = Convert.ToInt32(redPoints[4].InnerText);

                var bluePoints = div.SelectNodes("./span[@class='blue']");
                data.blue1 = Convert.ToInt32(bluePoints[0].InnerText);
                data.blue2 = Convert.ToInt32(bluePoints[1].InnerText);

                data.remark = div.InnerText;
                _otherDb.Add(data);
                Console.WriteLine($"写入{num} 组");
                num++;
            }
        }

        private string  ReadFileToHtml(string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            string html = "";
            using (StreamReader reader = new StreamReader(fileStream,Encoding.UTF8))
            {
                html = reader.ReadToEnd();
            }
            return html;

        }


        }
}
