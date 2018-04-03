<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" Debug="false" EnableViewState="false" %>

 <div id="FromEditor">
    <div class="ym-form linearize-form ym-columnar">
        <div class="ym-form-fields">
            <div class="ym-fbox-text ym-hideall">
                <label for="FormName">
                    表单标记：<sup class="ym-required">*</sup></label>
                <input id='FormName' name='FormName' readonly="readonly" />
            </div>
            <div class="ym-fbox-text">
                <label for="FormDisplayName">
                    表单显示名称：<sup class="ym-required">*</sup></label>
                <input id='FormDisplayName' name='FormDisplayName' />
            </div>
            
             <div class="ym-fbox-text">
                <label for="FormUri">
                    参考表单类型：</label>
                <span class="ym-fbox-check clearCss">
                      <select id="formlist">                            
                    </select>
                </span>
            </div>
            <div class="ym-fbox-text">
                <label for="FormUri">
                    表单地址：<sup class="ym-required">*</sup></label>
                <span class="ym-fbox-check clearCss">
                    <input id='FormUri' name='FormUri' style="width:310px !important"/>
                  
                </span>
            </div>
            <div class="ym-fbox-text">
                <label for="FormUri">
                    UI加载的脚本：</label>
                <textarea id='FormUIScript' name='FormUIScript' rows=3></textarea>
            </div>
            <div class="ym-fbox-text">     
                 <label>
                    控件控制设置：</label>              
                <span class="ym-fbox-check clearCss">
                    <table id="dg" style="width:320px;height:220px;" toolbar="#toolbar" fitColumns="true" singleSelect="true">
		                <thead>
			                    
		                </thead>
	                </table>
                    <input type=hidden id="FormUIControl" />
	                <div id="toolbar">
		                <a href="#" class="easyui-linkbutton" iconCls="icon-add" plain="true" onclick="javascript:$('#dg').edatagrid('addRow')">新建</a>
		                <a href="#" class="easyui-linkbutton" iconCls="icon-remove" plain="true" onclick="javascript:$('#dg').edatagrid('destroyRow')">删除</a>
		                <a href="#" class="easyui-linkbutton" iconCls="icon-save" plain="true" onclick="javascript:$('#dg').edatagrid('saveRow')">保存</a>
		                <a href="#" class="easyui-linkbutton" iconCls="icon-undo" plain="true" onclick="javascript:$('#dg').edatagrid('cancelRow')">取消</a>
                        <a href="#" class="easyui-linkbutton" iconCls="icon-search" plain="true" onclick="javascript:popupForm()">加载</a>
	                </div>                        
                </span>
            </div>
            <div class="ym-fbox-text">
                <label for="FormDescription">
                    表单描述：</label>
                <input id='FormDescription' name='FormDescription' />
            </div>
        </div>
        <div class="ym-fbox-button">
            <a id="saveForm" href='javascript:void(0);' class="easyui-linkbutton" icon="icon-ok">
                保存</a> <a id="cancelForm" href='javascript:void(0);' class="easyui-linkbutton" icon="icon-cancel">
                    取消</a>
        </div>
    </div>
</div>
