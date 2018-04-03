<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminList.Master"
    Title="选择字段内容查看" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="UkeyTech.OA.WebApp.Extenstion" %>
<%@ Import Namespace="UkeyTech.OA.WebApp" %>
<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <style type="text/css">
        body{overflow:hidden !important;}
    </style>
    <%{
          var collist = (List<UkeyTech.WebFW.DAO.SYSDataColumn>)ViewData["Columns"]; %>
    <script type="text/javascript">
        var baseurl = '<%=Url.Action("","Utility")%>';
        var returnValue = true;
        
        function LoadSelectItems() {
               
            $('#DataGrid').treegrid({
                nowrap: false,
                striped: true,
                fit: true,
                border: false,
                url: baseurl + "/GetPopupTreeDataList?selecttypeid=<%=ViewData["SelectTypeId"] %><%= string.IsNullOrEmpty(Request["Params"]) ? "":("&" +Request["Params"].Replace(":","=").Replace("'","").Replace(",","&"))%> ",
                treeField: "<%{var b = collist.Find(x => x.ColumnName.Trim().IndexOf("名称") >= 0);if (b != null){%><%=b.ColumnName%>]}<%}else{%>名称<%}} %>",
                idField: 'ID',
                queryParams: {
                    
                    
                    ModuleCode : "<%=Request["ModuleCode"] %>",
                    FunctionCode : "<%=Request["FunctionCode"] %>",
                    Sort : "<%=Request["sort"] %>"
                },
                columns: [[
                        <%if (Request["Mutil"] != null)
                          {%>
                        { field: 'ck', checkbox: true },
                        <%} %>
                        
                        <% if (ViewData["ExtendColumns"] != null)
                           {%>
                        <%=ViewData["ExtendColumns"] %>,
                        <%}
                           else
                           {
                               foreach (var m in collist)
                               {
                                   if (m.ColumnName == "ID" || m.ColumnName.IndexOf("@HIDE@") >= 0|| m.ColumnName.IndexOf("_parentId") >= 0)
                                       continue;
                        %>                            
                        { field: '<%=m.ColumnName %>', title:'<%=m.Caption %>', width:  ('<%=m.Caption %>'.length) * 15 < 100 ? 130 : '<%=m.Caption %>'.length * 15 },
                        <%}
                           } %>
                        { field: 'ANC', title:'', width: 1}
                    ]],
                    <%if (Request["Mutil"] == null)
                      {%>
                singleSelect: true,
                onDblClickRow: function (data) {
                    if(checkIsParentNode(data))
                        return false;
                    getValueAndQuit();
                },
                onDblClickCell:function(index,field,value){
                    //双击展开
                    if(index.indexOf('名称')>=0){
                        $('#DataGrid').treegrid('expand', field.ID);
                    }
                },
                onSelect: function (data) {
                   if(checkIsParentNode(data))
                        return false;
                },
                onLoadSuccess: function (row, data) {
                    if(isPress){
                        $('#DataGrid').datagrid('selectRow', 0);
                        isPress=false;
                    }
                    if(data){
                        for(var i = 0; i < data.length; i++){
                            if(find(data[i].ID) >= 0){
                                $("#DataGrid").treegrid('select', data[i].ID);
                            }
                        }
                    }
                },
                    <%} else{%>
                    singleSelect: false,
                onSelect: function (data) {
                   if(checkIsParentNode(data))
                        return false;
                   if(find(data.ID) < 0){
                        gobalkeyvalues.push({id:data.ID,name:findColumnValByName(data,"名称")});
                        setTextValue();
                   } 
                },
                onUnselect : function(data){
                   delById(data.ID);
                },
                onLoadSuccess: function (row, data) {
                    $("div.datagrid-header-check").find("input").attr("checked", false); 
                    for(var i = 0; i < data.length; i++){
                        if(find(data[i].ID) >= 0){
                           $("#DataGrid").treegrid('select', data[i].ID);
                        }
                    }
                },
                <%} %>
               
            
                rownumbers: false
                
            });
        }
        function checkIsParentNode(data){
            var haschild = false;
            haschild = typeof(data.children) != "undefined" && data.children && data.children.length > 0;
            <%if (Request["SelectAll"] == null)//设置是否可以选择父节点；SelectAll=false时，不能选择父节点；
              {%>

            //如果是父节点,取消选择
            if(data.state == "closed" || haschild){
                setTimeout(function(){
                    $('#DataGrid').treegrid('unselect', data.ID);
                    $("tr[node-id='" + data.ID + "']").find("input:checked").attr("checked", false);
                },200);
                return true;
            }
            <%}%>
            return false;
        }
        var gobalkeyvalues = [];
        function findColumnValByName(row,text){
            for(var col in row){
                if(col.indexOf(text)>=0){
                    return row[col];
                }
            }
            return "";
        }
        function delById(id){
            for(var i =0; i < gobalkeyvalues.length; i++){
                if(gobalkeyvalues[i].id ==id){
                   gobalkeyvalues.splice(i,1);
                   if(gobalkeyvalues.length > 0)
                    setTextValue();
                   else
                    clearvalue();
                   break;
                }
            }
        }
        function find(id){
            for(var i =0; i < gobalkeyvalues.length; i++){
                if(gobalkeyvalues[i].id ==id){
                    return i;
                }
            }
            return -1;
        }
        function findByName(name){
            for(var i =0; i < gobalkeyvalues.length; i++){
                if(gobalkeyvalues[i].name ==name){
                    return i;
                }
            }
            return -1;
        }
        
        function setTextValue(){
            var ids = '';
            var names = '';
        
          
            for(var i =0; i < gobalkeyvalues.length; i++){
              
                    ids  += gobalkeyvalues[i].id;
                    names += gobalkeyvalues[i].name;

                    if((i+1) < gobalkeyvalues.length)
                    {
                        ids += ',';
                        names += ',';
                    }
                
            }
           $("#text").val(names);
           $("#idvalue").val(ids);
        }
     
       function addValues(ids, names){
            if(ids=="" || ids === undefined){
                ids = getGridSelections("DataGrid", "ID");
                names = getGridSelections("DataGrid", "名称");
            }

               if(ids=="" || ids === undefined){
               return;
               }
            var idsary = ids.split(',');
            var namesary = names.split(',');
            var idx = -1;
            for(var i =0; i < idsary.length; i++){
                idx = find(idsary[i]);
                idx = -1;
                if(idx >= 0){
                    continue;
                }else{
                    gobalkeyvalues.push({id:idsary[i],name:namesary[i]});
                }
            }
            setTextValue();
        }
      
        function getOpener() {
            var theopener;
            if (window.dialogArguments != null) {
                theopener = window.dialogArguments;
            }else if (window.opener != null) {
                theopener = window.opener;
            } else {
                theopener = window.parent;
            }
            return theopener;
        }
        function getValueAndQuit() {
            var theopener = getOpener();
         <%if (Request["Mutil"] != null)
           {%>
                var ids = $("#idvalue").val();
                var names = $("#text").val();
        <%}
           else
           {%>
              
                var ids = getGridSelections("DataGrid", "ID");
                var names = getGridSelections("DataGrid", "名称");
                if(ids.length == 0)
                {
                    alert('请选择一行再点击确定');
                    return;
                }
                <%} %>
         <%if (!string.IsNullOrEmpty(Request["ID"]))
           { %>      
                theopener.returnValue = ids + ":" + names;               
                $(theopener.document).find('#<%=Request["ID"] %>').val(ids);
         $(theopener.document).find('#<%=Request["Name"] %>').val(names);
                <%} %>

                //valid the selection
        <%{
              var validselection = string.IsNullOrEmpty(Request["validselection"]) ? "" : Request["validselection"];
              if (!string.IsNullOrEmpty(validselection))
              {%>
                if(theopener.<%=validselection %> != "undefined" && !theopener.<%=validselection %>(ids, names, getGridSelectRows("DataGrid"))){
                    return;
                }
                <%}
          } %>
		
                //call back
        <%{
              var callback = string.IsNullOrEmpty(Request["callback"]) ? "popupCallback" : Request["callback"]; %>
                if(theopener.<%=callback %>){
                    theopener.<%=callback %>(ids, names, getGridSelectRows("DataGrid"));
         }
        <%} %>
                CloseTheWin();
            }
            function initValue(){
                var theopener = getOpener();
      
         <%if (!string.IsNullOrEmpty(Request["Name"]))
           { %>
         var names = $(theopener.document).find('#<%=Request["Name"] %>').val();
         $("#text").val(names);
        <%} %>

      <%if (!string.IsNullOrEmpty(Request["ID"]))
          { %>
         var ids = $(theopener.document).find('#<%=Request["ID"] %>').val();
         $("#idvalue").val(ids)
      
         <%} %>

         addValues( ids,names);

     }
        function clearvalue(){
            gobalkeyvalues.length = 0;
            $("#DataGrid").datagrid('unselectAll'); 
            $("#idvalue").val('')
            $("#text").val('');
        }
        function JumpToItems(){
            
        }

        function loopSetTreeItemSelected(data, selected){
            for(var i = 0; i < data.length; i++){
                if(checkIsParentNode(data[i] || data[i].state == 'closed'))
                {
                    $("#DataGrid").treegrid("unselect", data[i].ID);
                    if(data[i].children) 
                    loopSetTreeItemSelected(data[i].children, selected);
                }
                else{
                  if(selected && find(data[i].ID) < 0){
                    gobalkeyvalues.push({id:data[i].ID,name:findColumnValByName(data[i],"名称")});
                  }else if(!selected){
                    $("#DataGrid").treegrid("unselect", data[i].ID); 
                    delById(data[i].ID);
                  }
                }
            }
        }
        $(function(){
            LoadSelectItems();
            initValue();
            $("#text").keyup({
            
            });

             $("div.datagrid-header-check").find("input").click(function(){
                if($(this).is(":checked")){
                   var data = $("#DataGrid").treegrid("getData");
                   loopSetTreeItemSelected(data, true);
                }
                else{
                   loopSetTreeItemSelected(data, false);
                }
                  setTextValue();
            });
        });
        var isPress=false;
        function EnterPress(e) { //传入 event
            var e = e || window.event || arguments.callee.caller.arguments[0];
            if (e.keyCode == 13) {
                if($('#NameOrCode').val()!=""){    
                    document.getElementById("searchIcon").click();
                    isPress=true;
                }
            }
        }
    </script>
    <%} %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ld:Loading ID="Loading1" runat="server" />
    <div region="north" style="height: 0px;display:none;">
        <div class="SearchDiv" style="width: auto !important;">
            代码或名称：<input type="text" id="NameOrCode" style="width: 80px;" onkeypress="EnterPress(event)" />
            <a id="searchIcon" href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search" onclick="JumpToItems();">查询</a>
        </div>
    </div>
    <div id="center" region="center">
        <table id="DataGrid">
        </table>
    </div>
    <div region="south" style="height: 90px; text-align: center; padding: 5px;">
       
         <%if (Request["Mutil"] != null)
          {%>
        <div>
            <textarea id="text" readonly="readonly" style="height: 40px; width: 99%"></textarea>
            <input type="hidden" id="idvalue" />
        </div>
        <%} %>
        
        <a class="easyui-linkbutton" icon="icon-ok" href="javascript:void(0);" onclick="getValueAndQuit();"
            id="A1">确定</a>     
             <%if (Request["Mutil"] != null)
               {%>
                    <a class="easyui-linkbutton" icon="icon-remove" href="javascript:void(0);"
                onclick="clearvalue();" id="A3">清空</a>
          <%} %>
             
       <a class="easyui-linkbutton" icon="icon-cancel" href="javascript:void(0)"
                onclick="CloseTheWin();" runat="server" id="btnCancel">取消</a>
    </div>
    <script type="text/javascript" language="javascript">
        var permission = <%=UkeyTech.OA.WebApp.Helper.GetPermissionJson(
        "CustomForm","View")%>
    </script>
</asp:Content>
