using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoYolikes.YolikerApp;

namespace AutoYolikes.CaptchaService
{
    public class IResolveCaptchaResponse:CaptchaStatics
    {
        public ResolveState Resolve (CaptchaStatics statics, CaptchaService service)
        {
            if(service == CaptchaService.CAP_MONSTER)
            {
                Capmonter capmonter = new Capmonter(statics);
                return capmonter.ResoloveCaptcha();
            }    
            else if(service == CaptchaService.ANTI_CAPTCHA)
            {
                AntiCaptcha antiCaptcha = new AntiCaptcha(statics);
                return antiCaptcha.ResovleCaptcha();
            }
            else if (service == CaptchaService.BYPASS)
            {
                AntiCaptcha antiCaptcha = new AntiCaptcha(statics);
                return antiCaptcha.ResovleCaptcha();
            }
            else
            {
                _2Captcha _2Captcha = new _2Captcha(statics.Apikey);
                return _2Captcha.SolveRecaptchaV2(YolikerStatics.GooglePublicKey, YolikerStatics.LikeCustomUri, out statics.SolvedCaptchaResponse);
            }
        }
    }
}
