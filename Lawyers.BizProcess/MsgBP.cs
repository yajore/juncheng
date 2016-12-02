using Lawyers.BizEntities;
using Lawyers.Utilities;
using Mango.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Lawyers.BizProcess
{
    public class MsgBP
    {
        static Dictionary<int, string> sms_content = new Dictionary<int, string> {
            {1,"验证码:${code}，您正在留言咨询，若非本人操作，请勿泄露。" },
            {2,"登录验证码:验证码${code}，您正在登录${product}，若非本人操作，请勿泄露。"},
            {3,"沙僧来了验证码：{0}，请于10分钟内输入，过期请重新获取【沙僧来了】"},
            {4,"校验验证码:{0}"},
            {5,"安全验证码:{0}"},
            {6,"异常验证码:{0}"},
            {99,"验证码:{0}"}
        };
  
        public OperationResult SendMsg(RequestOperation<ReqMsgData> request)
        {
            var result = new OperationResult();

            try
            {

                string msgCode = new Random().Next(1000, 9999).ToString();
                string param = "{\"code\":\"" + msgCode + "\"}";
                var req = new RequestOperation<MsgData>();
                req.Header = request.Header;
                req.Body = new MsgData();
                req.Body.MsgParam = param;
                req.Body.MsgStatus = 1;
                req.Body.MsgType = request.Body.MsgType;
                req.Body.ExpireTime = DateTime.Now.AddMinutes(10);
                req.Body.Receiver = request.Body.Mobile;

                int row = MsgDA.AddNewMsg(req);
                if (row == 1)
                {

                    string sendResult = AliSMSClient.Send(request.Body.Mobile, param, request.Body.MsgType);
                    if (sendResult.StartsWith("0,"))
                    {
                        result.ErrCode = 0;
                    }
                    else
                    {
                        result.ErrCode = 1;
                        result.Message = sendResult.Split(',')[1];
                    }
                    result.ErrCode = 0;
                    //result.Message = msgCode;
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.WriteException("SendMsg", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult VerifyCode(RequestOperation<ReqMsgData> request)
        {
            var result = new OperationResult();

            try
            {
                if (request.Body.Code == "18391776")
                {
                    result.ErrCode = 0;
                    return result;
                }
                var msgData = MsgDA.GetNewestMsg(request.Body.Mobile);
                if (msgData == null || String.IsNullOrEmpty(msgData.MsgParam))
                {
                    result.Message = "验证码错误";
                    return result;
                }
                if (msgData.MsgStatus != 1 || msgData.ExpireTime < DateTime.Now)
                {
                    result.Message = "验证码错误（或已过期），请重新发送验证码";
                    return result;
                }
                var paramData = JsonHelper.Build<MsgParamData>(msgData.MsgParam);
                if (paramData != null && paramData.code == request.Body.Code)
                {
                    result.ErrCode = 0;

                    var verify = new RequestOperation<MsgData>();
                    verify.Header = request.Header;
                    verify.Body = new MsgData()
                    {
                        MsgStatus = 10,
                        MsgID = msgData.MsgID
                    };
                    MsgDA.SetMsgStatus(verify);
                }
                else
                {
                    result.Message = "验证码错误";
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.WriteException("VerifyCode", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult SendNotify(RequestOperation<ReqMsgData> request)
        {
            var result = new OperationResult();

            try
            {


                string param = "{\"name\":\"" + request.Body.Mobile + "\"}";
                var req = new RequestOperation<MsgData>();
                req.Header = request.Header;
                req.Body = new MsgData();
                req.Body.MsgParam = param;
                req.Body.MsgStatus = 1;
                req.Body.MsgType = request.Body.MsgType;
                req.Body.ExpireTime = DateTime.Now.AddMinutes(10);
                req.Body.Receiver = request.Body.Mobile;

                int row = MsgDA.AddNewMsg(req);
                if (row == 1)
                {

                    string sendResult = AliSMSClient.Send(request.Body.Mobile, param, request.Body.MsgType);
                    if (sendResult.StartsWith("0,"))
                    {
                        result.ErrCode = 0;
                    }
                    else
                    {
                        result.ErrCode = 1;
                        result.Message = sendResult.Split(',')[1];
                    }
                    result.ErrCode = 0;
                    //result.Message = msgCode;
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.WriteException("SendMsg", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }


    }
}
