using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoYolikes.FB
{
    public class FacebookStatics
    {
        public static string FBAPPID = "437340816620806";
        public string uid;
        public string pass;
        public string tfa;
        public string access_token;
        public string cookie;
        public bool IssCookie;
        public static string LoginFBUrl = "https://mbasic.facebook.com/login";
        public static string _svurl = "https://mbasic.facebook.com/login/device-based/regular/login/?refsrc=https%3A%2F%2Fmbasic.facebook.com%2F&lwv=100&refid=8";
        public static string _mbasic_checkpoint_uri = "https://mbasic.facebook.com/login/checkpoint/";
        public static string AndroidUG = "Mozilla/5.0 (Linux; Android 7.1.2; SM-G960N Build/N2G48B; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/68.0.3440.70 Mobile Safari/537.36";
        public static string BMUA = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.72 Safari/537.36";
        public static string BMURI = "https://business.facebook.com/business_locations/?nav_source=mega_menu";
        public static string AndroidLoginAuthUrl = "";
        public static string MbasicLoginAuthurl = $"https://m.facebook.com/dialog/oauth?app_id={FBAPPID}&redirect_uri=fbconnect://success&response_type=token"; //https://mbasic.facebook.com/v7.0/dialog/oauth?client_id=124024574287414&redirect_uri=fbconnect%3A%2F%2Fsuccess&response_type=token&sdk=android-5.1.0
        //public static string OAUTH_CONFIRM_URL = "https://m.facebook.com/v7.0/dialog/oauth/confirm/";
        public static string OAUTH_CONFIRM_URL = "https://m.facebook.com/v2.0/dialog/oauth/confirm/";
        public static string MBASIC_OAUTH_CONFIRM_URL = "https://mbasic.facebook.com/dialog/oauth/skip/submit/";
        public enum AccessTokenStatus
        {
            DIE,
            SUCCESS
        }
        public enum LoginStatus
        {
            SUCCESS,
            CHECKPOINT,
            WRONG_PASS,
            ERROR
        }
        public void dispose()
        {
            uid = "";
            pass = "";
            tfa = "";
            access_token = "";
            cookie = "";
        }
    }
}
