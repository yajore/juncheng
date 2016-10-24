using System;
using System.Collections.Generic;
using System.Data;
using Valon.Framework.Data;

namespace Lawyers.BizEntities
{
    public class ArticleData
    {
        [DataMapping("ID", DbType.Int32)]
        public int ID { get; set; }

        [DataMapping("GroupType", DbType.Int32)]
        public int GroupType { get; set; }

        [DataMapping("ArtType", DbType.Int32)]
        public int ArtType { get; set; }

        [DataMapping("Title", DbType.String)]
        public string Title { get; set; }

        [DataMapping("Summary", DbType.String)]
        public string Summary { get; set; }

        [DataMapping("Cover", DbType.String)]
        public string Cover { get; set; }

        [DataMapping("Contents", DbType.String)]
        public string Contents { get; set; }

        [DataMapping("PublisherID", DbType.Int32)]
        public int PublisherID { get; set; }

        [DataMapping("PublisherDate", DbType.DateTime)]
        public string PublisherDate { get; set; }

        [DataMapping("ArtStatus", DbType.Int32)]
        public int ArtStatus { get; set; }

        [DataMapping("Link", DbType.String)]
        public string Link { get; set; }

        [DataMapping("SortNo", DbType.Int32)]
        public int SortNo { get; set; }

        [DataMapping("InUser", DbType.String)]
        public string InUser { get; set; }

        [DataMapping("InDate", DbType.DateTime)]
        public string InDate { get; set; }

        public List<string> Lawyers { get; set; }
    }

    public class ArticleShowData
    {
        [DataMapping("ID", DbType.Int32)]
        public int ID { get; set; }

        [DataMapping("ArtType", DbType.Int32)]
        public int ArtType { get; set; }

        [DataMapping("Title", DbType.String)]
        public string Title { get; set; }

        [DataMapping("Cover", DbType.String)]
        public string Cover { get; set; }

        [DataMapping("PublisherDate", DbType.DateTime)]
        public DateTime PublisherDate { get; set; }

        [DataMapping("Summary", DbType.String)]
        public string Summary { get; set; }

    }

}
