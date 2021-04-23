using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoYolikes.CaptchaService;
using System.Drawing;
using AutoYolikes.FB;
namespace AutoYolikes.YolikerApp
{
    public class YolikerStatics:FacebookStatics
    {
        public static string GooglePublicKey = "6Lc4D68ZAAAAACCX57dQFmEBGNr1VOmOX228Zwv8";
        public static string LikeCustomUri = "";
        public static Color NormalColor = Color.FromArgb(212, 241, 183);
        public static Color WarningColor = Color.FromArgb(247, 175, 195);
        public static string LoginUrl = "";
        public int NumLikeDelivered;
        public string IdPost;
        public string TypeAction;
        public CaptchaStatics.CaptchaService captchaSvName;
        public enum YolikerState
        {
            SUCCESS,
            FAIL,
            ERROR,
            LIMITED,
            CAPTCHA_FAILED,
            CLOUD_FLARE
        }
        
    }
}
