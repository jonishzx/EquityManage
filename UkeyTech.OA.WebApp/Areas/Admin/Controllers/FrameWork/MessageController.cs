using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;
using System.Web.Mvc;
using Clover.Core.Collection;
using Clover.Core.Logging;
using Clover.Message.DAO;
using Clover.Message.Model;
using Clover.Web.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StructureMap;
using UkeyTech.WebFW.DAO;
using UkeyTech.OA.WebApp.Extenstion;

namespace UkeyTech.OA.WebApp.Areas.Admin.Controllers
{
    /// <summary>
    ///     消息管理控制器
    /// </summary>
    public class MessageController : AdminBaseController
    {
        private readonly MessageBoxDAO msgboxdao = ObjectFactory.GetInstance<MessageBoxDAO>();
        private readonly MessageDAO msgdao = ObjectFactory.GetInstance<MessageDAO>();
        private readonly MessageTemplateDAO templatedao = ObjectFactory.GetInstance<MessageTemplateDAO>();

        //默认视图
        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Browse, FuncName = "")]
        public ActionResult Index()
        {
            List<MessageBox> msgboxes = msgboxdao.AutoCreateMessageBoxAndGetBox(WebContext.CurrentUser.UniqueId);
            var boxtree = new Tree<MessageBox>(msgboxes);
            ViewData["BoxTreee"] = ToJsonTree(boxtree);

            return View();
        }

        //用户消息箱的json内容
        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Browse, FuncName = "")]
        public ActionResult UserMessageBox()
        {
            List<MessageBox> msgboxes = msgboxdao.AutoCreateMessageBoxAndGetBox(WebContext.CurrentUser.UniqueId);
            var boxtree = new Tree<MessageBox>(msgboxes);
            return Content(ToJsonTree(boxtree));
        }

        //用户消息列表
        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Browse, FuncName = "")]
        public ActionResult MessageList(string CodeOrName, int boxid, string BoxType, string userid)
        {
            int rstcount = 0;
            List<Message> list = null;
            userid = string.IsNullOrEmpty(userid) ? WebContext.CurrentUser.UniqueId : userid;
            string strWhere = "";
            switch (BoxType)
            {
                case "InBox":
                    if (!string.IsNullOrEmpty(CodeOrName))
                        strWhere =
                            string.Format(
                                "(Title Like '%{0}%' or MessageBody like '%{0}%' or CreatorName like '%{0}%')",
                                CodeOrName);
                    list = msgdao.GetInBoxMessage(boxid, PageSize, PageIndex, CodeOrName, true, "SendTime desc",
                        out rstcount);
                    break;
                case "OutBox":
                    list = msgdao.GetOutBoxMessage(userid,
                        PageSize, PageIndex, "", true, "CreateTime", out rstcount);
                    foreach (Message m in list)
                    {
                        if (string.IsNullOrEmpty(m.ReceiversName))
                        {
                            m.ReceiversName = AdminDAO.getAdminNames(m.Receivers);
                        }
                    }
                    break;
                case "Draft":
                    list = msgdao.GetDraftMessage(userid, PageSize, PageIndex, "", true, "CreateTime desc", out rstcount);
                    foreach (Message m in list)
                    {
                        if (string.IsNullOrEmpty(m.ReceiversName))
                        {
                            m.ReceiversName = AdminDAO.getAdminNames(m.Receivers);
                        }
                    }
                    break;
                case "Recyle":
                    list = msgdao.GetRecyleMessage(userid, PageSize, PageIndex, "", true, "SendTime desc", out rstcount);
                    break;
                default:
                    list = msgdao.GetInBoxMessage(boxid, PageSize, PageIndex, "", true, "SendTime desc", out rstcount);
                    break;
            }

            return Content(Helper.GetListJsonStr(list, rstcount));
        }

        //发送草稿的信息
        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Edit, FuncName = "")]
        public ActionResult Send(string MessageIds, string userid)
        {
            userid = string.IsNullOrEmpty(userid) ? WebContext.CurrentUser.UniqueId : userid;

            try
            {
                msgdao.SendMessage(MessageIds, userid);
                return Success();
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        //回收信息
        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Edit, FuncName = "")]
        public ActionResult Recyle(string MessageIds, string userid)
        {
            userid = string.IsNullOrEmpty(userid) ? WebContext.CurrentUser.UniqueId : userid;

            try
            {
                msgdao.RecyleMessage(MessageIds, userid);
                return Success();
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        //还原已回收的信息
        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Edit, FuncName = "")]
        public ActionResult Restore(string MessageIds, string userid)
        {
            userid = string.IsNullOrEmpty(userid) ? WebContext.CurrentUser.UniqueId : userid;

            try
            {
                msgdao.RestoreRecycleMessage(MessageIds, userid);
                return Success();
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        //删除已回收的信息
        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Edit, FuncName = "")]
        public ActionResult Delete(string MessageIds, string userid)
        {
            userid = string.IsNullOrEmpty(userid) ? WebContext.CurrentUser.UniqueId : userid;

            try
            {
                msgdao.DeleteALL(MessageIds, userid);
                return Success();
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        //清空回收站的信息
        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Edit, FuncName = "")]
        public ActionResult Clean(int BoxId, string userid)
        {
            userid = string.IsNullOrEmpty(userid) ? WebContext.CurrentUser.UniqueId : userid;

            try
            {
                msgdao.Clean(BoxId, userid);
                return Success();
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        //清空回收站的信息
        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Edit, FuncName = "")]
        public ActionResult WithDraw(string messageId, string userid)
        {
            userid = string.IsNullOrEmpty(userid) ? WebContext.CurrentUser.UniqueId : userid;

            try
            {
                msgdao.WithDrawMessage(messageId, userid);
                return Success();
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }

        #region 消息的新建与修改

        //验证方法
        private bool ValidateMessage(Message model)
        {
            if (String.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "请填写标题 ");
            }
            if (String.IsNullOrEmpty(model.MessageBody))
            {
                ModelState.AddModelError("MessageBody", "请填写消息内容 ");
            }

            /*
            if (ModelState.IsValid &&  msgdao.CheckExistsSameID(model.MessageID, model.DictID.ToString()))
            {
                ModelState.AddModelError("MessageID", "该代码已经存在，请输入另外一个");
            }
            */
            return ModelState.IsValid;
        }

        //初始化数据
        private void LoadInitDataForMessage(Message model)
        {
            if (!IsEdit)
            {
                model.TargetId = Request["TargetId"];
                model.TargetObject = Request["TargetObject"];
                model.TemplateCode = Request["TemplateCode"];
            }
        }

        //信息添加     
        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Create, FuncName = "新增资料")]
        public ActionResult CreateMessage()
        {
            var model = new Message();
            LoadInitDataForMessage(model);
            model.MessageId = Guid.NewGuid().ToString();
            return View("Create", model);
        }

        //信息添加(post)    
        [HttpPost]
        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Create, FuncName = "新增资料")]
        public ActionResult CreateMessage(Message model)
        {
            LoadInitDataForMessage(model);
            string command = Request["command"];
            if (ValidateMessage(model))
            {
                try
                {
                    MessageTemplate tempalte = null;
                    if (!string.IsNullOrEmpty(Request["TemplateCode"]))
                    {
                        //代表有业务对象关联,需要设置模版的值
                        tempalte = templatedao.GetModel(Request["TemplateCode"]);
                    }
                    else
                    {
                        tempalte = templatedao.GetModel("Common");
                    }
                    model.Creator = WebContext.CurrentUser.UniqueId;
                    model.CreatorName = WebContext.CurrentUser.UserName;
                    model.CreateTime = DateTime.Now;
                    model.CanReplay = tempalte.CanReplay;
                    model.MessageAction = tempalte.MessageAction;
                    model.OperationAction = tempalte.OperationAction;
                    model.NeedAccept = tempalte.NeedAccept;
                    model.NeedRead = tempalte.NeedRead;
                    model.ExtendAction1 = tempalte.ExtendAction1;
                    model.ExtendAction2 = tempalte.ExtendAction2;
                    model.TargetId = Request["TargetId"];
                    model.TargetObject = Request["TargetObject"];
                    model.ObjectStatus = DataRowState.Added;

                    if (command.CompareTo("Send") == 0)
                    {
                        msgdao.SaveAndSend(model, WebContext.CurrentUser.UniqueId);
                    }
                    else
                    {
                        msgdao.Save(model);
                    }
                    //日志记录
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), " 添加信息操作:", model);

                    return RefreshParentAndCloseFrame();
                }
                catch (Exception ex)
                {
                    ShowError("创建失败");
                    LogCentral.Current.Error("创建失败", ex);
                }
            }
            LoadInitDataForMessage(model);
            return View("Create", model);
        }


        //信息修改 
        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Edit, FuncName = "修改附件信息资料")]
        public ActionResult EditMessage(string id)
        {
            Message model = null;
            IsEdit = false;
            if (!string.IsNullOrEmpty(id))
            {
                IsEdit = true;
                model = msgdao.GetModel(id);
            }
            LoadInitDataForMessage(model);
            return View("Create", model);
        }

        //信息修改(post)    
        [HttpPost]
        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Edit, FuncName = "新增附件信息资料")]
        public ActionResult EditMessage(string id, Message model)
        {
            string command = Request["command"];

            if (ValidateMessage(model))
            {
                try
                {
                    Message m = msgdao.GetModel(id);
                    if (!string.IsNullOrEmpty(m.Status))
                        return Content("该消息已经发送或废弃");

                    m.Title = model.Title;
                    m.Receivers = model.Receivers;
                    m.ReceiversName = model.ReceiversName;
                    m.MessageBody = model.MessageBody;
                    m.Status = model.Status;
                    m.ObjectStatus = DataRowState.Modified;

                    if (command.CompareTo("Send") == 0)
                    {
                        msgdao.SaveAndSend(m, WebContext.CurrentUser.UniqueId);
                    }
                    else
                    {
                        msgdao.Save(m);
                    }

                    //日志记录
                    OPLogDAO.Log(WebContext.CurrentUser, Utility.GetViewerIP(), " 更新附件信息操作:", model);

                    return RefreshParentAndCloseFrame();
                }
                catch (Exception ex)
                {
                    ShowError("更新失败");
                    LogCentral.Current.Error("更新失败", ex);
                }
            }

            LoadInitDataForMessage(model);
            return View("Create", model);
        }

        #endregion

        #region 消息阅读

        private readonly BoxMessageDAO boxmsgDAO = ObjectFactory.GetInstance<BoxMessageDAO>();

        private Message InitMessageBiew(string MessageId)
        {
            Message model = msgdao.GetModel(MessageId);
            int rstcount = 0;
            List<BoxMessage> boxmessages = msgdao.GetMessageStatus(MessageId, int.MaxValue, 1, "", true, "ReceiveTime",
                out rstcount);

            //发件人才能查看接收人的答复情况
            if (model.Creator.CompareTo(WebContext.CurrentUser.UniqueId) == 0 ||
                WebContext.CurrentUser.UniqueId == SystemVar.AdminId)
            {
                ViewData["BoxMessage"] = boxmessages;
            }
            if (model.Receivers.IndexOf(WebContext.CurrentUser.UniqueId) >= 0)
            {
                //说明当前人也是收件人
                BoxMessage it = boxmessages.Find(x => x.Receiver.CompareTo(WebContext.CurrentUser.UniqueId) == 0);

                if (it != null)
                {
                    if (it.Status == "E" && it.Receiver.CompareTo(model.Creator) != 0) //已撤回且发件人不是收件人
                    {
                        model.Status = "E";
                        return model;
                    }
                    if (it.ReadTime == null && !model.NeedRead.Value)
                    {
                        //未读但无需要做手工阅读动作,则进行状态更新
                        it.ReadTime = DateTime.Now;
                        it.Status = "B";
                        it.ReadComment = "已读";
                        boxmsgDAO.Update(it);
                    }
                    if (it.ReadTime == null && model.NeedRead.Value)
                    {
                        model.NeedRead = true;
                    }
                    else
                    {
                        model.NeedRead = false;
                    }
                    if (it.OpTime == null && model.NeedAccept.Value)
                    {
                        model.NeedAccept = true;
                    }
                    else
                    {
                        model.NeedAccept = false;
                    }
                }
            }
            else
            {
                //当前查看人非收件人
                model.NeedRead = false;
                model.NeedAccept = false;
            }

            return model;
        }

        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Edit, FuncName = "")]
        public ActionResult ViewMessage(string id)
        {
            Message model = InitMessageBiew(id);
            if (model.Status == "E")
                return Content("该消息已被发件人撤回");

            return View(model);
        }

        [HttpPost]
        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Edit, FuncName = "")]
        public ActionResult ViewMessage(string id, FormCollection fmc)
        {
            string command = fmc["command"];
            int rstcount = 0;
            List<BoxMessage> boxmessages = msgdao.GetMessageStatus(id, int.MaxValue, 1,
                "Receiver='" + WebContext.CurrentUser.UniqueId + "'",
                true, "ReceiveTime", out rstcount);

            if (boxmessages.Count == 0)
                return new EmptyResult();

            BoxMessage it = boxmessages[0];
            switch (command)
            {
                case "Read":
                    it.ReadTime = DateTime.Now;
                    it.ReadComment = Request["ReadComment"];
                    boxmsgDAO.Update(it);
                    break;
                case "Accept":
                    it.OpTime = DateTime.Now;
                    it.Result = "Accept";
                    it.OpComment = Request["OpComment"];
                    boxmsgDAO.Update(it);
                    break;
                case "Reply": //回复消息
                    var messageDAO = ObjectFactory.GetInstance<MessageDAO>();
                    var orginalMessage =  messageDAO.GetModel(id);
                    using (var scope = new System.Transactions.TransactionScope(TransactionScopeOption.Required))
                    {
                        it.OpTime = DateTime.Now;
                        it.Result = "Reply";
                        it.OpComment = Request["OpComment"];
                        boxmsgDAO.Update(it);

                        //回复消息需要重新 发送消息给发件人，且需要记录发件人的消息ID
                        orginalMessage.Receivers = orginalMessage.Sender ?? orginalMessage.Creator; //收件人置换
                        orginalMessage.CreateTime = orginalMessage.SendTime = DateTime.Now;
                        orginalMessage.Sender = orginalMessage.Creator = WebContext.CurrentUser.UniqueId;
                        orginalMessage.Status = "A";

                        orginalMessage.Title = fmc["Title"];
                        orginalMessage.MessageId = Guid.NewGuid().ToString();
                        orginalMessage.ReferMessageId = !string.IsNullOrEmpty(orginalMessage.ReferMessageId) ? orginalMessage.ReferMessageId : id;
                        orginalMessage.MessageBody = fmc["MessageBody"];
                        messageDAO.SaveAndSend(orginalMessage, WebContext.CurrentUser.UniqueId);

                        RefreshParentAndCloseFrame();

                    }
                    //生成新的消息
                    break;
                case "Reject":
                    it.OpTime = DateTime.Now;
                    it.Result = "Reject";
                    it.OpComment = Request["OpComment"];
                    boxmsgDAO.Update(it);
                    break;
            }
            Message model = InitMessageBiew(id);
            return View(model);
        }

        #endregion

        #region 消息插件

        /// <summary>
        ///     消息插件输出
        /// </summary>
        /// <returns></returns>
        //[CloverAuthorize(ModuleCode = "Message", FuncCode = Consts.Edit, FuncName = "")]
        public ActionResult MessageWidget()
        {
            var webcontext = ObjectFactory.GetInstance<IWebContext>();

            if (webcontext.CurrentUser == null)
                return null;

            var dal = ObjectFactory.GetInstance<MessageDAO>();
            var boxdal = ObjectFactory.GetInstance<MessageBoxDAO>();

            string userid = webcontext.CurrentUser.UniqueId;
            List<MessageBox> boxes = boxdal.AutoCreateMessageBoxAndGetBox(userid);
            
            if(boxes == null || boxes.Count == 0)
                return new EmptyResult();

            MessageBox inbox = boxes.Find(x => x.BoxType.CompareTo("InBox") == 0);
            if (inbox == null)
                return new EmptyResult();
            int rowscount = 0;
            List<Message> result = dal.GetInBoxMessage(inbox.MessageBoxId, 10, 1, " Status is null ", true, "ReceiveTime desc",
                out rowscount);

            var groups = new List<WorkItemsGroup<Message>>();
            var typelist = new List<string>();

            WorkItemsGroup<Message> group = null;
            foreach (Message m in result)
            {
                m.SenderName = AdminDAO.getAdminName(m.Sender);
                string groupname = m.ReceiveTime.ToString("yyyy-MM-dd");
                if (!typelist.Contains(groupname))
                {
                    group = new WorkItemsGroup<Message>();
                    group.Id = group.Title = groupname;
                    groups.Add(group);
                    typelist.Add(groupname);
                    List<Message> children = result.FindAll(x => x.ReceiveTime.ToString("yyyy-MM-dd") == group.Title);
                    foreach (Message c in children)
                    {
                        group.Items.Add(c);
                    }
                }
            }
            return Content(JsonConvert.SerializeObject(groups, new IsoDateTimeConverter()));
        }

        #endregion

        #region 收件箱树形态形成

        private string BuildJsonTree(TreeNode<MessageBox> tree, string parentId, bool existsCheckColumn)
        {
            var builder = new StringBuilder();
            List<TreeNode<MessageBox>> list = tree.getChildren();
            string str = tree.getNode().Id;
            builder.Append("{\"id\":" + ("\"" + str + "\"") +
                           ",\"text\":\"" + tree.getNode().Name +
                           (tree.getNode().BoxType == "InBox" && tree.getNode().InBoxCount > 0
                               ? "(" + tree.getNode().InBoxCount + ")"
                               : "") + "\"" +
                           ",\"BoxType\":\"" + tree.getNode().BoxType + "\""
                           + (existsCheckColumn ? (",\"checked\":" + "checked") : string.Empty));

            if (list.Count > 0)
            {
                builder.Append(",\"children\":[");
                foreach (var row in list)
                {
                    string str2 = BuildJsonTree(row, str, existsCheckColumn);
                    builder.Append(((str2 == string.Empty) ? string.Empty : (str2 + ",")));
                }
                builder.Remove(builder.Length - 1, 1).Append("]");
            }
            builder.Append("},");

            return builder.ToString().Trim(new[] {','});
        }

        public string ToJsonTree(Tree<MessageBox> data)
        {
            string str = "[";
            if ((data != null) && (data.GetRoot().Count > 0))
            {
                List<TreeNode<MessageBox>> list = data.FindRoot();
                foreach (var m in list)
                {
                    str += BuildJsonTree(m, m.getNode().Id, false) + ",";
                }
            }

            str = str.Trim(new[] {','}) + "]";
            return str;
        }

        #endregion
    }

    /// <summary>
    /// 组项目内容
    /// </summary>
    public class WorkItemsGroup<T>
    {
        private List<T> _items;

        public string Id { get; set; }

        public string Title { get; set; }

        public List<T> Items
        {
            get { return _items ?? (_items = new List<T>()); }
        }
    }

    /// <summary>
    /// 组项目内容
    /// </summary>
    public class WorkItemsGroup
    {
        private List<object> _items;

        public string Id { get; set; }

        public string Title { get; set; }

        public List<object> Items
        {
            get { return _items ?? (_items = new List<object>()); }
        }
    }
}