using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft;
using Newtonsoft.Json;
namespace UkeyTech.OA.WebApp.Extenstion
{
    /// <summary>
    /// 高级查询功能
    /// by:laozijian
    /// modify:2013-03-05
    /// </summary>
    public class AdvSearchHelper
    {
        /// <summary>
        /// 获取高级查询项的设置
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static AdvSearchCondition[] GetSearchOption(string json)
        {
            return JsonConvert.DeserializeObject<AdvSearchCondition[]>(json);
        }

        public static string GetSearchOptionO(object o)
        {
            if (o != null) { 
                var list = o as List<AdvSearchCondition>;

                if(list != null)
                    return JsonConvert.SerializeObject(list);
            }

            return string.Empty;
        }

        public static string GetSearchOption(List<AdvSearchCondition> list)
        {
            return JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 获取传入的查询条件值
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<AdvSearchParam> GetSearchParam(string json)
        {
            return JsonConvert.DeserializeObject(json) as List<AdvSearchParam>;
        }

        public static List<AdvSearchParam> GetQueryString(string json)
        {
            return JsonConvert.DeserializeObject(json) as List<AdvSearchParam>;
        }
    }

      /// <summary>
    /// 高级查询条件的值
    /// </summary>
    public class AdvSearchParam
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 传入的值
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// 高级查询条件设置
    /// </summary>
    public class AdvSearchCondition
    {
        //like [{Label:'名称',Type:'类型(DateTime|CheckBox|Text|Int|ConstDict|SQLDict)',Field:"字段名", DictID:"字典ID"}]

        public const string Text = "Text";
        public const string DateTime = "DateTime";
        public const string CheckBox = "CheckBox";
        public const string Int = "Int";
        public const string ConstDict = "ConstDict";
        public const string SQLDict = "SQLDict";

        /// <summary>
        /// 标签
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 控件名称
        /// </summary>
        public string ControlName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 字典ID
        /// </summary>
        public string DictID { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 默认显示
        /// </summary>
        public bool ShowInDefault { get; set; }

        /// <summary>
        /// 传入参数
        /// </summary>
        public AdvSearchParam Param { get; set; }
    }
}