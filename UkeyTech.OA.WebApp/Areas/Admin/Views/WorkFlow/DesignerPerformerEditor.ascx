<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" Debug="false" EnableViewState="false"%>
<div id="PerformerEditor">
    <div class="ym-form linearize-form ym-columnar">
        <div class="ym-form-fields">
            <div class="ym-fbox-text ym-hideall">
                <label for="PerformerName">
                    标示：<sup class="ym-required">*</sup></label>
                <input id='PerformerName' name='PerformerName' />
            </div>
            <div class="ym-fbox-text">
                <label for="PerformerDisplayName">
                    显示名称：<sup class="ym-required">*</sup></label>
                <input id='PerformerDisplayName' name='PerformerDisplayName' />
            </div>
            <div id="dvHandler" class="ym-fbox-text">
                <label for="AssignmentHandler">
                    指派策略：<sup class="ym-required">*</sup></label>
                <input id='AssignmentHandler' name='AssignmentHandler' />
            </div>
            <div class="ym-fbox-text">
                <label for="AssignmentType">
                    指派方式：<sup class="ym-required">*</sup></label>
                <select id="AssignmentType" name="AssignmentType">
                    <option value='Handler'>通过分配策略实现</option>
                    <option value='Current'>通过默认获取当前创建者</option>
                    <option value='ProcessCreator'>通过流程创建人指派</option>
                    <option value='Superiors'>通过默认获取创建者的上级领导用户</option>
                    <option value='CreatorGroupPosition'>通过创建人所在的部门指派特定岗位</option>
                    <option value='GroupPosition'>通过指定的部门岗位指派</option>
                    <option value='ByProcessVar'>通过流程流程变量指定</option>
                    <option value='CurrentUserGroupPosition'>通过上一个步骤处理人的默认部门指派特定岗位</option>
                    <option value='LastStepUserGroupPosition'>通过上个关联步骤处理人的部门指派特定岗位</option>
                </select>
            </div>
            <div class="ym-fbox-text">
                <label for="PerformerValue">
                    指派目标：<sup class="ym-required">*</sup></label>
                <input id='PerformerValue' name='PerformerValue' />
                <input id="PopupPerformerValue" type="button" class="popup" onclick="ShowUserDialog();" value="......" />
            </div>
            <div class="ym-fbox-text">
                <label for="PerformerDescription">
                    描述：</label>
                <input id='PerformerDescription' name='PerformerDescription' />
            </div>
        </div>
        <div class="ym-fbox-button">
            <a id="savePerformer" href='javascript:void(0);' class="easyui-linkbutton" icon="icon-ok">
                保存</a> <a id="cancelPerformer" href='javascript:void(0);' class="easyui-linkbutton"
                    icon="icon-cancel">取消</a>
        </div>
    </div>
</div>