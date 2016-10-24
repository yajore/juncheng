using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using Valon.Framework.Data;

namespace Lawyers.BizEntities
{
    public class MenuData
    {
        [JsonIgnore]
        [DataMapping("MID", DbType.Int32)]
        public int MID { get; set; }
        [JsonIgnore]
        [DataMapping("RoleID", DbType.Int32)]
        public int RoleID { get; set; }
        [DataMapping("MName", DbType.String)]
        public string MName { get; set; }
        [DataMapping("MDesc", DbType.String)]
        public string MDesc { get; set; }
        [DataMapping("MIcon", DbType.String)]
        public string MIcon { get; set; }
        [DataMapping("MType", DbType.Int32)]
        public int MType { get; set; }
        [DataMapping("IsShow", DbType.Boolean)]
        public bool IsShow { get; set; }
        [DataMapping("Url", DbType.String)]
        public string Url { get; set; }
        [JsonIgnore]
        [DataMapping("PID", DbType.Int32)]
        public int PID { get; set; }
        [JsonIgnore]
        [DataMapping("Levels", DbType.Int32)]
        public int Levels { get; set; }
        public List<MenuData> Items { get; set; }
    }

    public class MenusData
    {
        public List<MenuData> PageRight { get; set; }
        public List<MenuExtentData> PageRightExtend { get; set; }
    }

    public class MenuTreeData
    {
        public List<MenuTreeItemData> PageRight { get; set; }
        public List<MenuTreeItemData> PageRightExtend { get; set; }
    }

    public class MenuTreeItemData
    {
        public int id { get; set; }
        public string parent { get; set; }
        public string icon { get; set; }
        public string text { get; set; }
        public Dictionary<string, string> li_attr { get; set; }
        public string url { get; set; }
        public MenuTreeStateData state { get; set; }
    }

    public class MenuTreeStateData
    {
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
    }
}
