<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master"
    Inherits="System.Web.Mvc.ViewPage<Clover.Config.WebSiteSetting.WebSiteConfigInfo>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    网站访问设置
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Url.Content("~/content/jquery.bt.css")%>" />
    <script type="text/javascript" src="<%=Url.Content("~/scripts/jquery.bt.min.js")%>"></script>
    <!--[if IE]><script src="<%=Url.Content("~/scripts/excanvas.js")%>" type="text/javascript" charset="utf-8"></script><![endif]-->
    <script type="text/javascript">

        $(document).ready(function () {
            //tooltip 
            $('.IpdenyaccessTip').each(function (e) {
                if ($(this).attr("title") != "")
                    $(this).bt($(this).attr("title"), { trigger: ['focus', 'blur'], positions: 'right' });
            })
        });
    </script>
    <style type="text/css">
        .form-element
        {
            padding: 0 !important;
            float: left;
        }
        .ym-fbox-text-label, .form-element, .ym-fbox-text, textarea, label
        {
            margin: 5px 0 5px 10px;
            position: inherit !important;
        }
        .panel-header
        {
            position: static !important;
        }
        .panel
        {
            margin: 10px;
        }
        textarea
        {
            width:400px !important;
            height:50px !important;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="editpage" region="center" style="padding: 0;">
        <%using (Html.BeginForm())
          { %>
        <div class="easyui-panel" title="IP访问控制">
            <div class="ym-form linearize-form ym-columnar zcolumn">
                <div class="ym-form-fields">
                    <div class="ym-fbox-text">
                        <label class="ym-fbox-text-label w150 required imgfront">
                            主页IP访问列表:
                        </label>
                        <div>
                            <div class="radioButtonList">
                                <label for="IpdenyaccessType">
                                    禁止</label><input name="IpdenyaccessType" <%= Model.IpdenyaccessType == 0 ? "checked=checked" : ""  %>
                                        type="radio" value="0" style="width:50px !important"/>
                                <label for="IpdenyaccessType">
                                    允许</label><input name="IpdenyaccessType" <%= Model.IpdenyaccessType == 1 ? "checked=checked" : ""  %>
                                        type="radio" value="1" style="width:50px !important"/>
                            </div>
                        </div>
                        <div class="form-element" style="vertical-align: middle;">
                            <textarea name="Ipdenyaccess" title="" class="textarea w200 IpdenyaccessTip"><%=Model.Ipdenyaccess%></textarea>
                            <%= Html.ValidationMessage("Ipdenyaccess")%></div>
                        <div class="form-element w300">
                            当用户处于本列表中的 IP 地址时将禁止或允许访问本网站. 每个 IP段换一行, 例如 "192.168.1.1-192.168.2.255"(不含引号) 可匹配
                            192.168.1.1~192.168.2.255 范围内的所有地址
                        </div>
                    </div>
                    <div class="ym-fbox-text">
                        <label class="ym-fbox-text-label w150 required imgfront">
                            禁止访问提示:</label>
                        <div class="form-element" style="vertical-align: middle;">
                            <textarea name="IpdenyaccessTip" class="textarea w200 IpdenyaccessTip"><%=Model.IpdenyaccessTip%></textarea>
                        </div>
                    </div>
                    <div style="clear: both;">
                    </div>
                </div>
            </div>
        </div>
        <div class="easyui-panel" title="网站资源IP访问控制">
            <div class="ym-form linearize-form ym-columnar zcolumn">
                <div class="ym-form-fields">
                    <div class="ym-fbox-text">
                        <label class="ym-fbox-text-label w150 required imgfront">
                            网站资源IP访问列表：</label>
                        <div class="form-element" style="vertical-align: middle;">
                            <textarea name="Ipdenyaccess" title="" class="textarea w200 IpdenyaccessTip"><%=Model.Resipaccess%></textarea>
                            <%= Html.ValidationMessage("Resipaccess")%></div>
                        <div class="form-element w300">
                            当用户处于本列表中的 IP 地址时将禁止或允许访问本网站资源. 每个 IP段换一行, 例如 "192.168.1.1-192.168.2.255"(不含引号)
                            可匹配 192.168.1.1~192.168.2.255 范围内的所有地址
                        </div>
                    </div>
                    <div class="ym-fbox-text">
                        <label class="ym-fbox-text-label w150 required imgfront">
                            禁止访问提示:</label>
                        <div class="form-element" style="vertical-align: middle;">
                            <textarea name="ResIpaccessTip" class="textarea w200 IpdenyaccessTip"><%=Model.ResipaccessTip%></textarea>
                        </div>
                    </div>
                    <div style="clear: both;">
                    </div>
                </div>
            </div>
        </div>
        <div class="easyui-panel" title="管理台IP访问控制">
            <div class="ym-form linearize-form ym-columnar zcolumn">
                <div class="ym-form-fields">
                    <div class="ym-fbox-text">
                        <label class="ym-fbox-text-label w150 required imgfront">
                            管理台IP访问列表：</label>
                        <div class="form-element" style="vertical-align: middle;">
                            <textarea name="Ipdenyaccess" title="" class="textarea w200 IpdenyaccessTip"><%=Model.Adminipaccess%></textarea>
                            <%= Html.ValidationMessage("Adminipaccess")%></div>
                        <div class="form-element w300">
                            只有当管理员处于本列表中的 IP 地址时才可以访问论坛系统设置, 列表以外的地址访问将无法访问, 请务必慎重使用本功能. 每个 IP段换一行, 例如 "192.168.1.1-192.168.2.255"(不含引号)
                            可匹配 192.168.1.1~192.168.2.255 范围内的所有地址, 留空为所有 IP 均可访问系统设置
                        </div>
                    </div>
                    <div class="ym-fbox-text ">
                        <label class="ym-fbox-text-label w150 required imgfront">
                            禁止访问提示:</label>
                        <div class="form-element" style="vertical-align: middle;">
                            <textarea name="AdminipaccessTip" class="textarea w200 IpdenyaccessTip"><%=Model.AdminipaccessTip%></textarea>
                        </div>
                    </div>
                    <div style="clear: both;">
                    </div>
                </div>
            </div>
        </div>
        <div style="display: none;">
            <div class="easyui-panel" title="禁止访问时间控制">
                <div class="ym-form linearize-form ym-columnar zcolumn">
                    <div class="ym-form-fields">
                        <div class="ym-fbox-text">
                            <label class="ym-fbox-text-label w150 required imgfront">
                                禁止访问时间段：</label>
                            <div class="form-element" style="vertical-align: middle;">
                                <textarea name="Visitbanperiods" title="" class="textarea w200 IpdenyaccessTip"><%=Model.Visitbanperiods%></textarea>
                                <%= Html.ValidationMessage("Visitbanperiods")%></div>
                            <div class="form-element w300">
                                每天该时间段内用户不能访问网站, 请使用 24 小时时段格式, 每个时间段一行, 如需要也可跨越零点, 留空为不限制. 例如:每日晚 11:25 到次日早 5:05
                                可设置为: 23:25-5:05, 每日早 9:00 到当日下午 2:30 可设置为: 9:00-14:30. 注意: 格式不正确将可能导致意想不到的问题. 所有时间段设置均以论坛系统默认时区为准,
                                不受用户自定义时区的影响
                            </div>
                        </div>
                        <div class="ym-fbox-text ">
                            <label class="ym-fbox-text-label w150 required imgfront">
                                禁止访问提示:</label>
                            <div class="form-element" style="vertical-align: middle;">
                                <textarea name="AdminipaccessTip" class="textarea w200 IpdenyaccessTip"><%=Model.VisitbanperiodsTip%></textarea>
                            </div>
                        </div>
                        <div style="clear: both;">
                        </div>
                    </div>
                </div>
            </div>
            <div class="easyui-panel" title="网站关闭设置">
                <div class="ym-form linearize-form ym-columnar zcolumn">
                    <div class="ym-form-fields">
                        <div class="ym-fbox-text">
                            <label class="ym-fbox-text-label w150 required imgfront">
                                网站状态：</label>
                            <div class="form-element" style="vertical-align: middle; width: 210px">
                                <div>
                                    <label>
                                        开放</label><input name="Closed" <%= Model.Closed == 0 ? "checked=checked" : ""  %>
                                            type="radio" value="0" />
                                    <label>
                                        关闭</label><input name="Closed" <%= Model.Closed == 1 ? "checked=checked" : ""  %>
                                            type="radio" value="1" /></div>
                            </div>
                        </div>
                        <div style="clear: both;">
                        </div>
                        <div class="ym-fbox-text ">
                            <label class="ym-fbox-text-label w150 required imgfront">
                                关闭提示:</label>
                            <div class="form-element" style="vertical-align: middle;">
                                <textarea name="Closedreason" class="textarea w200 IpdenyaccessTip"><%=Model.Closedreason%></textarea>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%} %>
        <div class="SouthForm form-action">
            <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="SubmitForm();" id="A1">
                确定</a>
        </div>
    </div>
</asp:Content>
