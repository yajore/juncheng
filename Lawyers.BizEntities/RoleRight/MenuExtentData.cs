using System.Data;
using Valon.Framework.Data;

namespace Lawyers.BizEntities
{
    public class MenuExtentData
    {
        [DataMapping("ExtendID", DbType.Int32)]
        public int ExtendID { get; set; }
        [DataMapping("RoleID", DbType.Int32)]
        public int RoleID { get; set; }
        [DataMapping("MID", DbType.Int32)]
        public int MID { get; set; }
        [DataMapping("RightKey", DbType.String)]
        public string RightKey { get; set; }
        [DataMapping("Status", DbType.String)]
        public string Status { get; set; }
        [DataMapping("Url", DbType.String)]
        public string Url { get; set; }
        [DataMapping("ExtendName", DbType.String)]
        public string ExtendName { get; set; }
        
    }
}
