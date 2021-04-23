//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using xNet;
//using System.Web;
//using System.Net.Http;
//using System.Windows.Forms;

//namespace AutoYolikes
//{
//    public class FacebookAPI
//    {
//        public string UID;
//        public string PASS;
//        public string TFA;
//        public string COOKIE;
//        public string USERAGENT;
//        public static string _svurl = "https://mbasic.facebook.com/login/device-based/regular/login/?refsrc=https%3A%2F%2Fmbasic.facebook.com%2F&lwv=100&refid=8";
//        public static string accept;
//        public static string _content_type;
//        public static string _mbasic_server_uri;
//        public static string _mbasic_checkpoint_uri;
//        public xNet.HttpRequest http;
//        public enum LoginStatus
//        {
//            SUCCESS,
//            CHECKPOINT,
//            WRONGPASS,
//            ERROR,
//            DIE
//        }
//        public LoginStatus LoginFacebook()
//        {
//            xNet.HttpRequest httpRequest = new xNet.HttpRequest();
//            httpRequest.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
//            httpRequest.AllowAutoRedirect = true;
//            httpRequest.KeepAlive = true;
//            httpRequest.IgnoreProtocolErrors = true;
//            //httpRequest.UserAgent = USERAGENT;
//            httpRequest.Cookies = new CookieDictionary(false);
//            try
//            {
//                string FirstResponse = httpRequest.Get("https://mbasic.facebook.com/login").ToString();
//                string lsd = Regex.Match(FirstResponse, "name=\"lsd\" value=\"(.*?)\"").Groups[1].Value;
//                string jazoest = Regex.Match(FirstResponse, "name=\"jazoest\" value=\"(.*?)\"").Groups[1].Value;
//                string m_ts = Regex.Match(FirstResponse, "name=\"m_ts\" value=\"(.*?)\"").Groups[1].Value;
//                string li = Regex.Match(FirstResponse, "name=\"li\" value=\"(.*?)\"").Groups[1].Value;
//                //string lsd = Regex.Match(FirstResponse, "name=\"lsd\" value=\"(.*?)\"").Groups[1].Value;
//                string Postdata = "lsd=" + lsd + "&jazoest=" + jazoest + "&m_ts=" + m_ts + "&li=" + li + "&try_number=0&unrecognized_tries=0&email=" + UID + "&pass=" + PASS + "&login=%C4%90%C4%83ng+nh%E1%BA%ADp";
//                string[] arraypostdata = Postdata.Split('&');
//                foreach(string param in arraypostdata)
//                {
//                    string[] par_ = param.Split('=');
//                    httpRequest.AddParam(par_[0], par_[1]);
//                }    
//                //Postdata = HttpUtility.UrlEncode(Postdata);
//                //byte[] bytes = Encoding.ASCII.GetBytes(Postdata);
//                httpRequest.AddHeader(HttpHeader.Accept, accept);
//                httpRequest.AddHeader(HttpHeader.AcceptLanguage, "vi-VN,vi;q=0.9");
//                httpRequest.AddHeader("accept-encoding", " gzip, deflate, br");
//                httpRequest.AddHeader("content-length", Postdata.Length.ToString());
//                httpRequest.AddHeader("content-type", _content_type);
//                httpRequest.AddHeader("origin", _mbasic_server_uri);
//                httpRequest.AddHeader("referer", _mbasic_server_uri);
//                string response = httpRequest.PostUrlEncoded(_svurl, Postdata).ToString();
//                string nh = "";
//                //nh = httpRequest.Get("https://mbasic.facebook.com/login").ToString();
//                if(TFA == null)
//                {
//                    httpRequest.Dispose();
//                    httpRequest.Close();
//                    COOKIE = httpRequest.Cookies.ToString();
//                    http = httpRequest;
//                    return LoginStatus.SUCCESS;
//                }    
//                if (response.Contains("checkpointSubmitButton"))
//                {
//                    if (response.Contains("id=\"approvals_code\""))
//                    {
//                        nh = Regex.Match(response, "name=\"nh\" value=\"(.*?)\"").Groups[1].Value;
//                        string fb_dtsg1 = Regex.Match(response, "name=\"fb_dtsg\" value=\"(.*?)\"").Groups[1].Value;
//                        string tfff = tfacode(TFA);
//                        jazoest = Regex.Match(response, "name=\"jazoest\" value=\"(.*?)\"").Groups[1].Value;
//                        Postdata = "fb_dtsg=" + fb_dtsg1 + "&jazoest=" + jazoest + "&checkpoint_data=&approvals_code=" + tfff + "&codes_submitted=0&submit%5BSubmit+Code%5D=G%E1%BB%ADi+m%C3%A3&nh=" + nh;
//                        response = httpRequest.Post(_mbasic_checkpoint_uri, Postdata, _content_type).ToString();
//                        Postdata = "fb_dtsg=" + fb_dtsg1 + "&jazoest=" + jazoest + "&checkpoint_data=&name_action_selected=save_device&submit%5BContinue%5D=Ti%E1%BA%BFp+t%E1%BB%A5c&nh=" + nh;
//                        response = httpRequest.Post(_mbasic_checkpoint_uri, Postdata, _content_type).ToString();
//                        if (response.Contains("checkpointSubmitButton"))
//                        {
//                            Postdata = "fb_dtsg=" + fb_dtsg1 + "&jazoest=" + jazoest + "&checkpoint_data=&submit%5BContinue%5D=Ti%E1%BA%BFp+t%E1%BB%A5c&nh=" + nh;
//                            response = httpRequest.Post(_mbasic_checkpoint_uri, Postdata, _content_type).ToString();
//                            Postdata = "fb_dtsg=" + fb_dtsg1 + "&jazoest=" + jazoest + "&checkpoint_data=&submit%5BThis+was+me%5D=%C4%90%C3%A2y+l%C3%A0+t%C3%B4i&nh=" + nh;
//                            response = httpRequest.Post(_mbasic_checkpoint_uri, Postdata, _content_type).ToString();
//                            Postdata = "fb_dtsg=" + fb_dtsg1 + "&jazoest=" + jazoest + "&checkpoint_data=&name_action_selected=save_device&submit%5BContinue%5D=Ti%E1%BA%BFp+t%E1%BB%A5c&nh=" + nh;
//                            response = httpRequest.Post(_mbasic_checkpoint_uri, Postdata, _content_type).ToString();
//                        }
//                        if (response.Contains("verification_method"))
//                        {
//                            httpRequest.Dispose();
//                            httpRequest.Close();
//                            return LoginStatus.CHECKPOINT;
//                        }
//                        httpRequest.Dispose();
//                        httpRequest.Close();
//                        COOKIE = httpRequest.Cookies.ToString();
//                        http = httpRequest;
//                        return LoginStatus.SUCCESS;
//                    }
//                    httpRequest.Dispose();
//                    httpRequest.Close();
//                    return LoginStatus.ERROR;
                    
