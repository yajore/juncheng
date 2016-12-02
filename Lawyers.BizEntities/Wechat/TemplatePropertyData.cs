using System.Collections.Generic;

namespace Lawyers.BizEntities
{
    public class TemplateData
    {
        public string topcolor { get; set; }
        public string openid { get; set; }
        public string templateid { get; set; }
        public string url { get; set; }
        public List<TemplatePropertyData> propertys { get; set; }
    }
    public class TemplatePropertyData
    {
        public string color { get; set; }
        public string key { get; set; }
        public string value { get; set; }
    }
}
