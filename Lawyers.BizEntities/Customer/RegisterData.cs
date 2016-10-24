namespace Lawyers.BizEntities
{

    public class CustomerRegisterData
    {
        public CustomerData Customer { get; set; }
        public LoginData Login { get; set; }
    }

    public class RegisterData
    {
        /// <summary>
        /// 账号 手机号码注册用户
        /// </summary>
        public string Account { get; set; }

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
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Face { get; set; }
        /// <summary>
        /// 性别1男，2女，0保密
        /// </summary>
        public int Sex { get; set; }
    }
}
