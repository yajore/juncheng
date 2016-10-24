using System.Data;
using Valon.Framework.Data;

namespace Lawyers.BizEntities
{
    public class RoleData
    {
        [DataMapping("RoleID", DbType.Int32)]
        public int RoleID { get; set; }
        [DataMapping("RoleName", DbType.String)]
        public string RoleName { get; set; }
        [DataMapping("Remark", DbType.String)]
        public string Remark { get; set; }
        [DataMapping("Status", DbType.String)]
        public string Status { get; set; }
        
    }
}
