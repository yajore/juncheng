using System.Collections.Generic;
using System.Data;
using Valon.Framework.Data;

namespace Lawyers.BizEntities
{
    public class RoleRightListData
    {

        [DataMapping("MID", DbType.Int32)]
        public int MID { get; set; }
    }


    public class SetRoleRightData
    {


        public int RoleID { get; set; }

        public List<int> RightIDs { get; set; }
    }

}
