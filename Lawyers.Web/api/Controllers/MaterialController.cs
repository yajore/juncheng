using Lawyers.BizEntities;
using Lawyers.BizProcess;
using Lawyers.Utilities;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Lawyers.Web.Controllers
{

    public class MaterialController : ApiController
    {


        [HttpPost]
        public QueryResultList<MaterialData> Items([FromBody]string queryString)
        {
            var result = new QueryResultList<MaterialData>();


            var request = JsonHelper.Build<QueryRequest<MaterialQueryData>>(queryString);

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
            result = new MaterialBP().GetItems(request);
            return result;
        }


    }
}
