using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapMonsterCloud.Models;
using CapMonsterCloud;
using CapMonsterCloud.Exceptions;
using CapMonsterCloud.Models.CaptchaTasks;
using CapMonsterCloud.Models.CaptchaTasksResults;
using AutoYolikes.YolikerApp;

namespace AutoYolikes.CaptchaService
{
    public class Capmonter:CaptchaStatics
    {
        CaptchaStatics captchaStatics;
        public Capmonter(CaptchaStatics statics)
        {
            captchaStatics = statics;
        }
        public ResolveState ResoloveCaptcha()
        {
            ResolveState resolveState;
            var client = new CapMonsterClient(captchaStatics.Apikey);
            try
            {
                var captchaTask = new NoCaptchaTaskProxyless
                {
                    WebsiteUrl = YolikerStatics.LikeCustomUri,
                    WebsiteKey = YolikerStatics.GooglePublicKey,
                };
                var taskId = client.CreateTaskAsync(captchaTask).Result;
                var solution = client.GetTaskResultAsync<NoCaptchaTaskProxylessResult>(taskId).Result;
                var recaptchaResponse = solution.GRecaptchaResponse;
                captchaStatics.SolvedCaptchaResponse = recaptchaResponse;
                resolveState = ResolveState.SUCCESS;
            }
            catch
            {
                resolveState = ResolveState.ERROR;
            }
            finally
            {
                client.Dispose();
            }
            return resolveState;
        }
        //public async void test()
        //{
        //    var clientKey = "3cf89cb341ccc74b3c52b2cacc336f34";
        //    var secret = clientKey;
        //    var start = DateTime.Now;
        //    var client = new CapMonsterClient(secret);
        //    var captchaTask = new NoCaptchaTaskProxyless
        //    {
        //        WebsiteUrl = "https://app.mp3songs.desi/like_rs.php?type=custom",
        //        WebsiteKey = "6Lc4D68ZAAAAACCX57dQFmEBGNr1VOmOX228Zwv8",
        //        //PageAction = "myverify"
        //    };
        //    // Create the task and get the task id
        //    var taskId = client.CreateTaskAsync(captchaTask).Result;
        //    Console.WriteLine("Created task id : " + taskId);
        //    var solution = client.GetTaskResultAsync<NoCaptchaTaskProxylessResult>(taskId).Result;
        //    // Recaptcha response to be used in the form
        //    var recaptchaResponse = solution.GRecaptchaResponse;
        //    //var recaptchaResponse = "bad";

        //    Console.WriteLine("Solution : " + recaptchaResponse);
        //    var web = new WebClient { Encoding = Encoding.UTF8 };
        //    web.Headers.Add("content-type", "application/x-www-form-urlencoded");
        //    var result = web.UploadString("https://lessons.zennolab.com/captchas/recaptcha/v3_verify.php?level=beta", "token=" + recaptchaResponse);
        //    var idxStart = result.IndexOf("<pre>", StringComparison.Ordinal);
        //    var idxEnd = result.IndexOf("</pre>", StringComparison.Ordinal);
        //    var jsonResult = result.Substring(idxStart, idxEnd - idxStart);
        //    Console.WriteLine(jsonResult);
        //    var end = DateTime.Now;
        //    var duration = end - start;
        //    Console.WriteLine(duration.TotalSeconds);

        //}
    }
}
