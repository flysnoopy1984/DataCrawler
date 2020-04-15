using System;
using System.Collections.Generic;
using System.Text;

namespace DataCrawler.Model
{
    public class ResultNormal
    {
        public int resultCode { get; set; } = 200;
        public bool IsSuccess { get; set; } = true;

        public string Message { get; set; }

        private string _ErrorMsg;
        public string ErrorMsg
        {
            get
            {
                return _ErrorMsg;
            }
            set
            {
                _ErrorMsg = value;
                IsSuccess = false;
                resultCode = 500;
            }
        }

        
    }
}
