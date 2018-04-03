<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/AdminEdit.Master" Inherits="System.Web.Mvc.ViewPage<Object>" %>
<%@ Register Src="../Shared/PopupWin.ascx" TagName="PopupWin" TagPrefix="uc2" %>
<%@ Import Namespace="UkeyTech.OA.WebApp" %>
<%@ Import Namespace="UkeyTech.OA.WebApp.Extenstion" %>
<%@ Register src="../Widget/AddWorkItemList.ascx" tagname="AddWorkItemList" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ViewPage1
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.edatagrid.js")%>"></script>
   <script src="../../../../Scripts/jscripts/tiny_mce/tiny_mce.js" type="text/javascript"></script>
   <script type="text/javascript">
       $(function () {
           $("#Items").edatagrid({
               height: "150",
               width: "100%",
               columns: [
                    [{ field: 'ItemName', title: '品名', width: 70, align: 'center', editor: { type: 'text'} },
                        { field: 'RecBrand', title: '建议品牌', width: 70, align: 'center', editor: { type: 'text'} },
                        { field: 'Spec', title: '规格', width: 70, align: 'center', editor: { type: 'text'} },
                        { field: 'Qty', title: '数量', width: 70, align: 'right', editor: { type: 'numberbox'} },
                        { field: 'Unit', title: '单位', width: 70, align: 'center', editor: { type: 'text'} },
                        { field: 'FCPrice', title: '预计单价', width: 70, align: 'right', editor: { type: 'numberbox'} },
                        { field: 'FCAmount', title: '预计金额', width: 70, align: 'right', editor: { type: 'numberbox'} },
                        { field: 'IsRes', title: '是否固定资产', width: 120, align: 'center', editor: { type: 'checkbox', options: { on: '是', off: '否'}} }
                    ]]
           });

           tinyMCE.init({
               // General options
               mode: "textareas",
               theme: "advanced",
               plugins: "style,layer,table,advhr,advimage,advlink,inlinepopups,insertdatetime,preview,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,advlist",
               language: "zh",
               width: "350",
               height: "300",
               // Theme options
               theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect,fontselect,fontsizeselect",
               theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
               theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
               theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,pagebreak,restoredraft",

               theme_advanced_toolbar_location: "top",
               theme_advanced_toolbar_align: "left",
               theme_advanced_statusbar_location: "bottom",
               theme_advanced_resizing: true,

               content_css: "css/content.css",
               setup: function (ed) {
                   if ($('#' + ed.id).prop('readonly')) {
                       ed.settings.readonly = true;
                   }
               }
           });

           setreadonly("#aselector");
           ShowError("12312312","123123123123123213123123");

       });
   </script>
    <form id="form1" runat="server">
   
<div region="center" id="aselector">
    <%{ 
          var dict = Model as Dictionary<string,string>;
          foreach(var key in dict.Keys){
        %>
        <div><%=key %>:<%=dict[key] %></div>
      <% }} %>
     <table id="Items" toolbar="#datafieldstoolbar" fitColumns="true" singleSelect="true">
                        </table>       
                        <div id="datafieldstoolbar">
		                    <a href="#" class="easyui-linkbutton" iconCls="icon-add" plain="true" onclick="javascript:$('#Items').edatagrid('addRow')">新建</a>
		                    <a href="#" class="easyui-linkbutton" iconCls="icon-remove" plain="true" onclick="javascript:$('#Items').edatagrid('destroyRow')">删除</a>
		                    <a href="#" class="easyui-linkbutton" iconCls="icon-save" plain="true" onclick="javascript:$('#Items').edatagrid('saveRow')">保存</a>
		                    <a href="#" class="easyui-linkbutton" iconCls="icon-undo" plain="true" onclick="javascript:$('#Items').edatagrid('cancelRow')">取消</a>
	                    </div>
    <h2>ViewPage1</h2>
    字典下拉列表
      <%Html.RenderPartial(Helper.DictDropDownListPath,
                                    new ViewDataDictionary(new
                                    {
                                        ID = "Sex",
                                        DictID = "sex",
                                        AddEmptyItem = true,
                                        Value = "",
                                        Enabled = false
                                    }));%>
 字典单选列表
      <%Html.RenderPartial(Helper.DictRadioButtonListPath,
                                    new ViewDataDictionary(new
                                    {
                                        ID = "Sex",
                                        DictID = "sex",
                                        Default = "A",
                                        Value = DateTime.Now.Second % 2 == 0 ? null : "B"
                                    }));%>
  

组织树
                                      <%Html.RenderPartial(Helper.DictComboTreePath,
                                    new ViewDataDictionary(new
                                    {
                                        ID = "TestGroupTree",
                                        Value = ""
                                    }));%>

    多选
        <%
           

            Html.RenderPartial(Helper.PopupControlPath,
           new ViewDataDictionary(new
           {
               IDControlName = "ID2",
               TextControlName = "Name2",
               DictID = "BaseAccountTitle",
               //TextValue = "222",
               Value = "",
               TreeView = true,
               MutilSelect = true,
               Enabled = true
           }));




        //  Html.RenderPartial(Helper.PopupControlPath,
        // new ViewDataDictionary(new
        // {
        //     IDControlName = "ID2",
        //     TextControlName = "Name2",
        //     DictID = "AllUser",
        //     //TextValue = "222",
        //     Value = "100.10",

        //     MutilSelect = true,
        //     Enabled = true
        // }));
                                    %>

    <%using (Html.BeginForm())
      { %>
    <input name="MasterA" type=text value=""/>
    <%
          var areacode = "";
          var areaName = "";
          var sex = 0;
          var birthDate = DateTime.Now;
          var age = 0;
          UkeyTech.OA.WebApp.Extenstion.IdentityCard.GetInfoByIdentityCard("44060219801220005X", out areacode, out areaName, out sex, out birthDate, out age);%>
        <%=areaName%>
        <%=sex%>
        <%=birthDate%>
        <%=areacode%>
        <%=age%>
    <input type=submit />
    
   
    <%} %>

    <% using (Ajax.BeginForm("AjaxActionTest", "Test", new AjaxOptions { UpdateTargetId = "mydiv", OnSuccess = "OnSuccess", OnFailure = "OnFailure" }))
       { %>
 
        <input name="input1" id="input1" type="text" /> <input name="input2" id="input2" type="text" />
 
        <input id="dosomething" type="submit" value="save" />
 
        <span>The Answer is: <%= TempData["Answer"]%></span>

   
 
        <% } %>

             result:<span id="mydiv"></span>
             
             id:<input type="text" id="id" />
text:<input type="text" id="text" />
<input type="button" value="popup" onclick="pop()"/>
    <uc1:AddWorkItemList ID="AddWorkItemList1" runat="server" Visible=false />
    <textarea id="bbq">
    hello world!
    <br />
    aasdsd
    </textarea>
</div>
   
    </form>
   
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Script" runat="server">
    
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/MicrosoftAjax.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/MicrosoftMvcAjax.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/MicrosoftMvcValidation.js")%>"></script>
    <script type="text/javascript">
        function validtrue() {
            return true;
        }
        function validfalse() {
            return false;
        }
        function OnSuccess(response) {
            alert(response);
        }

        function OnFailure(response) {
            alert("Whoops! That didn't go so well did it?");
        }
        function pop() {
            popupAndSelectItems(commonpopupurl, '', $("#id"), $("#text"));
        }
        document.write(fmtDecimal(1212.1212,","));
</script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="FootBlock" runat="server">
     <uc2:PopupWin ID="PopupWin1" runat="server" />
</asp:Content>
