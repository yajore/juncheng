using System.Data;
using Valon.Framework.Data;

namespace Lawyers.BizEntities
{
    public class UserRoleData
    {
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
        [DataMapping("Account", DbType.String)]
        public string Account { get; set; }
        [DataMapping("QQAccount", DbType.String)]
        public string QQAccount { get; set; }
        [DataMapping("WechatAccount", DbType.String)]
        public string WechatAccount { get; set; }
    }
}
