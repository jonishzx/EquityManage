<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UkeyTech.WebFW.Model.OPLog>" %>
<div class="editpage" region="center">
    <div align="center">
        <div class="ym-form linearize-form ym-columnar zcolumn">
            <div class="ym-form-fields">
                <div class="ym-fbox-text">
                    <label class="ym-fbox-text-label w120 imgfront">
                        操作员代码：</label>
                    <div class="ym-fbox-text" style="text-align: left">
                        <span class="ym-fbox-text-text" >
                            <%=Model.LoginName%></span>
                        <div class="form-clear-left">
                        </div>
                    </div>
                    <div class="ym-fbox-text">
                        <label class="ym-fbox-text-label w120 imgfront">
                            操作员名称：</label>
                        <div class="ym-fbox-text" style="text-align: left">
                            <span class="ym-fbox-text-text">
                                <%=Model.UserName%></span>
                            <div class="form-clear-left">
                            </div>
                        </div>
                    </div>
                    <div class="ym-fbox-text ">
                        <label class="ym-fbox-text-label w120 imgfront">
                            访问IP：</label>
                        <div class="ym-fbox-text" style="text-align: left">
                            <span class="ym-fbox-text-text">
                                <%=Model.UserIP%></span>
                            <div class="form-clear-left">
                            </div>
                        </div>
                    </div>
                    <div class="ym-fbox-text ">
                        <label class="ym-fbox-text-label w120 imgfront">
                            执行操作：</label>
                        <div class="ym-fbox-text" style="text-align: left">
                            <span class="ym-fbox-text-text">
                                <%=Model.LogOPName%></span>
                            <div class="form-clear-left">
                            </div>
                        </div>
                    </div>
                    <div class="ym-fbox-text ">
                        <label class="ym-fbox-text-label w120 required imgfront">
                            操作内容：</label>
                        <div class="ym-fbox-text">
                            <textarea name="Descn" class="textarea w300" readonly="readonly" style="height: 200px;"><%=Model.LogMessage%></textarea>
                            <div class="form-clear-left">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
