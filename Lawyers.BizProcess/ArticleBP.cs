using Lawyers.BizEntities;
using Lawyers.Utilities;
using Mango.DataAccess;
using System;
using System.Collections.Generic;

namespace Lawyers.BizProcess
{
    public class ArticleBP
    {
        public QueryResultList<ArticleData> GetItems(QueryRequest<ArticleQueryData> query)
        {
            var result = new QueryResultList<ArticleData>();

            try
            {

                result = ArticleDA.GetItems(query);
                if (result.Body == null)
                {
                    result.Body = new List<ArticleData>();
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

        public QueryResultList<ArticleShowData> GetShowItems(QueryRequest<string> query)
        {
            var result = new QueryResultList<ArticleShowData>();

            try
            {

                result = ArticleDA.GetShowItems(query);
                if (result.Body == null)
                {
                    result.Body = new List<ArticleShowData>();
                }
                result.ErrCode = 0;
                result.Message = "ok";

            }
            catch (Exception ex)
            {
                Logger.WriteException("GetShowItems", ex, query);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult<SearchResultData> GetSearchResult(QueryRequest<string> query)
        {
            var result = new OperationResult<SearchResultData>();

            try
            {
                result.Body = new SearchResultData();


                result.Body.Articles = ArticleDA.GetArticleByKey(query);
                if (result.Body.Articles == null)
                {
                    result.Body.Articles = new List<ArticleShowData>();
                }

                result.Body.Lawyers = CustomerDA.GetCustomerByKey(query);

                if (result.Body.Lawyers == null)
                {
                    result.Body.Lawyers = new List<CustomerLawyerShowData>();

                }

                result.ErrCode = 0;
                result.Message = "ok";

            }
            catch (Exception ex)
            {
                Logger.WriteException("GetShowItems", ex, query);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public QueryResultList<ArticleShowData> GetDailyNews(QueryRequest<ArticleQueryData> query)
        {
            var result = new QueryResultList<ArticleShowData>();

            try
            {

                result = ArticleDA.GetDailyNews(query);
                if (result.Body == null)
                {
                    result.Body = new List<ArticleShowData>();
                }


                result.ErrCode = 0;
                result.Message = "ok";

            }
            catch (Exception ex)
            {
                Logger.WriteException("GetDailyNews", ex, query);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult<int> AddArticle(RequestOperation<ArticleData> request)
        {
            var result = new OperationResult<int>();

            try
            {

                if (String.IsNullOrEmpty(request.Body.Title) ||
                    String.IsNullOrEmpty(request.Body.Cover) ||
                      String.IsNullOrEmpty(request.Body.Contents))
                {
                    result.Message = "缺少请求参数";
                    return result;
                }


                var data = ArticleDA.AddArticle(request);

                if (data == null || data.Sysno == null)
                {
                    result.ErrCode = 1;
                    result.Message = "添加文章失败";
                }
                else
                {

                    int artid = (int)data.Sysno;



                    if (request.Body.Lawyers != null && request.Body.Lawyers.Count > 0)
                    {
                        //更新文章与律师关联
                        string sql = "INSERT INTO [dbo].[T_Article_Lawyers_Mapping]([ArtID],[LawyerID]) VALUES ";
                        string sqlCon = "({0},{1}),";
                        string fullStr = "";
                        foreach (var law in request.Body.Lawyers)
                        {
                            fullStr += String.Format(sqlCon, artid, law);
                        }

                        fullStr = sql + fullStr.TrimEnd(',');
                        //更新律师案例
                        fullStr += " UPDATE [dbo].[T_Customer_Lawyers] SET [CaseSeries] = @Summary,ArtID =@ArtID"
                            + "WHERE [UserID] IN (" +
                            String.Join(",", request.Body.Lawyers)
                            + ")";
                        ArticleDA.SetArticleLaw(artid, fullStr, request.Body.Summary);

                    }
                    result.Body = artid;
                    result.ErrCode = 0;
                    result.Message = "ok";

                }


            }
            catch (Exception ex)
            {
                Logger.WriteException("AddArticle", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult<int> UpdateArticle(RequestOperation<ArticleData> request)
        {
            var result = new OperationResult<int>();

            try
            {

                if (request.Body.ID <= 0 ||
                    String.IsNullOrEmpty(request.Body.Title) ||
                    String.IsNullOrEmpty(request.Body.Cover) ||
                      String.IsNullOrEmpty(request.Body.Contents))
                {
                    result.Message = "缺少请求参数";
                    return result;
                }


                var data = ArticleDA.UpdateArticle(request);

                if (data <= 0)
                {
                    result.ErrCode = 1;
                    result.Message = "添加文章失败";
                }
                else
                {

                    int artid = request.Body.ID;
                    if (request.Body.Lawyers != null && request.Body.Lawyers.Count > 0)
                    {
                        //更新文章与律师关联
                        string sql = "INSERT INTO [dbo].[T_Article_Lawyers_Mapping]([ArtID],[LawyerID]) VALUES ";
                        string sqlCon = "({0},{1}),";
                        string fullStr = "";
                        foreach (var law in request.Body.Lawyers)
                        {
                            fullStr += String.Format(sqlCon, artid, law);
                        }

                        fullStr = sql + fullStr.TrimEnd(',');
                        //更新律师案例
                        fullStr += " UPDATE [dbo].[T_Customer_Lawyers] SET [CaseSeries] = @Summary,ArtID =@ArtID"
                              + "WHERE [UserID] IN (" +
                              String.Join(",", request.Body.Lawyers)
                              + ")";
                        ArticleDA.SetArticleLaw(artid, fullStr, request.Body.Summary);

                    }
                    else
                    {
                        //逻辑可能有问题,此处
                        //新增文章时,关联了律师案例,修改时,是否继续删除,待定
                        ArticleDA.SetArticleLaw(artid, "", request.Body.Summary);
                    }
                    result.Body = artid;
                    result.ErrCode = 0;
                    result.Message = "ok";

                }


            }
            catch (Exception ex)
            {
                Logger.WriteException("UpdateArticle", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult SetArticleStatus(RequestOperation<ArticleStatusData> request)
        {
            var result = new OperationResult();

            try
            {

                var row = ArticleDA.SetArticleStatus(request);

                if (row == 0)
                {
                    result.ErrCode = 1;
                    result.Message = "设置失败";
                }
                else
                {
                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                //System.Reflection.MethodInfo.GetCurrentMethod().Name
                Logger.WriteException("SetArticleStatus", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult SetArticleSortNo(RequestOperation<ArticleStatusData> request)
        {
            var result = new OperationResult();

            try
            {

                var row = ArticleDA.SetArticleSortNo(request);

                if (row == 0)
                {
                    result.ErrCode = 1;
                    result.Message = "设置失败";
                }
                else
                {
                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                //System.Reflection.MethodInfo.GetCurrentMethod().Name
                Logger.WriteException("SetArticleSortNo", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

        public OperationResult<ArticleData> GetDetail(RequestOperation<int> request)
        {
            var result = new OperationResult<ArticleData>();

            try
            {

                var row = ArticleDA.GetDetail(request.Body);

                if (row == null)
                {
                    result.ErrCode = 1;
                    result.Message = "不存在此文章";
                }
                else
                {
                    result.Body = row;
                    result.ErrCode = 0;
                    result.Message = "ok";
                }


            }
            catch (Exception ex)
            {
                //System.Reflection.MethodInfo.GetCurrentMethod().Name
                Logger.WriteException("GetDetail", ex, request);
                result.ErrCode = -1;
                result.Message = ex.Message;
            }

            return result;
        }

    }
}
