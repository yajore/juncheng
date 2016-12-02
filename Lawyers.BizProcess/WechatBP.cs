using Lawyers.BizEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaolon.Wechat.Framework;
using Vaolon.Wechat.Framework.Api;
using Vaolon.Wechat.Framework.Model;
using Vaolon.Wechat.Framework.Model.ApiRequests;

namespace Lawyers.BizProcess
{
    public class WechatBP
    {

        private static IApiClient m_client = new DefaultApiClient();
        private static AppIdentication m_appIdent = new AppIdentication(
            ConfigurationManager.AppSettings["wxappid"],
            ConfigurationManager.AppSettings["wxappsecret"]);

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OperationResult SendTemplate(RequestOperation<TemplateData> request)
        {


            var result = new OperationResult();

            if (String.IsNullOrEmpty(request.Body.openid) ||
                String.IsNullOrEmpty(request.Body.templateid) ||
                String.IsNullOrEmpty(request.Body.url) ||
                 request.Body.propertys == null)
            {
                result.Message = "缺少参数";
                return result;
            }

            var tempData = new Dictionary<string, TemplateDataProperty>();

            foreach (var prop in request.Body.propertys)
            {
                tempData.Add(prop.key, new TemplateDataProperty()
                {
                    Color = prop.color,
                    Value = prop.value
                });
            }
            var tempRequest = new TemplateSendRequest
            {
                ToUser = request.Body.openid,
                TemplateID = request.Body.templateid,
                Url = request.Body.url,
                TopColor = request.Body.topcolor,
                Data = tempData,
                AccessToken = ApiAccessTokenManager.Instance.GetCurrentToken()
            };

            var response = m_client.Execute(tempRequest);
            if (!response.IsError)
            {
                result.ErrCode = 0;
            }
            else
            {
                result.Message = response.ErrorMessage;
            }
            return result;
        }
    }
}
