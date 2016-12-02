using System;
using System.Data;
using Valon.Framework.Data;

namespace Lawyers.BizEntities
{
    public class ConsultationData
    {
        [DataMapping("Sysno", DbType.Int32)]
        public int Sysno { get; set; }

        [DataMapping("Mobile", DbType.String)]
        public string Mobile { get; set; }

        [DataMapping("ToLawyer", DbType.Int32)]
        public int ToLawyer { get; set; }

        [DataMapping("Contents", DbType.String)]
        public string Contents { get; set; }

        [DataMapping("ConStatus", DbType.Int32)]
        public int ConStatus { get; set; }

        [DataMapping("UserID", DbType.Int32)]
        public int UserID { get; set; }

        [DataMapping("CustomerFace", DbType.String)]
        public string CustomerFace { get; set; }

        public string Code { get; set; }
    }

    public class ConsultationShowData: ConsultationData
    {
        [DataMapping("InDate", DbType.DateTime)]
        public DateTime InDate { get; set; }

        [DataMapping("LawyerName", DbType.String)]
        public string LawyerName { get; set; }

        [DataMapping("LawyerFace", DbType.String)]
        public string LawyerFace { get; set; }

        [DataMapping("CustomerName", DbType.String)]
        public string CustomerName { get; set; }
        
        [DataMapping("Reply", DbType.String)]
        public string Reply { get; set; }

        [DataMapping("ReplyContent", DbType.String)]
        public string ReplyContent { get; set; }

        [DataMapping("LawyerMobile", DbType.String)]
        public string LawyerMobile { get; set; }

    }

}
