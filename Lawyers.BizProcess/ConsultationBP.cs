using Lawyers.BizEntities;
using Lawyers.Utilities;
using Mango.DataAccess;
using System;
using System.Collections.Generic;

namespace Lawyers.BizProcess
{
    public class ConsultationBP
    {
        public QueryResultList<ConsultationShowData> GetConsultations(QueryRequest<ConsultationQueryData> query)
        {
            var result = new QueryResultList<ConsultationShowData>();

            try
            {

                result = ConsultationDA.GetConsultations(query);
                if (result.Body == null)
                {
                    result.Body = new List<ConsultationShowData>();
                }
                result.ErrCode = 0;
                result.Message = "ok";

            }
            catch (Exception ex)
            {
                Logger.WriteException("GetConsultations", ex, query);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }


        public OperationResult<int> AddConsultation(RequestOperation<ConsultationData> request)
        {
            var result = new OperationResult<int>();

            try
            {

                if (String.IsNullOrEmpty(request.Body.Mobile) ||
                    String.IsNullOrEmpty(request.Body.Code) ||
                      String.IsNullOrEmpty(request.Body.Contents))
                {
                    result.Message = "缺少请求参数";
                    return result;
                }

                var verifyCode = new MsgBP().VerifyCode(new RequestOperation<ReqMsgData>()
                {
                    Header = request.Header,
                    Body = new ReqMsgData()
                    {
                        Mobile = request.Body.Mobile,
                        Code = request.Body.Code
                    }
                });

                if (verifyCode.ErrCode != 0)
                {
                    result.Message = verifyCode.Message;
                    return result;
                }

                request.Body.ConStatus = 1;
                var data = ConsultationDA.AddConsultation(request);

                if (data <= 0)
                {
                    result.ErrCode = 1;
                    result.Message = "添加留言信息失败";
                }
                else
                {
                    result.ErrCode = 0;
                    result.Message = "ok";

                    var law = new CustomerBP().GetCustomerById(new RequestOperation<int>()
                    {
                        Header = request.Header,
                        Body = request.Body.ToLawyer
                    });

                    if (law.ErrCode == 0)
                    {
                        var send = new MsgBP().SendNotify(new RequestOperation<ReqMsgData>()
                        {
                            Header = request.Header,
                            Body = new ReqMsgData()
                            {
                                MsgType = 2,
                                Mobile = law.Body.Mobile
                            }
                        });
                        if (send.ErrCode != 0)
                        {
                            Logger.WriteException("AddConsultation3", new Exception(send.Message), request);
                        }
                    }
                    else
                    {
                        Logger.WriteException("AddConsultation2", new Exception(law.Message), request);
                    }

                }


            }
            catch (Exception ex)
            {
                Logger.WriteException("AddConsultation", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult SetConsultationStatus(RequestOperation<ConsultationData> request)
        {
            var result = new OperationResult<int>();

            try
            {

                if (request.Body.ConStatus <= 0 ||
                    request.Body.Sysno <= 0
                    )
                {
                    result.Message = "缺少请求参数";
                    return result;
                }


                var data = ConsultationDA.SetConsultationStatus(request);

                if (data <= 0)
                {
                    result.ErrCode = 1;
                    result.Message = "更新留言状态失败";
                }
                else
                {
                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                Logger.WriteException("SetConsultationStatus", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public QueryResultList<ConsultationShowData> GetUserCon(QueryRequest<int> query)
        {
            var result = new QueryResultList<ConsultationShowData>();

            try
            {

                result = ConsultationDA.GetUserCon(query);
                if (result.Body == null)
                {
                    result.Body = new List<ConsultationShowData>();
                }
                result.ErrCode = 0;
                result.Message = "ok";

            }
            catch (Exception ex)
            {
                Logger.WriteException("GetUserCon", ex, query);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult SetReply(RequestOperation<ConsultationReplyData> request)
        {
            var result = new OperationResult<int>();

            try
            {


                var data = ConsultationDA.SetReply(request);

                if (data <= 0)
                {
                    result.ErrCode = 1;
                    result.Message = "设置留言失败";
                }
                else
                {
                    //设置模板消息
                    if (request.Body.IsNotify)
                    {

                        var liveUser = ConsultationDA.GetConUser(request.Body.ConsultationID);
                        if (liveUser == null || String.IsNullOrEmpty(liveUser.WechatAccount))
                        {
                            Logger.WriteException("SetReply", new Exception("查找留言用户失败"), request);
                        }
                        else
                        {

                            var reqTemp = new RequestOperation<TemplateData>();
                            reqTemp.Header = request.Header;
                            reqTemp.Body = new TemplateData();
                            reqTemp.Body.topcolor = "#1650a2";
                            reqTemp.Body.openid = liveUser.WechatAccount;
                            reqTemp.Body.templateid = "Fkr0tQpC5fCQUJR4QjrMa4A7a6PtMaHu8-BKPPrcLa0";
                            reqTemp.Body.url = "http://jc.webui.info/my-content.html?cid=" + request.Body.ConsultationID;
#if DEBUG
                            reqTemp.Body.templateid = "xSGVt_YQ8EZu8VD8yUoRqQJtKTkvjoRCOKqqY-8M4vQ";
                            reqTemp.Body.url = "http://mango.webui.info/law/my-content.html?cid=" + request.Body.ConsultationID;
#endif
                            reqTemp.Body.propertys = new List<TemplatePropertyData>();
                            reqTemp.Body.propertys.Add(new TemplatePropertyData()
                            {
                                color = "#000000",
                                key = "first",
                                value = liveUser.LawyerName + "律师已经针对您的咨询做出了解答，部分信息如下"
                            });
                            reqTemp.Body.propertys.Add(new TemplatePropertyData()
                            {
                                color = "#000000",
                                key = "keyword1",
                                value = liveUser.LawyerName
                            });
                            reqTemp.Body.propertys.Add(new TemplatePropertyData()
                            {
                                color = "#000000",
                                key = "keyword2",
                                value = liveUser.Reply.Length > 30 ? liveUser.Reply.Substring(0, 30) + "..." : liveUser.Reply
                            });
                            //18857303534
                            string mobile = "";
                            if (!String.IsNullOrEmpty(liveUser.LawyerMobile) && liveUser.LawyerMobile.Length == 11)
                            {
                                mobile = liveUser.LawyerMobile.Substring(0, 3) + "****" + liveUser.LawyerMobile.Substring(6, 4);
                            }

                            reqTemp.Body.propertys.Add(new TemplatePropertyData()
                            {
                                color = "#000000",
                                key = "keyword3",
                                value = mobile
                            });

                            reqTemp.Body.propertys.Add(new TemplatePropertyData()
                            {
                                color = "#000000",
                                key = "keyword4",
                                value = ""
                            });
                            reqTemp.Body.propertys.Add(new TemplatePropertyData()
                            {
                                color = "#000000",
                                key = "keyword5",
                                value = liveUser.Contents.Length > 30 ? liveUser.Contents.Substring(0, 30) + "..." : liveUser.Contents
                            });
                            reqTemp.Body.propertys.Add(new TemplatePropertyData()
                            {
                                color = "#000000",
                                key = "remark",
                                value = "查看更多详情"
                            });
                            var send = new WechatBP().SendTemplate(reqTemp);
                            if (send.ErrCode != 0)
                            {
                                Logger.WriteException("SetReply", new Exception(send.Message), reqTemp);
                            }
                        }

                    }

                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                Logger.WriteException("SetReply", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult<ConsultationShowData> GetReplyById(QueryRequest<int> query)
        {
            var result = new OperationResult<ConsultationShowData>();

            try
            {

                var data = ConsultationDA.GetReplyById(query);
                if (data == null)
                {
                    result.Message = "没有获取到留言信息";
                    return result;
                }
                result.Body = data;

                result.ErrCode = 0;
                result.Message = "ok";

            }
            catch (Exception ex)
            {
                Logger.WriteException("GetReplyById", ex, query);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

    }
}
