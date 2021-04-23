using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;

namespace AutoYolikes.YolikerApp
{
    public class Session
    {
        public HttpRequest http;
        public string fb_dtsg;
        public string lsd;
        public string jazoest;
        public string m_ts;
        public string li;

        public void CreateSession()
        {
            http = new HttpRequest();
            http.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            http.AllowAutoRedirect = true;
            http.KeepAlive = true;
            http.IgnoreProtocolErrors = true;
            http.Cookies = new CookieDictionary(false);
        }
        public void InitParamsCodeLogin(string html)
        {
            fb_dtsg = AppExtension.RegexByNameHtml(html, "fb_dtsg", 1);
            lsd = AppExtension.RegexByNameHtml(html, "lsd", 1);
            jazoest = AppExtension.RegexByNameHtml(html, "jazoest", 1);
            m_ts = AppExtension.RegexByNameHtml(html, "m_ts", 1);
            li = AppExtension.RegexByNameHtml(html, "li", 1);
        }
        public void AddParamsLogin(string postdata)
        {
            try
            {
                string[] arraypostdata = postdata.Split('&');
                foreach (string param in arraypostdata)
                {
                    string[] par_ = param.Split('=');
                    http.AddParam(par_[0], par_[1]);
                }
            }
            catch { }
        }
        public void Dispose()
        {
            fb_dtsg = null;
            jazoest = null;
            lsd = null;
            m_ts = null;
            li = null;
            http.Close();
            http.Dispose();
        }
    }
}
