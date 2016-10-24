using Lawyers.BizEntities;
using Lawyers.BizProcess;
using Lawyers.Utilities;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Lawyers.Web.Controllers
{
    /// <summary>
    /// 文章
    /// </summary>
    public class ArticleController : ApiController
    {

        [HttpPost]
        public QueryResultList<ArticleData> GetItems([FromBody]string queryString)
        {
            var result = new QueryResultList<ArticleData>();


            var request = JsonHelper.Build<QueryRequest<ArticleQueryData>>(queryString);

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
            result = new ArticleBP().GetItems(request);
            return result;
        }

        [HttpPost]
        public QueryResultList<ArticleShowData> GetShowItems([FromBody]string queryString)
        {
            var result = new QueryResultList<ArticleShowData>();


            var request = JsonHelper.Build<QueryRequest<string>>(queryString);

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
            result = new ArticleBP().GetShowItems(request);
            return result;
        }

        [HttpPost]
        public QueryResultList<ArticleShowData> GetDailyNews([FromBody]string queryString)
        {
            var result = new QueryResultList<ArticleShowData>();


            var request = JsonHelper.Build<QueryRequest<ArticleQueryData>>(queryString);

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
            result = new ArticleBP().GetDailyNews(request);
            return result;
        }

        [HttpPost]
        public OperationResult<SearchResultData> Search([FromBody]string queryString)
        {
            var result = new OperationResult<SearchResultData>();


            var request = JsonHelper.Build<QueryRequest<string>>(queryString);

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
            result = new ArticleBP().GetSearchResult(request);
            return result;
        }

        [HttpPost]
        public OperationResult<int> AddArticle([FromBody]string queryString)
        {
            var result = new OperationResult<int>();

            var request = JsonHelper.Build<RequestOperation<ArticleData>>(queryString);

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
            result = new ArticleBP().AddArticle(request);
            return result;
        }

        [HttpPost]
        public OperationResult<int> UpdateArticle([FromBody]string queryString)
        {
            var result = new OperationResult<int>();

            var request = JsonHelper.Build<RequestOperation<ArticleData>>(queryString);

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
            result = new ArticleBP().UpdateArticle(request);
            return result;
        }

        [HttpPost]
        public OperationResult SetArticleStatus([FromBody]string queryString)
        {
            var result = new OperationResult();

            var request = JsonHelper.Build<RequestOperation<ArticleStatusData>>(queryString);

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
            result = new ArticleBP().SetArticleStatus(request);
            return result;
        }

        [HttpPost]
        public OperationResult SetArticleSortNo([FromBody]string queryString)
        {
            var result = new OperationResult();

            var request = JsonHelper.Build<RequestOperation<ArticleStatusData>>(queryString);

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
            result = new ArticleBP().SetArticleSortNo(request);
            return result;
        }

        [HttpPost]
        public OperationResult<ArticleData> GetDetail([FromBody]string queryString)
        {
            var result = new OperationResult<ArticleData>();

            var request = JsonHelper.Build<RequestOperation<int>>(queryString);

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
            result = new ArticleBP().GetDetail(request);
            return result;
        }

    }
}
