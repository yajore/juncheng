using System.Data;
using Valon.Framework.Data;

namespace Lawyers.BizEntities
{
    public class MaterialData
    {
        [DataMapping("Sysno", DbType.Int32)]
        public int Sysno { get; set; }
        [DataMapping("Type", DbType.Int32)]
        public int Type { get; set; }
        [DataMapping("Url", DbType.String)]
        public string Url { get; set; }
    }
}
