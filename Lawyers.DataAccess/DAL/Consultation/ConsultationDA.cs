using Lawyers.BizEntities;
using System.Collections.Generic;
using Valon.Framework.DataAccess;

namespace Mango.DataAccess
{
    public class ConsultationDA
    {
        public static QueryResultList<ConsultationShowData> GetConsultations(QueryRequest<ConsultationQueryData> query)
        {
            var result = new QueryResultList<ConsultationShowData>();
            DataCommand cmd = DataCommandManager.GetDataCommand("Consultation_GetList");
            cmd.SetParameterValue("@ToLawyer", query.Body.ToLawyer);
            cmd.SetParameterValue("@ConStatus", query.Body.ConStatus);
            cmd.SetParameterValue("@LawyerName", query.Body.LawyerName + "%");
            cmd.SetParameterValue("@Mobile", query.Body.Mobile);
            cmd.SetParameterValue("@StartDate", query.Body.StartDate);
            cmd.SetParameterValue("@EndDate", query.Body.EndDate);

            cmd.SetParameterValue("@PageCurrent", query.PageInfo.PageIndex);
            cmd.SetParameterValue("@PageSize", query.PageInfo.PageSize);
            cmd.SetParameterValue("@SortType", query.PageInfo.SortType);
            cmd.SetParameterValue("@SortField", query.PageInfo.SortField);
            result.Body = cmd.ExecuteEntityList<ConsultationShowData>();
            result.TotalCount = (int)cmd.GetParameterValue("@TotalCount");
            return result;
        }


        public static int AddConsultation(RequestOperation<ConsultationData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Consultation_Add");
            cmd.SetParameterValue("@Mobile", request.Body.Mobile);
            cmd.SetParameterValue("@ToLawyer", request.Body.ToLawyer);
            cmd.SetParameterValue("@Contents", request.Body.Contents);
            cmd.SetParameterValue("@ConStatus", request.Body.ConStatus);
            cmd.SetParameterValue("@UserID", request.Header.UserID);
            cmd.SetParameterValue("@CustomerFace", request.Body.CustomerFace);
            cmd.SetParameterValue("@InUser", request.Header.DisplayName);
            return cmd.ExecuteNonQuery();
        }


        public static int SetConsultationStatus(RequestOperation<ConsultationData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Consultation_SetStatus");
            cmd.SetParameterValue("@ConStatus", request.Body.ConStatus);
            cmd.SetParameterValue("@Sysno", request.Body.Sysno);
            cmd.SetParameterValue("@EditUser", request.Header.DisplayName);
            return cmd.ExecuteNonQuery();
        }

        public static QueryResultList<ConsultationShowData> GetUserCon(QueryRequest<int> query)
        {
            var result = new QueryResultList<ConsultationShowData>();
            DataCommand cmd = DataCommandManager.GetDataCommand("Consultation_GetUserCon");
            cmd.SetParameterValue("@UserID", query.Body);

            //cmd.SetParameterValue("@PageCurrent", query.PageInfo.PageIndex);
            //cmd.SetParameterValue("@PageSize", query.PageInfo.PageSize);
            //cmd.SetParameterValue("@SortType", query.PageInfo.SortType);
            //cmd.SetParameterValue("@SortField", query.PageInfo.SortField);
            result.Body = cmd.ExecuteEntityList<ConsultationShowData>();
            return result;
        }


        public static int SetReply(RequestOperation<ConsultationReplyData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Consultation_SetReply");
            cmd.SetParameterValue("@Reply", request.Body.ReplyContent.Length > 50 ? request.Body.ReplyContent.Substring(0, 50) : request.Body.ReplyContent);
            cmd.SetParameterValue("@ReplyContent", request.Body.ReplyContent);
            cmd.SetParameterValue("@ConsultationID", request.Body.ConsultationID);
            cmd.SetParameterValue("@ReplyUser", request.Header.UserID);
            cmd.SetParameterValue("@InUser", request.Header.DisplayName);
            return cmd.ExecuteNonQuery();
        }

        public static int SetReplyReadStat(RequestOperation<int> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Consultation_SetReplyReadStat");
            cmd.SetParameterValue("@Sysno", request.Body);
            cmd.SetParameterValue("@EditUser", request.Header.DisplayName);
            return cmd.ExecuteNonQuery();
        }

        public static ConsultationShowData GetReplyById(QueryRequest<int> query)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Consultation_GetReply");
            cmd.SetParameterValue("@Sysno", query.Body);

            //cmd.SetParameterValue("@PageCurrent", query.PageInfo.PageIndex);
            //cmd.SetParameterValue("@PageSize", query.PageInfo.PageSize);
            //cmd.SetParameterValue("@SortType", query.PageInfo.SortType);
            //cmd.SetParameterValue("@SortField", query.PageInfo.SortField);
            //result.Body = cmd.ExecuteEntity<ConsultationShowData>();
            return cmd.ExecuteEntity<ConsultationShowData>();
        }

        public static ConsultationUserData GetConUser(int cid)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Consultation_GetConUser");
            cmd.SetParameterValue("@Sysno", cid);
            return cmd.ExecuteEntity<ConsultationUserData>();
        }


    }
}
