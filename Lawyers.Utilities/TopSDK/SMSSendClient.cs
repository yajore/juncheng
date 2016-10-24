using System.Collections.Generic;
using System.Configuration;
using Top.Api;
using Top.Api.Request;

namespace Lawyers.Utilities
{
    public class SMSClient
    {
        static string sms_url = ConfigurationManager.AppSettings["sms_url"];
        static string sms_appkey = ConfigurationManager.AppSettings["sms_appkey"];
        static string sms_secret = ConfigurationManager.AppSettings["sms_secret"];
        static string sms_sign = ConfigurationManager.AppSettings["sms_sign"];
        static Dictionary<int, string> sms_template = new Dictionary<int, string> {
            {1,"SMS_11066441" },
            {2,"SMS_11066443"}
        };

        public static string Send(string mobiles, string param, int smstypeid)
        {

            string result = "-1,发送短信失败";
            var req = new AlibabaAliqinFcSmsNumSendRequest();
            try
            {

                if (sms_template.ContainsKey(smstypeid) == false)
                {
                    throw new System.Exception("短信类型不正确");
                }
                var client = new DefaultTopClient(sms_url, sms_appkey, sms_secret);
                req.SmsType = "normal";
                req.SmsFreeSignName = sms_sign;
                req.SmsParam = param;
                req.RecNum = mobiles;
                req.SmsTemplateCode = sms_template[smstypeid];
                var rsp = client.Execute(req);
                if (rsp.IsError == false)
                {
                    result = "0,OK";
                }
                else
                {
                    result = rsp.ErrCode + "," + rsp.ErrMsg;
                }
            }
            catch (System.Exception ex)
            {
                result = "-1," + ex.Message;
                Logger.WriteException("【阿里大鱼短信接口】", ex, req);
            }

            return result;
        }
    }
}
