<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" Title="流程参与者选择" %>

<%@ Register src="../Shared/Loading.ascx" tagname="Loading" tagprefix="ld" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">    
    <script type="text/javascript">
        var rooturl = '<%= Clover.Web.Core.Utility.ConvertAbsoulteUrl("~/") %>';
    </script>
    <%:Styles.Render("~/styles/theme/" + UkeyTech.OA.WebApp.Helper.GetSkin())%>
	<script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.7.2.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.easyui.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/scripts/EasyUI/locale/easyui-lang-zh_CN.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/common.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/WorkFlow/UserMgmtSelector.js")%>"></script>    
</head>
<body>
    <div class="SearchDiv">
        代码或名称：<input type="text" id="txtCode" style="width: 120px;" />
        <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="LoadGrid($('#txtCode').val());">
            查询</a>
    </div>
    <div class="easyui-layout" style="margin: 5px;width:99%">
       <div id="center" region="center" split="true" title="拥有者" style="width: 10px;">
            <div id="tabs" class="easyui-tabs" fit="true" border="false">
                <%if(string.IsNullOrEmpty(Request["type"]) || Request["type"].Equals("Role")){ %>
                <div title="角色" cache="false">
                    <div class="easyui-layout" fit="true">
                        <div region="center" border="false">
                            <table id="tbRole">
                            </table>
                        </div>
                    </div>
                </div>
                <%} %>
                <%if(string.IsNullOrEmpty(Request["type"]) || Request["type"].Equals("User")){ %>

                <div title="用户" cache="false">
                    <div class="easyui-layout" fit="true">
                        <div region="center" border="false">
                            <table id="tbUser">
                            </table>
                        </div>
                    </div>
                </div>
                <%} %>
                <%if(string.IsNullOrEmpty(Request["type"]) || Request["type"].Equals("Group")){ %>
                <div id="divGroup" title="组织架构" cache="false" runat="server">
                    <div class="easyui-layout" fit="true" border="">
                        <div region="center" border="false">
                            <table id="tbGroup">
                            </table>
                        </div>
                    </div>
                </div>
                <%} %>
                <%if(string.IsNullOrEmpty(Request["type"]) || Request["type"].Equals("Position")){ %>
                <div id="divPosition" title="岗位" cache="false" runat="server">
                    <div class="easyui-layout" fit="true" border="">
                        <div region="center" border="false">
                            <table id="tbPosition">
                            </table>
                        </div>
                    </div>
                </div>
                <%} %>
                <%if(!string.IsNullOrEmpty(Request["type"]) && Request["type"].Equals("GroupPosition")){ %>
                 <div id="div1" title="部门岗位" cache="false" runat="server">
                    <div class="easyui-layout" fit="true" border="">
                        <div region="center" border="false">
                            <table id="tbGroupPosition">
                            </table>
                        </div>
                    </div>
                </div>
                <%} %>
            </div>
        </div>
       <div id="south" region="south" style="height:150px;">
            <div>
                <textarea id="performerValue" style="width:99.5%;height:100px;"></textarea>
                <input id="hidperformerValue" type=hidden />
            </div>
            <div style="text-align:center">
                 <a class="easyui-linkbutton" icon="icon-add" href="javascript:void(0);" onclick="addSelections();"
            id="A2">添加</a>
            <a class="easyui-linkbutton" icon="icon-remove" href="javascript:void(0);" onclick="clearSelections();"
            id="A3">清空</a>
                   <a class="easyui-linkbutton" icon="icon-ok" href="javascript:void(0);" onclick="Save();"
            id="A1">确定</a> <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)" onclick="CloseTheWin();"
                runat="server" id="btnCancel">取消</a>
            </div>
       </div>
    </div>
   
    <script type="text/javascript" language="javascript">
        $(".easyui-layout").height($(document).height() - 60);

        function init() {
            <%if(string.IsNullOrEmpty(Request["type"]) || Request["type"].Equals("Role")){ %>
            LoadRoleGrid();<%} %>
            <%if(string.IsNullOrEmpty(Request["type"]) || Request["type"].Equals("Group")){ %>
            setTimeout("LoadGroupGrid();", 200);<%} %>
            <%if(string.IsNullOrEmpty(Request["type"]) || Request["type"].Equals("Position")){ %>
            setTimeout("LoadPositionGrid();", 400);<%} %>
            <%if(!string.IsNullOrEmpty(Request["type"]) && Request["type"].Equals("GroupPosition")){ %>
            setTimeout("LoadGroupPositionGrid();", 300);<%} %>
            <%if(string.IsNullOrEmpty(Request["type"]) || Request["type"].Equals("User")){ %>
            setTimeout("LoadUserGrid();", 500);<%} %>
        }

        $(document).ready(function () {
            init();
        });
        
        var baseurl = '<%=Url.Action("FuncPermission","Permission")%>'  + "?type=";
        var FuncPermissionList = baseurl + "FuncPermissionList" ;
        
        var rolebaseurl = '<%=Url.Action("Role","Permission")%>' + "?type=RoleList";
        var groupbaseurl = '<%=Url.Action("Group","Permission")%>' + "?type=GroupList";
        var userbaseurl = '<%=Url.Action("GetAccountList","Account")%>';
        var postionbaseurl = '<%=Url.Action("Position","Permission")%>' + "?type=PositionList";
        var grouppostionbaseurl = '<%=Url.Action("WFPositionList","Permission")%>';

        function Save() {
            var val = "[" + $("#performerValue").val() + "][" + $("#hidperformerValue").val() + "]";
            window.parent.setUserSelection(val); 
            CloseTheWin();
        }

        var selectItems = {};        

        function addSelections() {
            var keys = GetSelectedGridKey().split(',');
            var names = GetSelectedGridKeyName().split(',');
            var title = GetSelectedTabTitle();
            var isexist = false;            
            if (selectItems[title]) {
                for(var i in keys){
                    for (var o in selectItems[title]) {
                        if (selectItems[title][o].key == keys[i]) {
                            isexist = true;
                            break;
                        }
                    }
                    if(!isexist)
                        selectItems[title].push({ key: keys[i], value: names[i] });

                    isexist = false;
                }
            } else {

                selectItems[title] = [];
                for (var o in keys) {
                    selectItems[title].push({ key: keys[o], value: names[o] });
                }
            }

            var disName = "", diskeys = "";

            for (var t in selectItems) {
                disName += t + ":";
                diskeys += t + ":";
                for (var m in selectItems[t]) {
                    disName += selectItems[t][m].value + ",";
                    diskeys += selectItems[t][m].key + ",";
                }
                if(diskeys[diskeys.length -1 ] == ',')
                    diskeys = diskeys.substring(0 ,diskeys.length -1 );
                if(disName[disName.length -1 ] == ',')
                    disName = disName.substring(0 ,disName.length -1 );
                disName += ";";
                diskeys += ";";
            }

            $("#performerValue").val(disName);
            $("#hidperformerValue").val(diskeys);
        }

        function clearSelections() {
            selectItems = {};
            $("#performerValue").val('');
            $("#hidperformerValue").val(''); 
        }
    </script>
</body>
</html>
