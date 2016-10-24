using System;
using System.Data;
using Valon.Framework.Data;

namespace Lawyers.BizEntities
{
    public class CustomerData
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMapping("UserID", DbType.Int32)]
        public int UserID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [DataMapping("Name", DbType.String)]
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [DataMapping("NickName", DbType.String)]
        public string NickName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        [DataMapping("Face", DbType.String)]
        public string Face { get; set; }
        /// <summary>
        /// 性别1男，2女，0保密
        /// </summary>
        [DataMapping("Sex", DbType.Int32)]
        public int Sex { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        [DataMapping("BirthDay", DbType.Date)]
        public DateTime BirthDay { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        [DataMapping("Mobile", DbType.String)]
        public string Mobile { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        [DataMapping("WexinNo", DbType.String)]
        public string WexinNo { get; set; }
        [DataMapping("WexinQrcode", DbType.String)]
        public string WexinQrcode { get; set; }
        /// <summary>
        /// qq号
        /// </summary>
        [DataMapping("QQNo", DbType.String)]
        public string QQNo { get; set; }
        [DataMapping("Email", DbType.String)]
        public string Email { get; set; }
        
        /// <summary>
        /// 家乡code
        /// </summary>
        [DataMapping("HomeTownCode", DbType.String)]
        public string HomeTownCode { get; set; }
        /// <summary>
        /// 家乡
        /// </summary>
        [DataMapping("HomeTown", DbType.String)]
        public string HomeTown { get; set; }
        /// <summary>
        /// 现居住地code
        /// </summary>
        [DataMapping("PCDCode", DbType.String)]
        public string PCDCode { get; set; }
        /// <summary>
        /// 现居住地址
        /// </summary>
        [DataMapping("PCDDesc", DbType.String)]
        public string PCDDesc { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [DataMapping("Address", DbType.String)]
        public string Address { get; set; }
        /// <summary>
        /// 工作
        /// </summary>
        [DataMapping("Job", DbType.String)]
        public string Job { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        [DataMapping("Company", DbType.String)]
        public string Company { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        [DataMapping("Signature", DbType.String)]
        public string Signature { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMapping("CustomerType", DbType.Int32)]
        public int CustomerType { get; set; }
        /// <summary>
        /// 用户审核状态 10 通过  11不通过 1:待审核
        /// </summary>
        [DataMapping("AuditStatus", DbType.Int32)]
        public int AuditStatus { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        [DataMapping("InDate", DbType.DateTime)]
        public DateTime InDate { get; set; }

    }

    public class CustomerLawyerData : CustomerData
    {
        [DataMapping("Skills", DbType.String)]
        public string Skills { get; set; }

        [DataMapping("CaseSeries", DbType.String)]
        public string CaseSeries { get; set; }

        [DataMapping("Subscribe", DbType.String)]
        public string Subscribe { get; set; }

        [DataMapping("Resume", DbType.String)]
        public string Resume { get; set; }

        [DataMapping("SortNo", DbType.Int32)]
        public int SortNo { get; set; }
    }

    public class CustomerLawyerShowData 
    {
        [DataMapping("ArtID", DbType.Int32)]
        public int ArtID { get; set; }

        [DataMapping("CaseSeries", DbType.String)]
        public string CaseSeries { get; set; }

        [DataMapping("Skills", DbType.String)]
        public string Skills { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMapping("UserID", DbType.Int32)]
        public int UserID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [DataMapping("Name", DbType.String)]
        public string Name { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        [DataMapping("Face", DbType.String)]
        public string Face { get; set; }
    }

    public class CustomerIDEntry
    {
        [DataMapping("UserID", DbType.Int32)]
        public int UserID { get; set; }
    }
}
