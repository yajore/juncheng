using Lawyers.BizEntities;
using Valon.Framework.DataAccess;

namespace Mango.DataAccess
{
    public class MsgDA
    {
        public static int AddNewMsg(RequestOperation<MsgData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Msg_AddNewMsg");
            cmd.SetParameterValue("@Receiver", request.Body.Receiver);
            cmd.SetParameterValue("@MsgParam", request.Body.MsgParam);
            cmd.SetParameterValue("@MsgStatus", request.Body.MsgStatus);
            cmd.SetParameterValue("@MsgType", request.Body.MsgType);
            cmd.SetParameterValue("@ExpireTime", request.Body.ExpireTime);
            cmd.SetParameterValue("@InUser", request.Header.DisplayName);
            return cmd.ExecuteNonQuery();
        }

        public static int SetMsgStatus(RequestOperation<MsgData> request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Msg_SetMsgStatus");
            cmd.SetParameterValue("@MsgStatus", request.Body.MsgStatus);
            cmd.SetParameterValue("@MsgID", request.Body.MsgID);
            cmd.SetParameterValue("@EditUser", request.Header.DisplayName);
            return cmd.ExecuteNonQuery();
        }

        public static MsgData GetNewestMsg(string request)
        {
            DataCommand cmd = DataCommandManager.GetDataCommand("Msg_GetNewestMsg");
            cmd.SetParameterValue("@Receiver", request);
            return cmd.ExecuteEntity<MsgData>();
        }

    }
}
