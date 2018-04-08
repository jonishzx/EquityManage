using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Clover.Core.Alias;
using Clover.Core.Common;
using Clover.Web.Core;

using UkeyTech.WebFW.DAO;
using UkeyTech.WebFW.Model;
/// <summary>
/// 在线信息处理器
/// </summary>
public class OnlineHandler
{

    public List<string> IpList = null;
    public DateTime CurrDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
    public int Count = 0;
    readonly OnlineListDAO dal = new OnlineListDAO();

    private object m_mutex = new object();

    public OnlineHandler()
    { 
        IpList = new List<string>();

        InitNowDayOnlineList();
    }

    public void InitNowDayOnlineList()
    {
        lock (this.m_mutex)
        {
            IpList.Clear();

            try
            {
                UkeyTech.WebFW.Model.OnlineList model = dal.GetModel(CurrDay);

                if (model != null)
                {
                    string[] ips = model.IPs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    IpList.AddRange(ips);

                    CurrDay = model.VisitDate;

                    Count = model.VisitCount;
                }
            }
            catch (Exception ex)
            {
                log.Current.Error("日期变更时，更新网站在线信息时发生系统错误", ex);
            }
        }
    }

    public void AddIp(string ip)
    {
        //如果日期发生变更，保存上个日期的点击信息
        lock (this.m_mutex)
        {
            try
            {
                DateTime now = DateTime.Now;

                if (now.Day != CurrDay.Day)
                {
                    if (UpdateOnlineStatus())
                    {
                        this.CurrDay = new DateTime(now.Year, now.Month, now.Day);
                        this.IpList.Clear();
                        this.Count = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    log.Current.Error("日期变更时，更新网站在线信息时发生系统错误", ex);
                }catch{

                }
            }

            if (!this.IpList.Contains(ip))
            {
                this.IpList.Add(ip);
                this.Count++;
            }
        }
    }

    public bool UpdateOnlineStatus()
    {
        //更新数据库的在线信息
        UkeyTech.WebFW.Model.OnlineList model = new UkeyTech.WebFW.Model.OnlineList();
        model.VisitDate = this.CurrDay;
        model.IPs = StringHelper.Join(",", IpList.ToArray());
        model.VisitCount = this.Count;
        bool rst = true;
        try
        {
            dal.Update(model);
        }
        catch (Exception ex)
        {
            log.Current.Error("更新网站在线信息时发生系统错误", ex);

            //尝试写入IO文件
            using (StreamWriter sw = new StreamWriter(Utility.ConvertPsyPath("~/Online/" + CurrDay.ToString("yyyyMMdd") + ".txt")))
            {
                try
                {
                    sw.Write(this.CurrDay.ToString());
                    sw.Write("\t");
                    sw.Write(model.IPs);
                    sw.Write("\t");
                    sw.Write(model.VisitCount);

                    sw.Flush();
                    sw.Close();
                }
                catch { }
            }

            rst = false;
        }

        return rst;

    }
}


public class OnlineListFactory
{

    private static OnlineHandler obj = null;
    private static object m_mutex = new object();

    public static OnlineHandler Current()
    {

        if (obj == null)
        {
            lock (m_mutex)
            {
                if (obj == null)
                    obj = new OnlineHandler();
            }
        }
        return obj;
    }
}
