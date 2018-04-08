using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Clover.Data;
using Dapper;

namespace UkeyTech.WebFW.DAO
{
    /// <summary>
    ///     User 数据访问层
    /// </summary>
    public partial class BillNoDAO : BaseDAO
    {
        #region 方法

        /// <summary>
        ///     读取信息
        /// </summary>
        public string GetBillNo(string document)
        {
            return GetBillNo("{document}-{datetime}-{Number}", "yyMM", document);
        }
            
        /// <summary>
        /// 修正重新重复编号的业务表
        /// </summary>
        /// <param name="document"></param>
        /// <param name="format"></param>
        /// <param name="datetimeformat"></param>
        /// <param name="targettable"></param>
        /// <param name="targetcolumn"></param>
        /// <param name="keyField"></param>
        /// <param name="orderField"></param>
        public List<string> FixBillNo(string billno, string document, string format, string datetimeformat, string targettable, string targetcolumn, string keyField, string orderField)
        {
           //1.获取重复的单号
           var billnodt = GetDataTable(string.Format(@"
            select * from {1} where {0} in (
            select {0} from {1} group by {0} having(count(*) > 1))
            AND {0} = '{3}'
            order by {2}", targetcolumn, targettable, orderField, billno));
       
            var changelist = new List<string>();
            //2.根据单号获取业务数据并排序
            var conn = DbService();
            foreach(DataRow dr in billnodt.Rows)
            {
                //3.更新业务编码
                string newBillNo = ShowNewBillNo(document, DateTime.Parse(dr[orderField].ToString()), format, datetimeformat, targettable, targetcolumn);
                string updSQL = string.Format("update {1} set {0} = '{2}' where {3} = '{4}'"
                    , targetcolumn, targettable, newBillNo, keyField, dr[keyField]);
                conn.Execute(updSQL);
                changelist.Add(string.Format("编码从{0}更改为{1}", newBillNo, dr[targetcolumn].ToString()));
            }

            return changelist;
        }

        //获取异常的单据信息
         public DataTable GetExceptBillNoList(string document)
        {
             DataTable dt = Select(document);
             if (dt.Rows.Count > 0)
             {
                 DataRow dr = dt.Rows[0];
                    //1.获取重复的单号
                 var billnodt = GetDataTable(string.Format(@"select {0} from {1} where {0} like '{2}%' group by {0} having(count(*) > 1) "
                     , dr["TargetColumn"].ToString(), dr["TargetTable"].ToString(), document));
                 return billnodt;
             }
            return null;
             
        }

        /// <summary>
        /// 获取一个标识的最新业务代码
        /// </summary>
        public string ShowNewBillNo(string document,string format, string datetimeformat,
            string targettable,
            string targetcolumn)
        {
            return ShowNewBillNo(document, DateTime.Now, format, datetimeformat, targettable, targetcolumn);
        }

        /// <summary>
        /// 通过特定时间获取一个标识的最新业务代码
        /// </summary>
        /// <param name="document"></param>
        /// <param name="format"></param>
        /// <param name="datetimeformat"></param>
        /// <param name="targettable"></param>
        /// <param name="targetcolumn"></param>
        /// <returns></returns>
        public string ShowNewBillNo(string document, DateTime datevalue, string format, string datetimeformat, string targettable,
            string targetcolumn)
        {
            bool hasTargetTable = !string.IsNullOrEmpty(targettable) && !string.IsNullOrEmpty(targetcolumn);
            bool hasRec = false;
            string currDate;
            int currNumber = 1;

            DataTable dt = Select(document);
            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                //已经具有编号，获取编号
                if (dr["TargetTable"] != DBNull.Value && dr["TargetColumn"] != DBNull.Value)
                {
                    targettable = dr["TargetTable"].ToString();
                    targetcolumn = dr["TargetColumn"].ToString();
                    hasTargetTable = true;
                }
                currDate = dr["date"].ToString();
                if (currDate.CompareTo(DateTime.Now.ToString(datetimeformat)) == 0)
                {
                    //日期发生变更,重置编号
                    currNumber = int.Parse(dr["number"].ToString()) + 1;
                }
              
                hasRec = true;
            }
         
            //生成新编号后，并进行更新BillNo表数据
            if (!hasRec && hasTargetTable)
            {
                //已关联数据表
                currNumber = GetMaxNumberByTargetTable(document, datetimeformat, targettable, targetcolumn);
            }
            if (hasTargetTable)
            {
                //防止重复编号
                while (CheckNoInTable(targettable, targetcolumn, GetFormatValue(format, document, datevalue, datetimeformat, currNumber)))
                {
                    currNumber++;
                }
            }
            if (!hasRec)
            {
                insert(document, currNumber.ToString(), datevalue.ToString(datetimeformat));
            }
            else
            {
                Update(document, currNumber.ToString(), datevalue.ToString(datetimeformat));
            }
          
            return GetFormatValue(format, document, datevalue, datetimeformat, currNumber);
        }

        /// <summary>
        /// 编号格式长度，如3代表1的值时生成字符串“001”
        /// </summary>
        private const int NumLen = 3;
        private int GetMaxNumberByTargetTable(string document, string dateformat, string targettable, string targetcolumn)
        {
            string maxNo = string.Empty;
            int iNumber = 0;
            string selmaxidSQL = string.Format("select max({0}) from {1} WITH (TABLOCKX) Where {0} like '%{2}%'",
                               targetcolumn, targettable, document + DateTime.Now.ToString(dateformat));

            IDbConnection conn = DbService();

            IEnumerable<string> list = conn.Query<string>(selmaxidSQL);
            if (list.Count() > 0)
                maxNo = list.First();

            if (!string.IsNullOrEmpty(maxNo))
            {
                //获取最大序号再+1
                maxNo = maxNo.Replace(document, "").Substring(dateformat.Length, NumLen);

                //返回最新的代码
                iNumber = int.Parse(maxNo) + 1;
                while (CheckNoInTable(targettable, targetcolumn, maxNo))
                {
                    iNumber ++;
                }
            }
            else
                iNumber = 1;

            return iNumber;
        }

        private static string GetFormatValue(string format, string document, DateTime datevalue, string dateformat, int iNumber)
        {
            return format.Replace("{document}", document)
                .Replace("{datetime}", datevalue.ToString(dateformat))
                .Replace("{Number}", iNumber.ToString().PadLeft(NumLen, '0'));
        }

        /// <summary>
        /// 检查目标数据表是否存在该记录 
        /// </summary>
        /// <param name="targettable">目标表</param>
        /// <param name="targetcolumn">表列</param>
        /// <returns></returns>
        public bool CheckNoInTable(string targettable, string targetcolumn, string billNo)
        {
            string CheckBillSql = "select count(*) from {0} WITH (TABLOCKX) where {1} = '{2}'";
            string selmaxidSQL = string.Format(CheckBillSql,targettable, targetcolumn, billNo);

            IDbConnection conn = DbService();

            IEnumerable<int> list = conn.Query<int>(selmaxidSQL);
            if (list.Any() && list.ToList()[0] > 0)
                return true;
            return false;
        }

        public string ShowNewBillNo(string document, string format, string targettable, string targetcolumn)
        {
            return ShowNewBillNo(document, format, "yyMM", targettable, targetcolumn);
        }

        /// <summary>
        ///     读取信息
        /// </summary>
        public string GetBillNo(string format, string datetimeformat, string document)
        {
            string Number = "";
            string date = string.Empty;
            string DateCode = "";
            DataTable dt = Select();
            bool bo = false;
            DateTime today = DateTime.Today;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["document"].ToString() == document)
                {
                    bo = true;
                    date = dr["Date"].ToString();
                    DateCode = today.ToString(datetimeformat);

                    if (Convert.ToDateTime(dr["Date"]).AddMonths(1) <= DateTime.Now) //隔天序号从001起
                    {
                        dr["Date"] = today.ToString("yyyy-MM");
                        dr["Number"] = "001";
                        Number = "001";

                        SetBillNo(document, Number); //修改数据库

                        return GetFormatValue(format, DateCode, Number, document);
                    }
                    Number = (dr["Number"] != Convert.DBNull) ? dr["Number"].ToString() : "0";
                }
            }

