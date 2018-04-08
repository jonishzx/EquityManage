using Clover.Data;

namespace UkeyTech.OA.Equity.DAO
{
    using System.Collections.Generic;
    using System.Data;
    using Dapper;

    /// <summary>
    /// 
    /// </summary>
    public class CommonHelper : Clover.Data.BaseDAO
    {
        /// <summary>
        /// 根据原id插入新的附件记录
        /// </summary>
        /// <param name="oldTargetId"></param>
        /// <param name="newTargetId"></param>
        /// <param name="oldTargetType"></param>
        /// <param name="newTargetType"></param>
        /// <returns></returns>
        public static int InsertAttachment(string oldTargetId, string newTargetId, string oldTargetType,
            string newTargetType)
        {
            int rst=0;
            var conn = DbService();
            var sql = string.Format(@"insert into sys_Attachment
select [Title],'{1}','{3}',[Tag],[FilePath],[FileName],[PreviewFilePath],[Bytes],[Descn],[ViewOrder],[NeedConvert],Status
      ,[DownloadCount],[Creator],[UpdateTime],'A' from dbo.sys_Attachment 
      where TargetType='{2}' and TargetID='{0}'", oldTargetId, newTargetId, oldTargetType, newTargetType);
            rst = conn.Execute(sql);
            return rst;
        }

        /// <summary>
        /// 根据原id插入新的附件记录
        /// </summary>
        /// <param name="oldTargetId"></param>
        /// <param name="newTargetId"></param>
        /// <param name="oldTargetType"></param>
        /// <param name="newTargetType"></param>
        /// <returns></returns>
        public static int InsertWithDelAttachment(string oldTargetId, string newTargetId, string oldTargetType,
            string newTargetType)
        {
            int rst = 0;
            var conn = DbService();
            var sql = string.Format("delete from sys_Attachment where TargetType='{0}' and TargetID<>'{1}'"
             , newTargetType, newTargetId);
            conn.Execute(sql);

            sql = string.Format(@"insert into sys_Attachment
select [Title],'{1}','{3}',[Tag],[FilePath],[FileName],[PreviewFilePath],[Bytes],[Descn],[ViewOrder],[NeedConvert],Status
      ,[DownloadCount],[Creator],[UpdateTime],'A' from dbo.sys_Attachment 
      where TargetType='{2}' and TargetID='{0}'", oldTargetId, newTargetId, oldTargetType, newTargetType);
            rst = conn.Execute(sql);
           
            return rst;
        }
    }
}
