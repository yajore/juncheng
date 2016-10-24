using Lawyers.BizEntities;
using Mango.DataAccess;
using System;
using System.Collections.Generic;
using Lawyers.Utilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawyers.BizProcess
{
    public class MaterialBP
    {
        public QueryResultList<MaterialData> GetItems(QueryRequest<MaterialQueryData> query)
        {
            var result = new QueryResultList<MaterialData>();

            try
            {

                result = MaterialDA.GetItems(query);
                if (result.Body == null)
                {
                    result.Body = new List<MaterialData>();
                }
                result.ErrCode = 0;
                result.Message = "ok";

            }
            catch (Exception ex)
            {
                Logger.WriteException("GetItems", ex, query);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }


        public OperationResult AddMaterial(RequestOperation<MaterialData> request)
        {
            var result = new OperationResult();

            try
            {

                if (String.IsNullOrEmpty(request.Body.Url))
                {
                    result.Message = "缺少请求参数";
                    return result;
                }

                var data = MaterialDA.AddMaterial(request);

                if (data <= 0)
                {
                    result.ErrCode = 1;
                    result.Message = "添加资源失败";
                }
                else
                {
                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                Logger.WriteException("AddMaterial", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }


        public void AddMaterialV2(RequestOperation<MaterialData> request)
        {
            AddMaterial(request);
        }

        delegate void SyncAddMaterial(RequestOperation<MaterialData> request);


        public void AddMaterialSync(RequestOperation<MaterialData> request)
        {

            SyncAddMaterial sync = new SyncAddMaterial(AddMaterialV2);
            sync.BeginInvoke(request, null, null);


        }

    }
}
