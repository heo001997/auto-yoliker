using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoYolikes.YolikerApp;
using Anticaptcha_example.Api;
using Anticaptcha_example.Helper;
using Newtonsoft.Json.Linq;

namespace AutoYolikes.CaptchaService
{
    public class AntiCaptcha:CaptchaStatics
    {
        CaptchaStatics captchaStatics;
        ResolveState resolveState;
        public AntiCaptcha(CaptchaStatics statics)
        {
            captchaStatics = statics;
        }
        public ResolveState ResovleCaptcha()
        {
            var api = new RecaptchaV2Proxyless
            {
                ClientKey = captchaStatics.Apikey,
                WebsiteUrl = new Uri(captchaStatics.Url),
                WebsiteKey = YolikerStatics.GooglePublicKey,
            };
            try
            {
                if (!api.CreateTask())
                    resolveState = ResolveState.ERROR;
                    //DebugHelper.Out("API v2 send failed. " + api.ErrorMessage, DebugHelper.Type.Error);
                else if (!api.WaitForResult())
                    resolveState = ResolveState.FAIL;
                    //DebugHelper.Out("Could not solve the captcha.", DebugHelper.Type.Error);
                else
                    resolveState = ResolveState.SUCCESS;
                captchaStatics.SolvedCaptchaResponse = api.GetTaskSolution().GRecaptchaResponse;
                    //DebugHelper.Out("Result: " + api.GetTaskSolution().GRecaptchaResponse, DebugHelper.Type.Success);
            }
            catch
            {
                resolveState = ResolveState.ERROR;
            }
            //Resolve();
            return resolveState;
        }
    }
}