            //找不到标识，新建
            if (!bo)
            {
                Number = "001";
                date = today.ToString("yyyy-MM");
                insert(document, Number, date + "-01"); //修改数据库

                DateCode = today.ToString(datetimeformat);

                return GetFormatValue(format, DateCode, Number, document);
            }

            Number = (Convert.ToInt32(Number) + 1).ToString();
            if (Number.Length == 1)
                Number = "00" + Number;
            if (Number.Length == 2)
                Number = "0" + Number;
            SetBillNo(document, Number); //修改数据库

            return GetFormatValue(format, DateCode, Number, document);
        }

        private string GetFormatValue(string format, string datecode, string number, string document)
        {
            return format.Replace("{datetime}", datecode).Replace("{Number}", number).Replace("{document}", document);
        }


        /// <summary>
        ///     设置信息
        /// </summary>
        private void SetBillNo(string document, string Number)
        {
            Update(document, Number, DateTime.Today.ToString("yyyy-MM-") + "01");
        }

        /// <summary>
        ///     选取数据
        /// </summary>
        /// <returns></returns>
        public DataTable Select()
        {
            string SQL = "SELECT * FROM sys_BillNo ";
            return GetDataTable(SQL);
        }

        /// <summary>
        ///     选取数据
        /// </summary>
        /// <returns></returns>
        private DataTable Select(string document)
        {
            string SQL = "SELECT * FROM sys_BillNo Where [document] ='" + document + "'";

            return GetDataTable(SQL);
        }

        /// <summary>
        ///     保存配置信息
        /// </summary>
        /// <param name="document">单号前缀</param>
        /// <param name="number">numLen位序号</param>
        /// <param name="Date">日期</param>
        private void Update(string document, string number, string Date)
        {
            //this.xmlDocument.Save(this.dataPath);

            string SQL = string.Format("UPDATE sys_BillNo WITH (ROWLOCK) SET [Number]='{0}',[Date]='{1}' WHERE [document]='{2}'"
                , number, Date, document);
         
            IDbConnection conn = DbService();
            conn.Execute(SQL);
        }

        /// <summary>
        ///     插入信息
        /// </summary>
        /// <param name="document">单号前缀</param>
        /// <param name="number">numLen位序号</param>
        /// <param name="Date">日期</param>
        private void insert(string document, string number, string Date)
        {
            string SQL = "INSERT sys_BillNo WITH (TABLOCKX)(document,number,date) VALUES('{0}','{1}','{2}')";
            SQL = string.Format(SQL, document, number, Date);
            IDbConnection conn = DbService();
            conn.Execute(SQL);
        }

        /// <summary>
        ///     插入信息
        /// </summary>
        /// <param name="document">单号前缀</param>
        /// <param name="number">numLen位序号</param>
        /// <param name="Date">日期</param>
        private void insert(string document, string number, string Date, string TargetTable, string TargetColumn)
        {
            string SQL =
                "INSERT sys_BillNo WITH (TABLOCKX) (document,number,date,TargetTable,TargetColumn) VALUES('{0}','{1}','{2}','{numLen}','{4}')";
            SQL = string.Format(SQL, document, number, Date, TargetTable, TargetColumn);
            IDbConnection conn = DbService();
            conn.Execute(SQL);
        }

        #endregion
    }
}