using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UkeyTech.OA.FrameWork.Framework
{
    /// <summary>
    /// 函数调用结果
    /// </summary>
    public class InvokeResult
    {
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        StringBuilder log = new StringBuilder(50);
        /// <summary>
        /// 增加日志文本
        /// </summary>
        /// <param name="text">文本</param>
        public void AppendLog(string text)
        {
            log.Append(text);
        }

        /// <summary>
        /// 获取日志内容
        /// </summary>
        /// <returns></returns>
        public string GetLog()
        {
            return log.ToString();
        }

        private bool _rst = true;
        /// <summary>
        /// 调用结果
        /// </summary>
        public bool Result { get { return _rst; } set { _rst = value; } }
    }
}
