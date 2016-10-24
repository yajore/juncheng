using System.Data;
using Valon.Framework.Data;

namespace Lawyers.BizEntities
{
    public class EntryID
    {
        [DataMapping("Sysno", DbType.Object)]
        public object Sysno { get; set; }
    }
}
