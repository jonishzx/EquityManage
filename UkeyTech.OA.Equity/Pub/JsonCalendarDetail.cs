using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace UkeyTech.OA.Equity.Pub
{
    #region json化日期明细
    public class JsonCalendarDetail
    {

        /// <summary>
        /// 工作日标记
        /// </summary>
	    public static readonly string WorkingDateTag = "working";
        /// <summary>
        /// 非工作日标记
        /// </summary>
        public static readonly string NonWorkingDateTag = "nonworking";

        public string id { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string title { get; set; }
        public string rendering { get; set; }
        public bool allDay { get; set; }
        public string className { get; set; }

        public static List<JsonCalendarDetail> RendingWorkingDate(DataTable dt)
        {
            /*
            id: 'availableForMeeting',
            start: '2015-02-13T10:00:00',
            end: '2015-02-13T16:00:00',
            rendering: 'background'
            */

            if (dt==null || dt.Rows.Count == 0)
                return null;

            var rst = new List<JsonCalendarDetail>();
            foreach (DataRow detail in dt.Rows)
            {
                rst.Add(new JsonCalendarDetail()
                {
                    id = Guid.NewGuid().ToString(),
                    start = Convert.ToDateTime(detail["SgDate"]),
                    end = Convert.ToDateTime(detail["SgDate"]),
                    rendering = "background",
                    allDay = true
                });
            }
            return rst;
        }
    }

   
    #endregion
}
