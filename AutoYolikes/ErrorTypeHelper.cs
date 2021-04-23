using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoYolikes.FB;
using AutoYolikes.YolikerApp;
using AutoYolikes.CaptchaService;

namespace AutoYolikes
{
    internal static class ErrorTypeHelper
    {
        public static readonly Dictionary<FacebookStatics.LoginStatus, string> FacebookErrorList = new Dictionary<FacebookStatics.LoginStatus, string>
        {
            {
                FacebookStatics.LoginStatus.CHECKPOINT, "facebook checkpoint"
            },
            {
                FacebookStatics.LoginStatus.WRONG_PASS, "sai mật khẩu facebook"
            },
            {
                FacebookStatics.LoginStatus.ERROR, "lỗi đăng nhập..."
            },
            {
                FacebookStatics.LoginStatus.SUCCESS, "đăng nhập thành công !"
            }
        };
        public static readonly Dictionary<YolikerStatics.YolikerState, string> YolikerErrorList = new Dictionary<YolikerStatics.YolikerState, string>
        {
            {
                YolikerStatics.YolikerState.CAPTCHA_FAILED, "giải captcha thất bại..."
            },
            {
                YolikerStatics.YolikerState.ERROR, "lỗi không xác định..."
            },
            {
                YolikerStatics.YolikerState.FAIL , "nhiệm vụ thất bại..."
            },
            {
                YolikerStatics.YolikerState.LIMITED, "bị giới hạn... chờ 1 chút nữa"
            },
            {
                YolikerStatics.YolikerState.SUCCESS, "nhiệm vụ thành công !"
            },
            {
                YolikerStatics.YolikerState.CLOUD_FLARE, "bị dính cloufare rồi... chờ chút !"
            }
        };
        public static string GetDescriptionYolikerState(YolikerStatics.YolikerState yolikerState)
        {
            return YolikerErrorList[yolikerState];
        }
        public static string GetDescriptionFacebookState(FacebookStatics.LoginStatus loginStatus)
        {
            return FacebookErrorList[loginStatus];
        }
    }
}
