using System;
using System.Data;
using Valon.Framework.Data;

namespace Lawyers.BizEntities
{
    public class ConsultationReplyData
    {
        [DataMapping("Sysno", DbType.Int32)]
        public int Sysno { get; set; }

        [DataMapping("ConsultationID", DbType.Int32)]
        public int ConsultationID { get; set; }

        [DataMapping("ReplyContent", DbType.String)]
        public string ReplyContent { get; set; }

        [DataMapping("IsRead", DbType.Int16)]
        public int IsRead { get; set; }

        [DataMapping("ReplyUser", DbType.Int32)]
        public int ReplyUser { get; set; }

        [DataMapping("InDate", DbType.DateTime)]
        public DateTime InDate { get; set; }

        [DataMapping("InUser", DbType.String)]
        public string InUser { get; set; }

        public bool IsNotify { get; set; }

    }

}
