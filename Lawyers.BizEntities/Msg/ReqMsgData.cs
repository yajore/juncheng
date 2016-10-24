namespace Lawyers.BizEntities
{
    public class ReqMsgData
    {
        //手机号码
        public string Mobile { get; set; }
        //短信类型
        /// <summary>
        /// {1,"注册验证码:{0}，请于10分钟内输入，过期请重新获取" }, 注册
        /// {2,"登录验证码:{0}，请于10分钟内输入，过期请重新获取"},  登录
        /// {3,"验证码：{0}，请于10分钟内输入，过期请重新获取"}, 注册+登录
        /// {4,"您有一笔新的退款申请已受理，请登录xx查看进度"},  退款提醒
        /// {5,"您的提现申请已受理，请登录xx查看进度"},   提现提醒
        /// {6,"您已成功参与【西藏7日行】活动，请登录APP查看活动时间及出发地点"}, 参与活动提醒
        /// {99,"验证码:{0}"}  其他
        /// </summary>
        public int MsgType { get; set; }
        /// <summary>
        /// 短信码
        /// </summary>
        public string Code { get; set; }
    }
}
