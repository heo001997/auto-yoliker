using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoYolikes.YolikerApp;
using xNet;
using System.Text.RegularExpressions;

namespace AutoYolikes.FB
{
    class FacebookLogin:FacebookStatics
    {

        /// <summary>
        /// source code by Nguyễn Đắc Tài | Founder of FAM Software, tienichmmo.net
        /// </summary>
        public FacebookStatics facebookStatics;
        public Session FacebookSession;
        public FacebookLogin(FacebookStatics statics)
        {
            facebookStatics = statics;
        }
        public LoginStatus Login()
        {
            Session session = new Session();
            session.CreateSession();
            FacebookSession = session;
            LoginStatus loginStatus;
            string Postdata = "";
            string ResponseLogin = "";
            try
            {
                if(facebookStatics.IssCookie)
                {
                    session.http.AddHeader("cookie", facebookStatics.cookie);
                    ResponseLogin = session.http.Get(LoginFBUrl).ToString();
                    {
                        if(ResponseLogin.Contains("name=\"xc_message\""))
                        {
                            loginStatus = LoginStatus.SUCCESS;
                            return loginStatus;
                        }    
                        else
                        {
                            loginStatus = LoginStatus.ERROR;
                            return loginStatus;
                        }
                    }
                }    
                string ResponseLoginPage = session.http.Get(LoginFBUrl).ToString();
                session.InitParamsCodeLogin(ResponseLoginPage);
                ResponseLoginPage = "";
                Postdata = $"lsd={session.lsd}&jazoest={session.jazoest}&m_ts={session.m_ts}&li={session.li}&try_number=0&unrecognized_tries=0&email={facebookStatics.uid}&pass={facebookStatics.pass}&login=%C4%90%C4%83ng+nh%E1%BA%ADp";
                session.AddParamsLogin(Postdata);
                //Postdata = System.Net.WebUtility.UrlEncode(Postdata);
                ResponseLogin = session.http.Post(_svurl, Postdata, "application/x-www-form-urlencoded").ToString();
                string nh = "";
                if(facebookStatics.tfa == null)
                {
                    loginStatus = LoginStatus.SUCCESS;
                }    
                if(ResponseLogin.Contains("checkpointSubmitButton"))
                {
                    nh = AppExtension.RegexByNameHtml(ResponseLogin, "nh", 1);
                    session.fb_dtsg = AppExtension.RegexByNameHtml(ResponseLogin, "fb_dtsg", 1);
                    string _2facode = AppExtension.tfacode(facebookStatics.tfa);
                    session.jazoest = AppExtension.RegexByNameHtml(ResponseLogin, "jazoest", 1);
                    Postdata = $"fb_dtsg={session.fb_dtsg}&jazoest={session.jazoest}&checkpoint_data=&approvals_code={_2facode}&codes_submitted=0&submit%5BSubmit+Code%5D=G%E1%BB%ADi+m%C3%A3&nh={nh}";
                    ResponseLogin = session.http.PostUrlEncoded(_mbasic_checkpoint_uri, Postdata).ToString();
                    Postdata = $"fb_dtsg={session.fb_dtsg}&jazoest={session.jazoest}&checkpoint_data=&name_action_selected=save_device&submit%5BContinue%5D=Ti%E1%BA%BFp+t%E1%BB%A5c&nh={nh}";
                    ResponseLogin = session.http.PostUrlEncoded(_mbasic_checkpoint_uri, Postdata).ToString();
                    if (ResponseLogin.Contains("checkpointSubmitButton"))
                    {
                        Postdata = $"fb_dtsg={session.fb_dtsg}&jazoest={session.jazoest}&checkpoint_data=&submit%5BContinue%5D=Ti%E1%BA%BFp+t%E1%BB%A5c&nh=" + nh;
                        ResponseLogin = session.http.PostUrlEncoded(_mbasic_checkpoint_uri, Postdata).ToString();
                        Postdata = $"fb_dtsg={session.fb_dtsg}&jazoest={session.jazoest}&checkpoint_data=&submit%5BThis+was+me%5D=%C4%90%C3%A2y+l%C3%A0+t%C3%B4i&nh=" + nh;
                        ResponseLogin = session.http.PostUrlEncoded(_mbasic_checkpoint_uri, Postdata).ToString();
                        Postdata = $"fb_dtsg={session.fb_dtsg}&jazoest={session.jazoest}&checkpoint_data=&name_action_selected=save_device&submit%5BContinue%5D=Ti%E1%BA%BFp+t%E1%BB%A5c&nh=" + nh;
                        ResponseLogin = session.http.PostUrlEncoded(_mbasic_checkpoint_uri, Postdata).ToString();
                    }
                    else
                    {
                        loginStatus = LoginStatus.SUCCESS;
                    }
                    if (ResponseLogin.Contains("verification_method"))
                    {
                        loginStatus = LoginStatus.CHECKPOINT;
                    }
                    else
                    {
                        loginStatus = LoginStatus.SUCCESS;
                    }
                }   
                else
                {
                    loginStatus = LoginStatus.WRONG_PASS;
                }
            }
            catch
            {
                loginStatus = LoginStatus.ERROR;
                return loginStatus;
            }
            finally
            {
                AccessTokenStatus accessTokenStatus = LoginAutGetToken();
                if (accessTokenStatus == AccessTokenStatus.SUCCESS)
                {
                    facebookStatics.cookie = session.http.Cookies.ToString();
                    loginStatus = LoginStatus.SUCCESS;
                }
                else
                {
                    loginStatus = LoginStatus.ERROR;
                }
                FacebookSession = session;
                session.Dispose();
                Postdata = "";
                ResponseLogin = "";
            }
            return loginStatus;
        }
        public LoginStatus LoginMBS()
        {
            Session session = new Session();
            session.CreateSession();
            FacebookSession = session;
            LoginStatus loginStatus;
            string Postdata = "";
            string ResponseLogin = "";
            try
            {
                if (facebookStatics.IssCookie)
                {
                    session.http.AddHeader("cookie", facebookStatics.cookie);
                    ResponseLogin = session.http.Get(LoginFBUrl).ToString();
                    {
                        if (ResponseLogin.Contains("name=\"xc_message\""))
                        {
                            loginStatus = LoginStatus.SUCCESS;
                            return loginStatus;
                        }
                        else
                        {
                            loginStatus = LoginStatus.ERROR;
                            return loginStatus;
                        }
                    }
                }
                string ResponseLoginPage = session.http.Get(MbasicLoginAuthurl).ToString();
                session.InitParamsCodeLogin(ResponseLoginPage);
                ResponseLoginPage = "";
                Postdata = $"lsd={session.lsd}&jazoest={session.jazoest}&m_ts={session.m_ts}&li={session.li}&try_number=0&unrecognized_tries=0&email={facebookStatics.uid}&pass={facebookStatics.pass}&login=%C4%90%C4%83ng+nh%E1%BA%ADp";
                session.AddParamsLogin(Postdata);
                //Postdata = System.Net.WebUtility.UrlEncode(Postdata);
                ResponseLogin = session.http.Post(_svurl, Postdata, "application/x-www-form-urlencoded").ToString();
                string nh = "";
                if (facebookStatics.tfa == null)
                {
                    loginStatus = LoginStatus.SUCCESS;
                }
                if (ResponseLogin.Contains("checkpointSubmitButton"))
                {
                    nh = AppExtension.RegexByNameHtml(ResponseLogin, "nh", 1);
                    session.fb_dtsg = AppExtension.RegexByNameHtml(ResponseLogin, "fb_dtsg", 1);
                    string _2facode = AppExtension.tfacode(facebookStatics.tfa);
                    session.jazoest = AppExtension.RegexByNameHtml(ResponseLogin, "jazoest", 1);
                    Postdata = $"fb_dtsg={session.fb_dtsg}&jazoest={session.jazoest}&checkpoint_data=&approvals_code={_2facode}&codes_submitted=0&submit%5BSubmit+Code%5D=G%E1%BB%ADi+m%C3%A3&nh={nh}";
                    ResponseLogin = session.http.PostUrlEncoded(_mbasic_checkpoint_uri, Postdata).ToString();
                    Postdata = $"fb_dtsg={session.fb_dtsg}&jazoest={session.jazoest}&checkpoint_data=&name_action_selected=save_device&submit%5BContinue%5D=Ti%E1%BA%BFp+t%E1%BB%A5c&nh={nh}";
                    ResponseLogin = session.http.PostUrlEncoded(_mbasic_checkpoint_uri, Postdata).ToString();
                    if (ResponseLogin.Contains("checkpointSubmitButton"))
                    {
                        Postdata = $"fb_dtsg={session.fb_dtsg}&jazoest={session.jazoest}&checkpoint_data=&submit%5BContinue%5D=Ti%E1%BA%BFp+t%E1%BB%A5c&nh=" + nh;
                        ResponseLogin = session.http.PostUrlEncoded(_mbasic_checkpoint_uri, Postdata).ToString();
                        Postdata = $"fb_dtsg={session.fb_dtsg}&jazoest={session.jazoest}&checkpoint_data=&submit%5BThis+was+me%5D=%C4%90%C3%A2y+l%C3%A0+t%C3%B4i&nh=" + nh;
                        ResponseLogin = session.http.PostUrlEncoded(_mbasic_checkpoint_uri, Postdata).ToString();
                        Postdata = $"fb_dtsg={session.fb_dtsg}&jazoest={session.jazoest}&checkpoint_data=&name_action_selected=save_device&submit%5BContinue%5D=Ti%E1%BA%BFp+t%E1%BB%A5c&nh=" + nh;
                        ResponseLogin = session.http.PostUrlEncoded(_mbasic_checkpoint_uri, Postdata).ToString();
                    }
                    else
                    {
                        loginStatus = LoginStatus.SUCCESS;
                    }
                    if (ResponseLogin.Contains("verification_method"))
                    {
                        loginStatus = LoginStatus.CHECKPOINT;
                    }
                    else
                    {
                        loginStatus = LoginStatus.SUCCESS;
                    }
                }
                else
                {
                    loginStatus = LoginStatus.WRONG_PASS;
                }
            }
            catch
            {
                loginStatus = LoginStatus.ERROR;
                return loginStatus;
            }
            finally
            {
                AccessTokenStatus accessTokenStatus = LoginAutGetToken();
                if (accessTokenStatus == AccessTokenStatus.SUCCESS)
                {
                    facebookStatics.cookie = session.http.Cookies.ToString();
                    loginStatus = LoginStatus.SUCCESS;
                }
                else
                {
                    loginStatus = LoginStatus.ERROR;
                }
                FacebookSession = session;
                session.Dispose();
                Postdata = "";
                ResponseLogin = "";
            }
            return loginStatus;
        }

