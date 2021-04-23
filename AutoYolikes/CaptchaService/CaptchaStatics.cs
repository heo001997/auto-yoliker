using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoYolikes.CaptchaService
{
    public class CaptchaStatics
    {
        public string Apikey;
        public string Url;
        public string SolvedCaptchaResponse;
        public enum CaptchaService
        {
            _2CAPTCHA,
            CAP_MONSTER,
            ANTI_CAPTCHA,
            BYPASS
        }
        public enum ResolveState
        { 
            SUCCESS,
            FAIL,
            LOW_MONEY,
            ERROR,
            HTTP_ERROR
        }

    }
}
