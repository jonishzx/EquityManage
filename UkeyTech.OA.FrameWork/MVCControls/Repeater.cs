using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Mvc;

namespace RepeaterInMvc.Codes
{
    public class MvcRepeater : Repeater
    {
        /// <summary>
        /// ViewData中的键名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 得到ViewPage对象
        /// </summary>
        public ViewPage ViewPage
        {
            get { return base.Page as ViewPage; }
        }

        /// <summary>
        /// 重写Onload事件 用于绑定数据
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            this.DataSource = this.ViewPage.ViewData[this.Name]; //得到数据源
            this.DataBind();    //绑定  这样就不用手动写N个绑定了

            base.OnLoad(e);
        }
    }
}
