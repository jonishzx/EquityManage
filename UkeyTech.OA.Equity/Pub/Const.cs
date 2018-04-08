using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UkeyTech.OA.Equity
{
    internal class Const
    {
        /// <summary>
        /// 用于连接待办信息表的语句(左连接)
        /// </summary>
        public static readonly string WORKFLOW_QUERY_RUNNING_QUERY_LEFTJOIN =
            @"LEFT JOIN dbo.T_FF_RT_WORKITEM wi on wi.STATE IN (0,1)               
               LEFT JOIN T_FF_RT_PROCESSINSTANCE tfp ON wi.PROCESSINSTANCE_ID = tfp.ID AND tfp.BIZ_ID = {0}";

//        @"LEFT JOIN dbo.T_FF_RT_WORKITEM wi on wi.STATE IN (0,1)
//               LEFT JOIN T_FF_RT_TASKINSTANCE tfft ON wi.TASKINSTANCE_ID = tfft.ID
//               LEFT JOIN T_FF_RT_PROCESSINSTANCE tfp ON tfft.PROCESSINSTANCE_ID = tfp.ID AND tfp.BIZ_ID = {0}";
        
        /// <summary>
        /// 用于连接待办信息表的语句
        /// </summary>
        public static readonly string WORKFLOW_QUERY_RUNNING_QUERY =
            @"JOIN dbo.T_FF_RT_WORKITEM wi on wi.STATE IN (0,1)                
                JOIN T_FF_RT_PROCESSINSTANCE tfp ON wi.PROCESSINSTANCE_ID = tfp.ID AND tfp.BIZ_ID = {0}";

//            @"JOIN dbo.T_FF_RT_WORKITEM wi on wi.STATE IN (0,1)
//                JOIN T_FF_RT_TASKINSTANCE tfft ON wi.TASKINSTANCE_ID = tfft.ID
//                JOIN T_FF_RT_PROCESSINSTANCE tfp ON tfft.PROCESSINSTANCE_ID = tfp.ID AND tfp.BIZ_ID = {0}";

        /// <summary>
        /// 与工作流表关联的条件
        /// </summary>
        public static readonly string WORKFLOW_QUERY_WHEREACTOR =
           @" ((t.ACTOR_ID = '{0}' or t.Complete_ACTOR_ID='{0}') or EXISTS (SELECT 1 FROM CF_AssignUserList caul 
           WHERE caul.UserId = t.ACTOR_ID AND caul.FormID = t.BIZ_TYPE_ID
		   AND caul.AssignToUserId = '{0}'))";

        /// <summary>
        /// 与工作流表关联的条件(委托时间内产生的待办才可以查看,以往待办不允许查看,被委托的任务如果其中一个处理人是当前用户时也不需要显示[只用于任意一人策略])
        /// </summary>
        public static readonly string WORKFLOW_QUERY_RUNNING_WHEREACTOR =
           @" ((t.ACTOR_ID = '{0}' or t.Complete_ACTOR_ID='{0}') or (t.ACTOR_ID <> '{0}' AND EXISTS (SELECT 1 FROM CF_AssignUserList caul 
           WHERE   caul.AssignToUserId = '{0}' AND caul.UserId = t.ACTOR_ID AND caul.FormID = t.BIZ_TYPE_ID         
           AND convert(varchar(16),getdate(),121) between convert(varchar(16),caul.AssginBeginDate,121)  and convert(varchar(16),caul.AssginEndDate,121)
           --AND convert(varchar(16),t.CREATED_TIME,121) between convert(varchar(16),caul.AssginBeginDate,121)  and convert(varchar(16),caul.AssginEndDate,121)
		   
           AND Not Exists (select 1 from T_FF_RT_WORKITEM twi 
                            join T_FF_RT_TASKINSTANCE tssi on twi.TASKINSTANCE_ID = tssi.ID
                            where twi.TASKINSTANCE_ID = t.TASKINSTANCE_ID AND twi.ACTOR_ID = '{0}' AND tssi.ASSIGNMENT_STRATEGY = 1)
)))";

        /// <summary>
        /// 用于连接已办信息表的语句
        /// </summary>
        public static readonly string WORKFLOW_QUERY_FINISH =
           @"JOIN dbo.T_FF_RT_WORKITEM wi on wi.STATE IN (5,7,9) AND wi.DISPLAY_NAME <> '提交申请'                
                JOIN T_FF_RT_PROCESSINSTANCE tfp ON wi.PROCESSINSTANCE_ID = tfp.ID AND tfp.BIZ_ID = {0}";

//         @"JOIN dbo.T_FF_RT_WORKITEM wi on wi.STATE IN (5,7,9)
//                JOIN T_FF_RT_TASKINSTANCE tfft ON wi.TASKINSTANCE_ID = tfft.ID AND tfft.DISPLAY_NAME <> '提交申请'
//                JOIN T_FF_RT_PROCESSINSTANCE tfp ON tfft.PROCESSINSTANCE_ID = tfp.ID AND tfp.BIZ_ID = {0}";
    }
}
