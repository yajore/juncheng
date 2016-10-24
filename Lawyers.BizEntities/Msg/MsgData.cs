using System;
using System.Data;
using Valon.Framework.Data;

namespace Lawyers.BizEntities
{
    public class MsgData
    {
        [DataMapping("MsgID", DbType.Int32)]
        public int MsgID { get; set; }

        [DataMapping("Receiver", DbType.String)]
        public string Receiver { get; set; }

        [DataMapping("MsgParam", DbType.String)]
        public string MsgParam { get; set; }

        [DataMapping("MsgType", DbType.Int32)]
        public int MsgType { get; set; }

        [DataMapping("MsgStatus", DbType.Int32)]
        public int MsgStatus { get; set; }

        [DataMapping("ExpireTime", DbType.DateTime)]
        public DateTime ExpireTime { get; set; }
        
        public string InUser { get; set; }
    }

    public class MsgParamData
    {
        public string code { get; set; }
    }
}
