namespace Lawyers.BizEntities
{
    public class UserLoginData
    {
        /// <summary>
        /// 一般为手机号码
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        public bool IsRemeber { get; set; }

        public string ReferUrl { get; set; }
    }

    public class UserLoginWithMobileData
    {
        /// <summary>
        /// 一般为手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Code { get; set; }

    }
}
