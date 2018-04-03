<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" Title="功能授权" %>

<%@ Register src="../Shared/Loading.ascx" tagname="Loading" tagprefix="ld" %>
<%@ Register src="../Shared/PopupWin.ascx" tagname="PopupWin" tagprefix="uc2" %>
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
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/Permission/RolePermissionTree.js")%>"></script>    
</head>
<body>
    <div class="SearchDiv">
       <%-- 角色编码：<input type="text" id="txtCode" style="width: 120px;" />
         角色名称：<input type="text" id="txtName" style="width: 120px;" />
        <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="LoadRoleGrid($('#txtCode').val(), $('#txtName').val());">
            查询</a> --%>
        <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-config" onclick="PopupClonePermission();">
            克隆权限</a> 
    </div>
    <div class="easyui-layout" style="margin: 5px;">
        <div id="west" region="west" split="true" title="角色树" style="width: 10px;">
            <ul id="treeRole">
            </ul>
        </div>
        <div id="center" region="center" title="模块" style="width: auto; padding: 5px;">
            <div class="easyui-layout" fit="true">
                <div region="north" border="false" class="SearchDiv" style="border: 0;">
                    代码或名称：<input type="text" id="txtModuleCode" style="width: 120px;" />
                    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                        onclick="LoadModuleGrid();"></a>
                </div>
                <div region="center">
                    <table id="tbModule">
                    </table>
                </div>
            </div>
        </div>
        <div id="east" region="east" split="true" title="功能" style="width: 10px; padding: 5px;">
            <div class="easyui-layout" fit="true">
                <div region="north" style="height:200px;">
                    <table id="tbModuleFunction">
                    </table>
                </div>
                <div region="center" >
                    <table id="tbModuleFunctionDataRule">
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div style=" display: none;">
     <div id="dlgCloneFuncPermission" title="权限克隆" style="width:400px; height: 140px;">
         <table cellpadding="2">
            <tr>
                <td>
                    当前角色（源头）：
                </td>
                <td>
                    <input id="CurrOwenerName" type="text" class="form-item-text easyui-validatebox" readonly=readonly
                        required="true" value="" maxlength="100" />
                </td>
            </tr>
            <tr>
                <td>
                    克隆角色（目标）：
                </td>
                <td>
                  <%
                          Html.RenderPartial(UkeyTech.OA.WebApp.Helper.PopupControlPath,
                                      new ViewDataDictionary(new
                                      {
                                          IDControlName = "CloneRoleValue",
                                          TextControlName = "CloneRoleName",
                                          DictID = "AllEnabledRole",
                                          Value = ViewData["CloneRoleValue"],
                                          MutilSelect = false,
                                          Width = "300",
                                          Required = true
                                      }));%>
                </td>
            </tr>
        </table>
        <div region="south" border="false" class="SouthForm">
        <a class="easyui-linkbutton" icon="icon-ok" href="#" onclick="ClonePermission();"
            id="A1">确定</a> <a class="easyui-linkbutton" icon="icon-cancel" onclick="$('#dlgCloneFuncPermission').window('close');"
                runat="server" id="btnCancel">取消</a>
        </div>     
    </div>
    </div>
    <uc2:PopupWin ID="PopupWin1" runat="server" />
    <script type="text/javascript" language="javascript">
        $(".easyui-layout").height($(document).height() - 37);
        $("#east").width($(document).width() * 0.35);
        $("#west").width($(document).width() * 0.15); 
        $.fn.pagination.defaults.displayMsg = '';
        function init() {
            LoadRoleGrid();
            setTimeout("LoadModuleGrid();", 500);
        }
        var functions = '<%=ViewData["Functions"]%>';
        $(document).ready(function () {
            //权限获取
            LoadPageModuleFunction("FuncPermission", init);
        });
        
        var baseurl = '<%=Url.Action("FuncPermission","Permission")%>'  + "?type=";
        var FuncPermissionList = baseurl + "FuncPermissionList" ;
        var FuncPermissionDataRuleList = baseurl + "FuncPermissionDataRuleList" ;
        var SetDataRulePermissionurl ='<%=Url.Action("UpdateFuncDataPermission","Permission")%>'
        var SetFuncPermissionurl ='<%=Url.Action("SetFuncPermission","Permission")%>'
        var GetRelationFunctionListUrl = '<%=Url.Action("GetRelationFunctionList","Permission")%>';
        var dataruleurl = '<%=Url.Action("GetFuncDataRuleList","Permission")%>';
        var rolebaseurl = '<%=Url.Action("Role","Permission")%>' + "?type=RoleList";

        var GetRoleTree =  '<%=Url.Action("Role","Permission")%>' + "?type=GetRoleTree";

        var groupbaseurl = '<%=Url.Action("Group","Permission")%>' + "?type=GroupList";
        var modulebaseurl = '<%=Url.Action("GetModuleTreeContent","Permission")%>';
        var userbaseurl = '<%=Url.Action("GetAccountList","Account")%>';
        var DeleteRolesUrl= '<%=Url.Action("DeleteRoles","Permission")%>';
        var clonePermissionUrl= '<%=Url.Action("CloneFuncPermission","Permission")%>';
        function LoadModuleFunctionDataRule(){
            //读取授权的数据规则
             if (hiddenModuleId != null && hiddenFuncPermissionId != null) {
                //此处为加载设置时获取Owner，如刷新Module时Owner无选中，则会清除设置；如放到LoadOwner的Click获取，则设置会使用上一次的Owner
               $(".layout-panel-south").show();
                var lock = !hiddenFuncPermissionId ? true : false;
                    $('#tbModuleFunctionDataRule').datagrid({
                    nowrap: false,
                    striped: true,
                    fit: true,
                    border: false,
                    url: dataruleurl,
                    idField:'DataPermissionId',
                    queryParams: {FuncPermissionID : hiddenFuncPermissionId },
                    columns: [[
                             { field: 'IsSelected', title: '允许', width: GetWidth(0.04), align: 'left',
                                formatter: function (value, rec) {
                         
                                    return "<input type='radio' datavalue='" + rec.DataPermissionId + "' name='rd' " + (value ? "checked=checked" : "") + "\"/>";                                   
                                }
                             },
                            { field: 'Code', title: '功能代码', width: GetWidth(0.08), align: 'left' },
                            { field: 'Name', title: '数据权限名称', width: GetWidth(0.08), align: 'left' }
			        ]],
                    singleSelect: true,
                    pageSize: 100,
                    pageList: [100, 150, 200, 300],
                    rownumbers: true,
                    toolbar: [
                    {
                        text: permission.UserEnabled ? '保存' : '',
                        iconCls: permission.UserEnabled ? 'icon-save' : 'null',
                        id: 'btnSaveDataRule',
                        handler: function () {
                            SaveDataRule();
                        }
                    },'-',
                    {
                        text: '全消',
                        iconCls: 'icon-cut',
                        handler: function () {
                           $("input[type='radio']:checked").attr("checked", false);
                        }
                    }],
                    onBeforeLoad: function () {
                        RemoveForbidButton();
                    }
                });
            }
            else{
                    $(".layout-panel-south").hide();
            }
           
            if (lock) 
            {
                $("#btnSaveDataRule").parent().remove();
            }
        }
        function CreatCellInfo(value, denyEnabled) {
            if (value != null && value.length > 0) {
                var firstStr = value.split(';');

                if (denyEnabled == 'False') {
          
                    if (firstStr[0] == "True") {
                        return '<img src=<%=Url.Content("~/Scripts/EasyUI/themes/icons/ok.png")%> />';
                    }
                }
                else {
                    if (firstStr[0] == "True" && firstStr[1] == "False") {
                        return '<img src=<%=Url.Content("~/Scripts/EasyUI/themes/icons/ok.png")%> />';
                    }
           
                }
            }    
        };
        function LoadModuleGrid() {
           
            hiddenOwnerTitle = GetSelectedTabTitle();
            hiddenOwnerValue = GetSelectedGridKey();
           
            var lock = (hiddenOwnerValue == null || hiddenOwnerValue == "") || (!permission.UserEnabled && hiddenOwnerTitle == "用户") ||
                (hiddenOwnerTitle == "角色" && "" == '<%= "System" %>'
                );

            $('#tbModule').treegrid({
                nowrap: false,
                striped: true,
                fit: true,
                border: false,
                url: modulebaseurl,
                treeField: "ModuleName",
                idField: 'ModuleID',
                queryParams: { CodeOrName: $('#txtModuleCode').val(),OwnerCode: hiddenOwnerTitle, OwnerValue: hiddenOwnerValue },
                columns: [[
                            { field: 'ModuleName', title: '模块名称', width: 200, align: 'left' },
                            <%=ViewData["Columns"] %>
			        ]],
                singleSelect: true,
                pagination: true,
                pageSize: 200,
                pageList: [200, 250, 300, 350],
                rownumbers: true,
                onClickRow: function (data) {
                    if (hiddenModuleId != data.ModuleID.toString()) {             
                        hiddenModuleId = data.ModuleID.toString();                       
                        LoadModuleFunction();
                    }
                },
                onLoadSuccess: function(){
                     $('#tbModule').treegrid("collapseAll");
                     hiddenModuleId = -1;
                     LoadModuleFunction();
                }
            });
        }
        function LoadModuleFunction() {
            $(".layout-panel-south").hide();
            hiddenFuncPermissionId = null;
            if (hiddenModuleId != null) {
                //此处为加载设置时获取Owner，如刷新Module时Owner无选中，则会清除设置；如放到LoadOwner的Click获取，则设置会使用上一次的Owner
                hiddenOwnerTitle = GetSelectedTabTitle();
                hiddenOwnerValue = GetSelectedGridKey();
                var lock = (hiddenOwnerValue == null || hiddenOwnerValue == "") || (!permission.UserEnabled && hiddenOwnerTitle == "用户") ||
                    (hiddenOwnerTitle == "角色" && "" == '<%= "System" %>'
                    );

                $('#tbModuleFunction').datagrid({
                    nowrap: false,
                    striped: true,
                
                    border: false,
                    url: FuncPermissionList,
                    queryParams: { OwnerTitle: hiddenOwnerTitle, OwnerValue: hiddenOwnerValue, ModuleID: hiddenModuleId },
                    columns: [[
                            { field: 'FunctionCode', title: '功能代码', width: GetWidth(0.08), align: 'left' },
                            { field: 'FunctionName', title: '功能名称', width: GetWidth(0.08), align: 'left' },
                            { field: 'IsAllow', title: '允许', width: GetWidth(0.04), align: 'left',
                            formatter: function (value, rec) {
                                
                                    if (value != undefined) {
                                        //用户控制
                                        if(lock)
                                        {
                                            return (value == "True") ? '允许': ''  ;
                                        }
                                        else
                                        {
                                            var temp = "";
                                            var isSelf = "";

                                            if (value) {
                                                temp = " checked='checked' ";

                                                if (rec.IsSelf == "0") {
                                                    isSelf = " disabled='disabled' ";
                                                }
                                            }

                                            return "<input type='checkbox' " + temp + isSelf + " onclick=\"if(this.checked){GetRelationFunctionlist('"+ rec.FunctionID +"');}\"/>";
                                        }
                                    }
                                }
                            }
                            <%if(Clover.Config.CPM.PermissionConfig.Config.EnableDenyPermission ){%>
                                ,{ field: 'IsDeny', title: '拒绝', width: GetWidth(0.04), align: 'left',		
								  formatter: function (value, rec) {
									if (value != undefined) {
									if(lock){
										return (value == 'True') ? "<img src='../Images/iswake.png' alt='允许' />":'';
									}else{
										var temp = '';
										if (value) {
											temp = 'checked="checked"';
										}
										return '<input type="checkbox" ' + temp + '/>';
									}
                                   }
							     }
					            }
                            <%} %>
			        ]],
                    singleSelect: true,
                    pageSize: 50,
                    pageList: [50,100],
                    rownumbers: true,
                    toolbar: [
                    {
                        text: permission.UserEnabled ? '保存' : '',
                        iconCls: permission.UserEnabled ? 'icon-save' : 'null',
                        id: 'btnSave',
                        handler: function () {
                            Save();
                        }
                    },
                    '-',
                    {
                        text: '全选',
                        iconCls: 'icon-add',
                        handler: function () {
                            SetGridData("tbModuleFunction", "IsAllow", true);
                        }
                    },
                    '-',
                    {
                        text: '全消',
                        iconCls: 'icon-cut',
                        handler: function () {
                            SetGridData("tbModuleFunction", "IsAllow", false);
                            SetGridData("tbModuleFunction", "IsDeny", false);
                        }
                    }],
                    onBeforeLoad: function () {
                        RemoveForbidButton();
                    },
                    onClickRow: function (index, data) {
                        $("input[name='rd']:checked").attr("checked", false);
                        if (index > -1) {
                            hiddenFuncPermissionId = data.FuncPermissionID;
                            LoadModuleFunctionDataRule();
                        }
                    }
                });

                LoadModuleFunctionDataRule();
            }

            if (lock) 
            {
                $("#btnSave").parent().remove();
            }
        }
    </script>
</body>
</html>
