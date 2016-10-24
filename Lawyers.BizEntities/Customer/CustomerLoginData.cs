using System.Data;
using Valon.Framework.Data;

namespace Lawyers.BizEntities
{
    /// <summary>
    /// 登录表
    /// </summary>
    public class LoginData
    {
        /// <summary>
        /// 6位id，100001开始
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 邮件
        /// </summary>
        public string Email { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// qq登录
        /// </summary>
        public string QQAccount { get; set; }
        /// <summary>
        /// 微信登录
        /// </summary>
        public string WechatAccount { get; set; }
        /// <summary>
        /// 微薄
        /// </summary>
        public string WeiboAccount { get; set; }
        /// <summary>
        /// 百度
        /// </summary>
        public string BaiduAccount { get; set; }
        /// <summary>
        /// 用户角色(后台登录使用) 1：普通2：领队10：平台
        /// </summary>
        public int UserRoleID { get; set; }
        /// <summary>
        ///  1:安卓 2:ISO, 3,渠道 5,h5 来源
        /// </summary>
        public int UserFrom { get; set; }
        /// <summary>
        /// 1启用 0未启用
        /// </summary>
        public int UserStatus { get; set; }
    }

    /// <summary>
    /// 用户登录返回
    /// </summary>
    public class CustomerLoginData
    {
        /// <summary>
        /// 6位id，100001开始
        /// </summary>
        [DataMapping("UserID", DbType.Int32)]
        public int UserID { get; set; }
        /// <summary>
        /// 用户角色(后台登录使用) 1：普通2：领队10：平台
        /// </summary>
        [DataMapping("UserRoleID", DbType.Int32)]
        public int UserRoleID { get; set; }
        /// <summary>
        /// 用户审核状态 10 通过  11不通过 0:待审核
        /// </summary>
        [DataMapping("UserStatus", DbType.Int32)]
        public int UserStatus { get; set; }
        /// <summary>
        /// 用户审核状态 10 通过  11不通过 0:待审核
        /// </summary>
        [DataMapping("AuditStatus", DbType.Int32)]
        public int AuditStatus { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [DataMapping("Name", DbType.String)]
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [DataMapping("NickName", DbType.String)]
        public string NickName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        [DataMapping("Face", DbType.String)]
        public string Face { get; set; }
        /// <summary>
        /// 1:注册用户，2：领队
        /// </summary>
        [DataMapping("CustomerType", DbType.Int32)]
        public int CustomerType { get; set; }

        [DataMapping("Mobile", DbType.String)]
        public string Mobile { get; set; }

    }
}
