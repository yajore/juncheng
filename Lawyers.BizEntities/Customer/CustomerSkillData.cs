using System.Data;
using Valon.Framework.Data;

namespace Lawyers.BizEntities
{
    public class CustomerSkillData
    {
        [DataMapping("Skill", DbType.String)]
        public string Skill { get; set; }

        [DataMapping("Sysno", DbType.Int32)]
        public int Sysno { get; set; }

        [DataMapping("Status", DbType.String)]
        public string Status { get; set; }
    }
}
