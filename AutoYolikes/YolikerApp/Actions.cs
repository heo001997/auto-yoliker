using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoYolikes.FB;
using AutoYolikes.YolikerApp;
using AutoYolikes.CaptchaService;
using System.Text.RegularExpressions;

namespace AutoYolikes.YolikerApp
{
    public class Actions:YolikerStatics
    {
        public FacebookStatics statics;
        public Session fbSession;
        public CaptchaStatics captchaSt;
        public YolikerStatics yolikerStatics;
        public YolikerLogs Logs;
        public YolikerState LoginYoliker()
        {
            string LoginPageResponse = "";
            YolikerState yolikerState;
            try
            {
                string dataecd = "fbconnect%3A%2F%2Fsuccess%23access_token%3D" + statics.access_token + "%26data_access_expiration_time%3d0%26expires_in%3d0%26cookie%3d" + statics.cookie; //1625339446
                string url = LoginUrl + dataecd;
                fbSession.http.KeepAlive = true;
                fbSession.http.AllowAutoRedirect = true;
                fbSession.http.IgnoreProtocolErrors = true;
                fbSession.http.AddHeader("user-agent", FacebookStatics.AndroidUG);
                fbSession.http.AddHeader("x-requested-with", "app.yolikers.fb");
                fbSession.http.AddHeader("host", "app.mp3songs.desi");
                LoginPageResponse = fbSession.http.Get(url).ToString();
                if (!LoginPageResponse.Contains("Login Successful"))
                {
                    if(LoginPageResponse.Contains("Attention Required! | Cloudflare"))
                    {
                        yolikerState = YolikerState.CLOUD_FLARE;
                    }   
                    else
                    {
                        yolikerState = YolikerState.FAIL;
                    }
                }
                else
                {
                    yolikerState = YolikerState.SUCCESS;
                }
            }
            catch
            {
                yolikerState = YolikerState.ERROR;
            }
            finally
            {
                LoginPageResponse = "";
            }
            return yolikerState;
        }
        public YolikerState StartAction()
        {
            string Response = "";
            YolikerState yolikerState;
            try
            {
                fbSession.http.KeepAlive = true;
                fbSession.http.AllowAutoRedirect = true;
                fbSession.http.IgnoreProtocolErrors = true;
                fbSession.http.AddHeader("user-agen", FacebookStatics.AndroidUG);
                fbSession.http.AddHeader("x-requested-with", "app.yolikers.fb");
                Response = fbSession.http.Get(LikeCustomUri).ToString();
                if(Response.Contains("Time Left"))
                {
                    yolikerState = YolikerState.LIMITED;
                    return yolikerState;
                }
                string datePostId = Regex.Match(Response, "class=\"form-control\" name=\"(.*?)\" value=\"\"").Groups[1].Value;
                string dataPostuttonOrigin = Regex.Match(Response, "type=\"submit\"(.*)class=").Groups[0].Value;
                string dataPostuttonLast = Regex.Match(dataPostuttonOrigin, "name=\"(.*?)\"").Groups[1].Value;
                IResolveCaptchaResponse captchaResponse = new IResolveCaptchaResponse();
                Logs.Write("đang giải captcha...", YolikerLogs.TypeLog.NORMAL);
                CaptchaStatics.ResolveState resolveState = captchaResponse.Resolve(captchaSt, yolikerStatics.captchaSvName);
                if (resolveState == CaptchaStatics.ResolveState.SUCCESS)
                {
                    Logs.Write($"giải captcha thành công ! tiến hành tăng {yolikerStatics.TypeAction}", YolikerLogs.TypeLog.NORMAL);
                    string dataPost = datePostId + $"={yolikerStatics.IdPost}&type={yolikerStatics.TypeAction}&g-recaptcha-response={captchaSt.SolvedCaptchaResponse}&{dataPostuttonLast}=Submit";
                    //Response = fbSession.http.Get("https://www.google.com/recaptcha/api2/anchor?ar=1&k=6Lc4D68ZAAAAACCX57dQFmEBGNr1VOmOX228Zwv8&co=aHR0cHM6Ly9hcHAubXAzc29uZ3MuZGVzaTo0NDM.&hl=vi&v=mrdLhN7MywkJAAbzddTIjTaM&size=normal&cb=92qzwtgq95c9").ToString();
                    //string resultXC = Regex.Match(Response, "id=\"recaptcha-token\" value=\"(.*?)\"").Groups[1].Value;
                    //string dataPost = datePostId + $"={yolikerStatics.IdPost}&type={yolikerStatics.TypeAction}&g-recaptcha-response={resultXC}&{dataPostuttonLast}=Submit";
                    fbSession.http.KeepAlive = true;
                    fbSession.http.AllowAutoRedirect = true;
                    fbSession.http.IgnoreProtocolErrors = true;
                    fbSession.http.AddHeader("user-agen", FacebookStatics.AndroidUG);
                    fbSession.http.AddHeader("x-requested-with", "app.yolikers.fb");
                    Response = fbSession.http.PostUrlEncoded(LikeCustomUri, dataPost).ToString();
                    if(Response.Contains("Success"))
                    {
                        string LikeSent = Regex.Match(Response, "<strong>(\\d+) Likes/Reaction Delivered").Groups[1].Value;
                        if(!string.IsNullOrEmpty(LikeSent))
                        {
                            NumLikeDelivered = Convert.ToInt32(LikeSent);
                        }   
                        else
                        {
                            NumLikeDelivered = 50;
                        }
                        yolikerState = YolikerState.SUCCESS;
                    } 
                    else
                    {
                        yolikerState = YolikerState.FAIL;
                    }
                }
                else
                {
                    yolikerState = YolikerState.CAPTCHA_FAILED;
                }
            }
            catch
            {
                yolikerState = YolikerState.ERROR;
            }
            finally
            {
                Response = "";
            }
            return yolikerState;
        }
    }
}
