using System.Collections.Generic;


namespace Lawyers.BizEntities
{


    /// <summary>
    /// 用户信息
    /// </summary>
    public class HeaderInfo
    {
        /// <summary>
        /// 展示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 设备 1:安卓   2:ISO  3:渠道   5:H5   必填
        /// </summary>
        public int DeviceID { get; set; }

        public static HeaderInfo GetUser()
        {
            return new HeaderInfo();
        }
    }
    /// <summary>
    /// 分页内容
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// 页码 0开始
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 条目
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 排序类型 DESC：倒叙  ASC:正序
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 排序字段，根据指定的字段传值
        /// </summary>
        public string SortType { get; set; }
    }

    /// <summary>
    /// 查询列表（带分页请求）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryRequest<T>
    {
        public T Body { get; set; }
        public PageInfo PageInfo { get; set; }
        public HeaderInfo Header { get; set; }
    }

    /// <summary>
    /// 请求基类
    /// </summary>
    public class RequestOperation
    {
        public HeaderInfo Header { get; set; }
    }

    /// <summary>
    /// 操作类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RequestOperation<T>
    {
        public T Body { get; set; }
        public HeaderInfo Header { get; set; }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Body { get; set; }
    }

    public class QueryResultList<T> : OperationResult
    {
        public List<T> Body { get; set; }
        public int TotalCount { get; set; }
    }

    public class OperationResult
    {
        public OperationResult()
        {
            ErrCode = 1;
        }
        /// <summary>
        /// 0-正确 1-系统业务逻辑错误  2-权限错误 3-参数错误 -1-系统异常错误
        /// </summary>
        public int ErrCode { get; set; }
        /// <summary>
        /// 附带消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 保留字段 
        /// </summary>
        public object Fields { get; set; }
    }

   
}