//                }
//                else
//                {
//                    httpRequest.Dispose();
//                    httpRequest.Close();
//                    return LoginStatus.WRONGPASS;
//                }
//            }
//            catch(Exception ex)
//            {
//                httpRequest.Dispose();
//                httpRequest.Close();
//                return LoginStatus.ERROR;
//            }
//        }
//        public static void LoginFBApp()
//        {
//            try
//            {
//                xNet.HttpRequest http = new xNet.HttpRequest();
//                http.UserAgent = "Mozilla/5.0 (Linux; Android 7.1.2; SM-G960N Build/N2G48B; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/68.0.3440.70 Mobile Safari/537.36";
//                http.AddHeader("x-requested-with", "app.yolikers.fb");
//                http.AddHeader("cookie", "datr=gXujXyzEE1478wHkj1z01v81; sb=gXujX0NVaRaDghonxqRFcLO-; m_pixel_ratio=1.5; c_user=100049346150817; xs=43%3AbevlHq-SMkAf8w%3A2%3A1604549637%3A16868%3A6264; wd=480x854");
//                string a = http.Get("https://m.facebook.com").ToString();
//                string response = http.Get("https://m.facebook.com/v7.0/dialog/oauth?client_id=174829003346&redirect_uri=fbconnect://success&response_type=token&sdk=android-5.1.0").ToString();
//                string fb_dtsgsd = Regex.Match(response, "name=\"fb_dtsg\" value=\"(.*?)\"").Groups[1].Value;
//                string jazoest = Regex.Match(response, "name=\"jazoest\" value=\"(.*?)\"").Groups[1].Value;
//                string logger_id = Regex.Match(response, "name=\"logger_id\" value=\"(.*?)\"").Groups[1].Value;
//                string encrypted_post_body = Regex.Match(response, "name=\"encrypted_post_body\" value=\"(.*?)\"").Groups[1].Value;
//                string postdata = $"fb_dtsg={fb_dtsgsd}&jazoest={jazoest}&from_post=1&auth_type=&auth_nonce=&cbt={GetTimestamp()}&default_audience=&dialog_type=gdp_v4&fbapp_pres=&ref=&ret=&return_format=access_token&domain=&scope=&sso_device=&logger_id={logger_id}&sheet_name=initial&fallback_redirect_uri=&sdk=android-5.1.0&facebook_sdk_version=&sdk_version=&user_code=&nonce=&logged_out_behavior=&install_nonce=&l_nonce=&original_redirect_uri=&loyalty_program_id=&messenger_page_id=&reset_messenger_state=&aid=&deferred_redirect_uri=&code_redirect_uri=&extras=&tp=unspecified&fx_app=&encrypted_post_body={encrypted_post_body}&__CONFIRM__=Ti%E1%BA%BFp+t%E1%BB%A5c";
//                string rsm = http.Post("https://m.facebook.com/v7.0/dialog/oauth/confirm/", postdata, "application/x-www-form-urlencoded").ToString();
//                http.Dispose();
//            }
//            catch
//            {

//            }
//            finally
//            {
                
//            }
//        }
//        public static String GetTimestamp()
//        {
//            DateTime value = new DateTime();
//            return value.ToString("yyyyMMddHHmmssffff");
//        }
//        public static string tfacode(string code)
//        {
//            code = code.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToString();
//            xNet.HttpRequest http = new xNet.HttpRequest();
//            string response = http.Get("http://2fa.live/tok/" + code).ToString();
//            http.Dispose();
//            http.Close();
//            return Regex.Match(response, "token.*?(\\d+)").Groups[1].Value;
//        }
//    }
//}
