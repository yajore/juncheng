using System.Data;
using Valon.Framework.Data;

namespace Lawyers.BizEntities
{
    public class ConsultationUserData : ConsultationShowData
    {

        [DataMapping("WechatAccount", DbType.String)]
        public string WechatAccount { get; set; }

    }
}
