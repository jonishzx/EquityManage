<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<Clover.Message.Model.BoxMessage>>" %>
<%if(Model != null) {%>
<div id="MessageStatusLog">
        
	<table id="tt" style="width:700px;" class="panel-body">
		<thead>
			<tr class="datagrid-header" style="height:29px;">
				<td width="80px"><div class="datagrid-cell">收件人</div></td>
                <td width="160"><div class="datagrid-cell">送达时间</div></td>
                <td width="160"><div class="datagrid-cell">阅读时间</div></td>
                <td width="120"><div class="datagrid-cell">阅读批注</div></td>
                <td width="120"><div class="datagrid-cell">处理时间</div></td>
                <td width="160"><div class="datagrid-cell">处理批注</div></td>
                <td width="80"><div class="datagrid-cell">状态</div></td>
			</tr>
		</thead>
	    <tbody class="datagrid-body">
        
        <%   
            foreach (var m in Model)
            {
        %>
        <tr class="datagrid-row" style="height:24px;">
            <td><div class="datagrid-cell" style="height:24px;"><%=m.ReceiverName%></div></td>
            <td><div class="datagrid-cell" style="height:24px;"><%=m.ReceiveTime%></div></td>
            <td><div class="datagrid-cell" style="height:24px;"><%=m.ReadTime%></div></td>
            <td><div class="datagrid-cell" style="height:24px;"><%=m.ReadComment%></div></td>
            <td><div class="datagrid-cell" style="height:24px;"><%=m.OpTime%></div></td>
            <td><div class="datagrid-cell" style="height:24px;"><%=m.OpComment%></div></td>
            <td><div class="datagrid-cell" style="height:24px;"><%=m.StatusName%></div></td>
            </tr>
        <%}%>
        
        </tbody>
   </table>
</div>
<%} %>
