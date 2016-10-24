using Lawyers.BizEntities;
using System.Collections.Generic;
using Valon.Framework.DataAccess;
using System.Linq;
using System.Data;

namespace Mango.DataAccess
{
    public class ArticleDA
    {
        public static QueryResultList<ArticleData> GetItems(QueryRequest<ArticleQueryData> query)
        {
            var result = new QueryResultList<ArticleData>();
            DataCommand cmd = DataCommandManager.GetDataCommand("Article_GetItems");
            cmd.SetParameterValue("@ArtStatus", query.Body.ArtStatus);
            cmd.SetParameterValue("@Publisher", query.Body.Publisher + "%");
            cmd.SetParameterValue("@Publisher", query.Body.EndDate);
            cmd.SetParameterValue("@Title", query.Body.Title + "%");
            cmd.SetParameterValue("@ArtType", query.Body.ArtType);
            cmd.SetParameterValue("@StartDate", query.Body.StartDate);
            cmd.SetParameterValue("@EndDate", query.Body.EndDate);

            cmd.SetParameterValue("@PageCurrent", query.PageInfo.PageIndex);
            cmd.SetParameterValue("@PageSize", query.PageInfo.PageSize);
            cmd.SetParameterValue("@SortType", query.PageInfo.SortType);
            cmd.SetParameterValue("@SortField", query.PageInfo.SortField);
            result.Body = cmd.ExecuteEntityList<ArticleData>();
            result.TotalCount = (int)cmd.GetParameterValue("@TotalCount");
            return result;
        }

        public static QueryResultList<ArticleShowData> GetShowItems(QueryRequest<string> query)
        {
            var result = new QueryResultList<ArticleShowData>();
            DataCommand cmd = DataCommandManager.GetDataCommand("Article_GetShowItems");

            string[] types = query.Body.Split(',');
            for (int i = 0; i < types.Length; i++)
            {
                cmd.SetParameterValue("@ArtType" + (i+1), types[i]);
            }
            cmd.ReplaceParameterValue("@PageSize", query.PageInfo.PageSize + "");
            result.Body = cmd.ExecuteEntityList<ArticleShowData>();
            //result.TotalCount = (int)cmd.GetParameterValue("@TotalCount");
            return result;
        }

        public static List<ArticleShowData> GetArticleByKey(QueryRequest<string> query)
        {

            DataCommand cmd = DataCommandManager.GetDataCommand("Article_SearchKey");
            cmd.SetParameterValue("@KeyWrod", "%" + query.Body + "%");


            cmd.SetParameterValue("@PageCurrent", query.PageInfo.PageIndex);
            cmd.SetParameterValue("@PageSize", query.PageInfo.PageSize);
            cmd.SetParameterValue("@SortType", query.PageInfo.SortType);
            cmd.SetParameterValue("@SortField", query.PageInfo.SortField);
            return cmd.ExecuteEntityList<ArticleShowData>();
        }

        public static QueryResultList<ArticleShowData> GetDailyNews(QueryRequest<ArticleQueryData> query)
        {
            var result = new QueryResultList<ArticleShowData>();
            DataCommand cmd = DataCommandManager.GetDataCommand("Article_GetDailyNews");
            cmd.SetParameterValue("@ArtType", query.Body.ArtType);
            cmd.SetParameterValue("@StartDate", query.Body.StartDate);
            cmd.SetParameterValue("@EndDate", query.Body.EndDate);

            cmd.SetParameterValue("@PageCurrent", query.PageInfo.PageIndex);
            cmd.SetParameterValue("@PageSize", query.PageInfo.PageSize);
            cmd.SetParameterValue("@SortType", query.PageInfo.SortType);
            cmd.SetParameterValue("@SortField", query.PageInfo.SortField);
            result.Body = cmd.ExecuteEntityList<ArticleShowData>();
            result.TotalCount = (int)cmd.GetParameterValue("@TotalCount");
            return result;
        }

        public static EntryID AddArticle(RequestOperation<ArticleData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Article_Add");
            cmd.SetParameterValue("@GroupType", request.Body.GroupType);
            cmd.SetParameterValue("@ArtType", request.Body.ArtType);
            cmd.SetParameterValue("@Title", request.Body.Title);
            cmd.SetParameterValue("@Summary", request.Body.Summary);
            cmd.SetParameterValue("@Cover", request.Body.Cover);
            cmd.SetParameterValue("@Contents", request.Body.Contents);
            cmd.SetParameterValue("@Link", request.Body.Link);
            cmd.SetParameterValue("@PublisherID", request.Header.UserID);
            cmd.SetParameterValue("@ArtStatus", request.Body.ArtStatus);
            cmd.SetParameterValue("@InUser", request.Header.DisplayName);
            return cmd.ExecuteEntity<EntryID>();
        }

        public static int UpdateArticle(RequestOperation<ArticleData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Article_Update");
            cmd.SetParameterValue("@GroupType", request.Body.GroupType);
            cmd.SetParameterValue("@ArtType", request.Body.ArtType);
            cmd.SetParameterValue("@Title", request.Body.Title);
            cmd.SetParameterValue("@Summary", request.Body.Summary);
            cmd.SetParameterValue("@Cover", request.Body.Cover);
            cmd.SetParameterValue("@Contents", request.Body.Contents);
            cmd.SetParameterValue("@Link", request.Body.Link);
            cmd.SetParameterValue("@EditUser", request.Header.DisplayName);
            cmd.SetParameterValue("@ID", request.Body.ID);
            return cmd.ExecuteNonQuery();
        }

        public static int SetArticleLaw(int artid, string sqlText, string summary)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Article_SetLawyers");
            cmd.ReplaceParameterValue("@Lawyers", sqlText);
            cmd.SetParameterValue("@ArtID", artid);
            cmd.SetParameterValue("@Summary", summary);
            return cmd.ExecuteNonQuery();
        }

        public static int SetArticleStatus(RequestOperation<ArticleStatusData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Article_SetStatus");
            cmd.SetParameterValue("@ArtStatus", request.Body.ArtStatus);
            cmd.SetParameterValue("@EditUser", request.Header.DisplayName);
            cmd.SetParameterValue("@ID", request.Body.ID);
            return cmd.ExecuteNonQuery();
        }

        public static int SetArticleSortNo(RequestOperation<ArticleStatusData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Article_SetSortNo");
            cmd.SetParameterValue("@SortNo", request.Body.SortNo);
            cmd.SetParameterValue("@ID", request.Body.ID);
            return cmd.ExecuteNonQuery();
        }

        public static ArticleData GetDetail(int request)
        {

            var result = new ArticleData();
            DataCommand cmd = DataCommandManager.GetDataCommand("Article_GetDetail");
            cmd.SetParameterValue("@ID", request);
            var dt = cmd.ExecuteDataSet();
            if (dt != null && dt.Tables.Count > 0)
            {
                var rows = dt.Tables[0].Rows;
                if (rows.Count > 0)
                {
                    result = Valon.Framework.Data.EntityBuilder.BuildEntity<ArticleData>(rows[0]);
                }
                if (dt.Tables.Count > 1)
                {
                    var rows2 = dt.Tables[1];

                    var laws = rows2.AsEnumerable().Select(c => c.Field<int>("LawyerID").ToString()).ToList();
                    if (laws == null)
                    {
                        result.Lawyers = new List<string>();
                    }
                    else
                    {
                        result.Lawyers = laws;
                    }
                }
                else
                {
                    result.Lawyers = new List<string>();
                }
            }

            return result;

        }

    }
}
