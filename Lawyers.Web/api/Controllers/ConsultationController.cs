using Lawyers.BizEntities;
using Lawyers.BizProcess;
using Lawyers.Utilities;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Lawyers.Web.Controllers
{
    /// <summary>
    /// 咨询
    /// </summary>
    public class ConsultationController : ApiController
    {

        [HttpPost]
        public QueryResultList<ConsultationShowData> GetItems([FromBody]string queryString)
        {
            var result = new QueryResultList<ConsultationShowData>();


            var request = JsonHelper.Build<QueryRequest<ConsultationQueryData>>(queryString);

            if (request.Body == null)
            {
                result.Message = "请求参数为NULL";
                return result;
            }

            if (request.PageInfo == null)
            {
                result.Message = "分页参数为NULL";
                return result;
            }


            var verify = ValidaQueryString.ValidaDevice(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = new ConsultationBP().GetConsultations(request);
            return result;
        }

        [HttpPost]
        public OperationResult AddConsultation([FromBody]string queryString)
        {
            var result = new OperationResult();

            var request = JsonHelper.Build<RequestOperation<ConsultationData>>(queryString);

            if (request == null)
            {
                result.Message = "请求参数为NULL";
                return result;
            }

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = new ConsultationBP().AddConsultation(request);
            return result;
        }

        [HttpPost]
        public OperationResult SetConsultationStatus([FromBody]string queryString)
        {
            var result = new OperationResult();

            var request = JsonHelper.Build<RequestOperation<ConsultationData>>(queryString);

            if (request == null)
            {
                result.Message = "请求参数为NULL";
                return result;
            }

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = new ConsultationBP().SetConsultationStatus(request);
            return result;
        }

        [HttpPost]
        public QueryResultList<ConsultationShowData> My([FromBody]string queryString)
        {
            var result = new QueryResultList<ConsultationShowData>();


            var request = JsonHelper.Build<QueryRequest<int>>(queryString);

            if (request.Body <= 0)
            {
                result.Message = "请求参数为NULL";
                return result;
            }

            if (request.PageInfo == null)
            {
                result.Message = "分页参数为NULL";
                return result;
            }


            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = new ConsultationBP().GetUserCon(request);
            return result;
        }

        [HttpPost]
        public OperationResult SetReply([FromBody]string queryString)
        {
            var result = new OperationResult();

            var request = JsonHelper.Build<RequestOperation<ConsultationReplyData>>(queryString);

            if (request == null)
            {
                result.Message = "请求参数为NULL";
                return result;
            }

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = new ConsultationBP().SetReply(request);
            return result;
        }

        [HttpPost]
        public OperationResult<ConsultationShowData> GetReplyById([FromBody]string queryString)
        {
            var result = new OperationResult<ConsultationShowData>();

            var request = JsonHelper.Build<QueryRequest<int>>(queryString);

            if (request == null)
            {
                result.Message = "请求参数为NULL";
                return result;
            }

            var verify = ValidaQueryString.Valida(request.Header);

            if (verify.ErrCode != 0)
            {
                result.ErrCode = verify.ErrCode;
                result.Message = verify.Message;
                return result;

            }
            result = new ConsultationBP().GetReplyById(request);
            return result;
        }

    }
}
