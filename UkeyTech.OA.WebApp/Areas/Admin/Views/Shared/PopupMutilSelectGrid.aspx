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
               
            $('#DataGrid').datagrid({
                nowrap: false,
                striped: true,
                fit: true,
                border: false,
                url: baseurl + "/GetPopupSelectDataList?t="+new Date(),
                queryParams: {
                    <%= string.IsNullOrEmpty(Request["Params"]) ? "" : Request["Params"] + "," %> 
                    selecttypeid: '<%=ViewData["SelectTypeId"] %>', 
                    where : $("#NameOrCode").val() ? ("<% var a = collist.Find(x => x.ColumnName.Trim().IndexOf("名称") >= 0);
                                                          if (a != null)
                                                          {%>[<%=a.ColumnName%>]<%}
                                                              else
                                                              {%><%=collist.Count > 0 ? (collist.Count > 1 ? collist[1].ColumnName : collist[0].ColumnName) : "名称" %><% }%> like '%" + $("#NameOrCode").val() + "%'"  + " OR Code like '%" + $("#NameOrCode").val() + "%'" ) : ""  ,
                                                              
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
                                   if (m.ColumnName == "ID" || m.ColumnName.IndexOf("@HIDE@") >= 0)
                                       continue;
                        %>                            
                        { field: '<%=m.ColumnName %>', title:'<%=m.Caption %>', width:  ('<%=m.Caption %>'.length) * 15 < 100 ? 130 : '<%=m.Caption %>'.length * 15 },
                        <%}
                           } %>
                        { field: 'ANC', title:'', width: 1,hidden:true}
                    ]],
                    <%if (Request["Mutil"] == null)
                      {%>
                singleSelect: true,
                onDblClickRow: function (idx, data) {
                    getValueAndQuit();
                },
            
                onLoadSuccess: function (data) {
                    if(isPress){
                        $('#DataGrid').datagrid('selectRow', 0);
                        isPress=false;
                    }
                    for(var i = 0; i < data.rows.length; i++){
                        if(find(data.rows[i].ID) >= 0){
                            $("#DataGrid").datagrid('selectRow', i);
                            $("#DataGrid").datagrid('checkRow', i);  
                        }
                    }
                },
                    <%} else{%>
                onSelect: function (idx, data) {
                   if(find(data.ID) < 0){
                        gobalkeyvalues.push({id:data.ID,name:findColumnValByName(data,"名称")});
                        setTextValue();
                   } 
                },
                onUnselect : function(idx ,data){
                   delById(data.ID);
                },
                  onLoadSuccess: function (data) {
                    $("div.datagrid-header-check").find("input").attr("checked", false); 
                    for(var i = 0; i < data.rows.length; i++){
                        if(find(data.rows[i].ID) >= 0){
                            $("#DataGrid").datagrid('checkRow', i); 
                        }
                    }
                    
                  },
                <%} %>
               
                pagination: true,
                pageSize: 100,
                pageList: [100, 150, 200, 300],
                rownumbers: true,
                pageNumber: 1
            });
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
                <%} %>
         <%if (!string.IsNullOrEmpty(Request["ID"]))
           { %>      
                theopener.returnValue = ids + ":" + names;               
                $(theopener.document).find('#<%=Request["ID"] %>').val(ids);
                var textbox = $(theopener.document).find('#<%=Request["Name"] %>');
                
               // $(textbox).text(names);
                $(textbox).val(names);
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
        function binCheckAllRows() {
              $("div.datagrid-header-check").find("input").click(function(){
                if($(this).is(":checked")){
                    var data = $("#DataGrid").datagrid("getData");
                    for(var i = 0; i < data.rows.length; i++){
                        if(find(data.rows[i].ID) < 0){
                            gobalkeyvalues.push({id:data.rows[i].ID,name:findColumnValByName(data.rows[i],"名称")});
                        }
                    }
                    setTextValue();
                }
                else{
                    var data = $("#DataGrid").datagrid("getData");
                    for(var i = 0; i < data.rows.length; i++){
                        delById(data.rows[i].ID);
                    }
                    setTextValue();

                }
            });
        }
        function clearvalue(){
            gobalkeyvalues.length = 0;
            $("#DataGrid").datagrid('unselectAll'); 
            $("#idvalue").val('')
            $("#text").val('');
        }
        $(function(){
            LoadSelectItems();
            initValue();
            binCheckAllRows();
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
        function Query() {
            LoadSelectItems();
            binCheckAllRows();
        }
    </script>
    <%} %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ld:Loading ID="Loading1" runat="server" />
    <div region="north" style="height: 40px">
        <div class="SearchDiv" style="width: auto !important;">
            代码或名称：<input type="text" id="NameOrCode" style="width: 200px;" onkeypress="EnterPress(event)" />
            <a id="searchIcon" href="javascript:void(0);" class="easyui-linkbutton" icon="icon-search"
                onclick="Query();">查询</a>
        </div>
    </div>
    <div id="center" region="center">
        <table id="DataGrid">
        </table>
    </div>
    <div region="south" style="height: 100px; text-align: center; padding: 5px;">
       
         
            
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
