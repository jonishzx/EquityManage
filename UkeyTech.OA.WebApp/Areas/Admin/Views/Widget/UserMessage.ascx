<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UkeyTech.WebFW.Model.Widget>" %>
<%@ Import Namespace="UkeyTech.OA.WebApp.Areas.Admin.Controllers" %>
<%@ Import Namespace=" StructureMap" %>
<%@ Import Namespace=" Clover.Web.Core" %>
<%@ Import Namespace=" Clover.Message.DAO" %>
<%@ Import Namespace=" Clover.Message.Model" %>
<script type="text/javascript">
    $(function () {
        messageBeforeEvent.push(beforeLoadMessage);
        messageAfterEvent.push(afterLoadMessage);
    });
    function initMessageList() {
        var usermessagelist = $(".usermessageList").find(".messagewrap");
        $(usermessagelist).hover(
            function () { $(this).addClass("x-view-over"); },
            function () { $(this).removeClass("x-view-over"); })
            .click(function () {
                SetWinWithMaxSize("查看消息", "/Admin/Message/ViewMessage/" + $(this).attr("messageid"));
            });
        $(usermessagelist).each(function () {
            if ($(this).attr("hasRead") && $(this).attr("hasRead") != '0001-01-01T00:00:00') {
                $(this).addClass("readed");
            }
        });
    }
    function beforeLoadMessage() {
        $("#x-message-content").html("读取中,请稍候...");
    }
    function afterLoadMessage(data) {
        var o = { groups: data };
        $('#x-message-content').html("");

        // compile the template
        var messageTemplate = dust.compile($("#x-message-template").html(), "tpl");

        // load the compiled template into the dust template cache
        dust.loadSource(messageTemplate);

        // create a function that takes the data object
        // in this case it's a 'building' object
        var template = function (groups) {
            var result;
            dust.render("tpl", groups, function (err, res) {
                result = res;
            });
            return result;
        };

        $("#x-message-content").html(template(o));

        initMessageList();
    }
    function reloadMessage() {
        showMessageAlert();
    }
</script>
<script type="text/x-template" id="x-message-template">
   {#groups}
   <div class="usermessageList common-ct">
        <tpl for=".">
            <div><a name="{Title}"></a><h2><div>{Title}</div></h2>
            <dl><tpl for="samples">
                {#Items}
                <div class="messagewrap" messageid="{MessageId}" hasRead="{ReadTime}">                       
                    <div><H4><b>{SenderName}</b><i>{ReceiveTime}</i></H4>
                        <H5>{Title}</H5>
                    </div>
                </div>
                {/Items}
            </tpl>
        <div style="clear:left"></div>
        </dl>
        </div>
        </tpl>
    </div>
    {/groups}
</script>
<div>
    <a href="javascript:void(0);" onclick="reloadMessage()"><span class="icon icon-refresh"
        style="float: left;"></span>刷新</a>
</div>
<div id="x-message-content">
</div>
