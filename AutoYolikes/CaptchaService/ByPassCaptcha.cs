using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;
using System.Text.RegularExpressions;
namespace AutoYolikes.CaptchaService
{
    public class ByPassCaptcha
    {
        public static string urlT = "https://www.google.com/recaptcha/api2/anchor?ar=1&k=6Lc4D68ZAAAAACCX57dQFmEBGNr1VOmOX228Zwv8&co=aHR0cHM6Ly9hcHAubXAzc29uZ3MuZGVzaTo0NDM.&hl=vi&v=mrdLhN7MywkJAAbzddTIjTaM&size=normal&cb=92qzwtgq95c9";
        public static string GetTokenCaptcha()
        {
            urlT = "https://www.google.com/recaptcha/enterprise/anchor?ar=1&k=6Lc4D68ZAAAAACCX57dQFmEBGNr1VOmOX228Zwv8&co=aHR0cHM6Ly9hcHAubXAzc29uZ3MuZGVzaTo0NDM.&hl=en&v=mrdLhN7MywkJAAbzddTIjTaM&size=normal&cb=92qzwtgq95c9";
            string result = "";
            string response = "";
            HttpRequest httpRequest = new HttpRequest();
            try
            {
                response = httpRequest.Get(urlT).ToString();
                result = Regex.Match(response, "id=\"recaptcha-token\" value=\"(.*?)\"").Groups[1].Value;
            }
            catch
            {
                result = "error";
            }
            finally
            {
                response = "";
            }
            return result;
        }
    }
}