        public AccessTokenStatus LoginAutGetToken()
        {
            FacebookSession.http.UserAgent = AndroidUG;
            AccessTokenStatus accessTokenStatus;
            string ResponseConfirm = "";
            string postdata = "";
            string responseHeader = "";
            try
            {
                FacebookSession.http.AddHeader("x-requested-with", "app.yolikers.fb");
                string response = FacebookSession.http.Get(MbasicLoginAuthurl).ToString();
                //response = System.Net.WebUtility.UrlDecode(response);
                string fb_dtsgsd = Regex.Match(response, "name=\"fb_dtsg\" value=\"(.*?)\"").Groups[1].Value;
                string jazoest = Regex.Match(response, "name=\"jazoest\" value=\"(.*?)\"").Groups[1].Value;
                //string read = Regex.Match(response, "name=\"read\" value=\"(.*?)\"").Groups[1].Value;
                //string seen_scopes = Regex.Match(response, "name=\"seen_scopes\" value=\"(.*?)\"").Groups[1].Value;
                //string scope = Regex.Match(response, "scope=(.*?)&").Groups[1].Value;
                string logger_id = Regex.Match(response, "name=\"logger_id\" value=\"(.*?)\"").Groups[1].Value;
                //string __CONFIRM__ = Regex.Match(response, "__CONFIRM__\"><span class=\"(.*?)\">(.*?)</span></button></div>").Groups[2].Value;
                string encrypted_post_body = Regex.Match(response, "name=\"encrypted_post_body\" value=\"(.*?)\"").Groups[1].Value;
                //postdata = $"fb_dtsg={fb_dtsgsd}&jazoest={jazoest}&from_post=1&deduplicate=&link_customer_account=&read={read}&link_news_subscription=&write=&extended=&confirm=&reauthorize=&user_messenger_contact=&seen_scopes={seen_scopes}&auth_type=&auth_nonce=&cbt={AppExtension.GetTimestamp()}&default_audience=&dialog_type=gdp_v4&fbapp_pres=&ref=&ret=login&return_format=access_token&domain=&scope=&sso_device=&logger_id={logger_id}&sheet_name=initial&fallback_redirect_uri=&sdk=android-5.1.0&facebook_sdk_version=&sdk_version=&user_code=&nonce=&logged_out_behavior=&install_nonce=&l_nonce=&original_redirect_uri=&loyalty_program_id=&messenger_page_id=&reset_messenger_state=&aid=&deferred_redirect_uri=&code_redirect_uri=&extras=&tp=unspecified&fx_app=&encrypted_post_body={encrypted_post_body}&__CONFIRM__={__CONFIRM__}";
                //postdata = $"fb_dtsg={fb_dtsgsd}&jazoest={jazoest}&scope={scope}&display=touch&sdk=android-5.1.0&domain=&sso_device=&state=&user_code=&nonce=&logger_id={logger_id}&auth_type=&auth_nonce=&nonce=&return_format%5B%5D=access_token&encrypted_post_body={encrypted_post_body}";
                postdata = $"fb_dtsg={fb_dtsgsd}&jazoest={jazoest}&from_post=1&auth_type=&auth_nonce=&cbt=&default_audience=&dialog_type=gdp_v4&fbapp_pres=&ref=&ret=&return_format=access_token&domain=&scope=&sso_device=&logger_id={logger_id}a&sheet_name=initial&fallback_redirect_uri=&sdk=&facebook_sdk_version=&sdk_version=&user_code=&nonce=&logged_out_behavior=&install_nonce=&l_nonce=&original_redirect_uri=&loyalty_program_id=&messenger_page_id=&reset_messenger_state=&aid=&deferred_redirect_uri=&code_redirect_uri=&extras=&tp=unspecified&fx_app=&encrypted_post_body={encrypted_post_body}&__CONFIRM__=Continue";
                response = "";
                FacebookSession.http.AddHeader("content-length", postdata.Length.ToString());
                FacebookSession.http.AddHeader("referer", MbasicLoginAuthurl);
                FacebookSession.http.AddHeader("content-type", "application/x-www-form-urlencoded");
                //FacebookSession.http.AddHeader("x-requested-with", "app.yolikers.fb");
                ResponseConfirm = FacebookSession.http.Post(OAUTH_CONFIRM_URL, postdata, "application/x-www-form-urlencoded").ToString(); //MBASIC_OAUTH_CONFIRM_URL
                //FacebookAPI.LoginFBApp();
                //responseHeader = FacebookSession.http.GetResponeHeader();
            }
            catch(Exception ex)
            {
                //responseHeader = FacebookSession.http.GetResponeHeader();
                accessTokenStatus = AccessTokenStatus.DIE;
            }
            finally
            {
                if (ResponseConfirm.Contains("success#"))
                {
                    facebookStatics.access_token = Regex.Match(ResponseConfirm, "access_token=(.*?)&").Groups[1].Value;
                    accessTokenStatus = AccessTokenStatus.SUCCESS;
                }
                else
                {
                    accessTokenStatus = AccessTokenStatus.DIE;
                }
                responseHeader = "";
                postdata = "";
                ResponseConfirm = "";
            }
            return accessTokenStatus;
        }
    }
}
