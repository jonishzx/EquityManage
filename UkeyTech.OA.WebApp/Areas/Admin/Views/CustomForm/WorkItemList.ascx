<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="UkeyTech.OA.FrameWork" Namespace="RepeaterInMvc.Codes" TagPrefix="MVC"  %>

<MVC:MvcRepeater ID="rpParentMenu" Name="WorkItemsGroup" runat="server">
    <ItemTemplate>
        <div id="sample-ct">
            <tpl for=".">
                        <div>
                        <a name="<%# Eval("id")%>"></a>
                        <h2><div><%# Eval("title")%></div></h2>
                            <dl>
                        <tpl for="samples">
                             <MVC:MvcRepeater ID="rpParentMenu" Name="Items" runat="server" >
                                <ItemTemplate>
                                    <div class="thumb-wrap">
                                         <img src="<%# Eval("ImageUrl") != null ? Eval("ImageUrl") : Url.Content("~/Content/Images/noimg.gif")%>" title="<%# Eval("FormName") %>" />
                                        <div>
                                            <H4><%# Eval("FormName")%></H4>
                                            <P><%# Eval("Descn")%></P>
                                            <p><a class="add" url=" <%# (Eval("ExternalFormUrl")!=null && Eval("ExternalFormUrl").ToString() != string.Empty ? Eval("ExternalFormUrl") : "UserSubmitData") + "?formid=" + Eval("id").ToString()%>" href='javascript:void(0);'>添加</a> 
                                                | <a href='javascript:void(0)' onclick='openListParentTab("<%# Eval("FormName")%>","<%# Url.Action("UserSubmitDataList","CustomForm") + "?formid=" + Eval("id").ToString()%>")' >信息列表</a></p>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </MVC:MvcRepeater>
                         </tpl>
                        <div style="clear:left"></div>
                        </dl>
                       </div>
                    </tpl>
        </div>
    </ItemTemplate>
</MVC:MvcRepeater>
