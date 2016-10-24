using Lawyers.BizEntities;
using System.Collections.Generic;
using Valon.Framework.DataAccess;

namespace Mango.DataAccess
{
    public class MaterialDA
    {
        public static QueryResultList<MaterialData> GetItems(QueryRequest<MaterialQueryData> query)
        {
            var result = new QueryResultList<MaterialData>();
            DataCommand cmd = DataCommandManager.GetDataCommand("Material_GetItems");
            cmd.SetParameterValue("@Type", query.Body.Type);
            cmd.ReplaceParameterValue("@PageSize", query.PageInfo.PageSize + "");
            //cmd.SetParameterValue("@PageCurrent", query.PageInfo.PageIndex);
            //cmd.SetParameterValue("@PageSize", query.PageInfo.PageSize);
            //cmd.SetParameterValue("@SortType", query.PageInfo.SortType);
            //cmd.SetParameterValue("@SortField", query.PageInfo.SortField);
            result.Body = cmd.ExecuteEntityList<MaterialData>();
            //result.TotalCount = (int)cmd.GetParameterValue("@TotalCount");
            return result;
        }


        public static int AddMaterial(RequestOperation<MaterialData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Material_AddMaterial");
            cmd.SetParameterValue("@Type", request.Body.Type);
            cmd.SetParameterValue("@Url", request.Body.Url);
            return cmd.ExecuteNonQuery();
        }


    }
}
