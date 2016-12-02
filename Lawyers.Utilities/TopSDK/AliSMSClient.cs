using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Sms.Model.V20160927;
using System.Collections.Generic;
using System.Configuration;

namespace Lawyers.Utilities
{
    public class AliSMSClient
    {
        static string sms_oss = ConfigurationManager.AppSettings["sms_oss"];
        static string sms_key = ConfigurationManager.AppSettings["sms_appkey"];
        static string sms_secret = ConfigurationManager.AppSettings["sms_secret"];
        static string sms_sign = ConfigurationManager.AppSettings["sms_sign"];
        static Dictionary<int, string> sms_template = new Dictionary<int, string> {
            {1,"SMS_21695196" },
            {2,"SMS_25650738" }
        };

        public static string Send(string mobiles, string param, int smstypeid)
        {
            string result = "-1,发送短信失败";
            IClientProfile profile = DefaultProfile.GetProfile(sms_oss, sms_key, sms_secret);
            IAcsClient client = new DefaultAcsClient(profile);
            SingleSendSmsRequest request = new SingleSendSmsRequest();
            try
            {
                request.SignName = sms_sign;
                request.TemplateCode = sms_template[smstypeid]; ;
                request.RecNum = mobiles;
                request.ParamString = param;
                SingleSendSmsResponse httpResponse = client.GetAcsResponse(request);
                result = "0,OK";
            }
            catch (ServerException e)
            {
                result = "-1," + e.Message;
            }
            catch (ClientException e)
            {
                result = "-1," + e.Message;
            }
            return result;
        }
    }

}
