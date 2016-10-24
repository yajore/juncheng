using System.Collections.Generic;

namespace Lawyers.BizEntities
{
    public class SearchResultData
    {
        public List<ArticleShowData> Articles { get; set; }

        public List<CustomerLawyerShowData> Lawyers { get; set; }
    }
}
