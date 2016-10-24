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
                Logger.WriteException("GetConsultations", ex, query);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

    }
}
