<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage" ValidateRequest="false" %>

<%@ Register Src="../Shared/Loading.ascx" TagName="Loading" TagPrefix="ld" %>
<%@ Register Src="../Shared/ScriptBlock.ascx" TagName="ScriptBlock" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=Clover.Config.WebSiteConfig.Config.WebAppName%>
    </title>
    <script type="text/javascript">
        var rooturl = '<%= Clover.Web.Core.Utility.ConvertAbsoulteUrl("~/") %>'
    </script>
  
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/WorkFlow/lib/raphael-min.js")%>"></script>
    <%:Styles.Render("~/styles/theme/" + UkeyTech.OA.WebApp.Helper.GetSkin())%>
    <script type="text/javascript">
        if (typeof (Object) === "undefined") {
            window.location.reload();
        }
        var rooturl = '<%= Clover.Web.Core.Utility.ConvertAbsoulteUrl("~/") %>'
    </script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.7.2.min.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/WorkFlow/myflow.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/ModuleJs/WorkFlow/myflow.fpdl.js")%>"></script>
    <script type="text/javascript" src="<%=Url.Content("~/Scripts/EasyUI/jquery.easyui.min.js")%>"></script>
    <script type="text/javascript">     
        var jsondata = <%=ViewData["ProcessContent"]%>;
        var activeRects = <%=ViewData["ActiveRects"]%>;
        var historyRects = <%=ViewData["HistoryRects"]%>;
        var baseurl = '<%=Url.Action("","WorkFlow")%>';
        var currid = '<%=ViewData["id"] %>';
        var gtop = 0,gleft = 0;

        function getStausName(state){
            switch(state){
                case 0 :
                    return '<span class="initialized">' + '初始化中<span>' + "</span>";
                case 1 :
                    return '<span class="running">' + '运行中' + "</span>";
                case 7 :
                    return '<span class="completed">' + '审批通过' + "</span>";
                case 9 :
                    return '<span class="canceled">' + '被撤销' + "</span>";

            }
        }
        //override
        var myflow = $.myflow;
        var oldattr;
        var title;
        $.extend(true, myflow.config.rect, {          
            mouseover:function(uiobject, target){
                if(target.type != 'text' && target.type != 'image'){
                    oldattr = target.attr()
                    target.attr({stroke: "#9966FF","stroke-width": 2 ,"stroke-linecap":"square"});             
                }
                showToolTip(uiobject);
                $('#dialog').parent().css("top", gtop+10);
                $('#dialog').parent().css("left", gleft+10);
                if(uiobject.text.text)
                    $(title).html(uiobject.text.text + "-工作明细项");
                else
                    $(title).html(uiobject.text.text + "工作明细项");
            },
            mouseout:function(uiobject, target){
                if(target.type != 'text' && target.type != 'image'){
                    target.attr(oldattr);
                }
            }          
        });

        $.extend(true, myflow.config.path, {
            text : {  text: "TO {to}",
                cursor: "pointer",
                background: "#000"
            },
            mouseover:function(uiobject, target){
                oldattr = target.attr();
                var tip = uiobject.props.DisplayName ? uiobject.props.DisplayName.value : ""
                    tip += (uiobject.props.Condition && uiobject.props.Condition.value) ? "(生效条件:" + uiobject.props.Condition.value + ")" : "" ;
            
                target.attr({title:tip});  
                
            },
            mouseout:function(uiobject, target){
                target.attr(oldattr);
               
            }   
        });

        $.extend(true, myflow.config.activeRects, {
            rects: [],
                rectAttr: {
                    stroke: "#ff0000",
                    "stroke-width": 2,                    
                    cursor:"pointer"
                }
        });

        $.extend(true, myflow.config.historyRects, {
             rects: [],
                rectAttr: {
                    stroke: "#C0C0C0",
                    "stroke-width": 2,
                    fill: "#E5E5E5",
                    "fill-opacity": 0.6,
                    "opacity":0.4,
                    cursor:"pointer"
                },
                pathAttr: {
                    path: {
                        stroke: "#C0C0C0"
                    },
                    arrow: {
                        stroke: "#C0C0C0",
                        fill: "#C0C0C0"
                    }
                }
        });

        //tooltip
        var cache = {};
        var template = "<div><label>工作任务</label>{6}</div>" 
                      + "<div><label>领取人</label>{0}</div>"
                      + "<div><label>完成人</label>{7}</div>"
                      + "<div><label>状态</label>{1}</div>"
                      + "<div><label>开始时间</label>{2}</div>"
                      + "<div><label>签收时间</label>{3}</div>"
                      + "<div><label>结束时间</label>{4}</div>"
                      + "<div><label>说明</label>{5}</div>"

        var endtemplate =  "<div>结束</div>" 
                      + "<div><label>结束时间</label>{0}</div>"
       
       function DateHandler(value) {
            if (value != undefined)
                return value.substring(0, 19).replace('T', '&nbsp;');
            else
                return "";
        }              
        function loadWorkItems(json){
          
            if(json.total > 0){
                var output = "";
                for (var i in json.rows) {
                    
                    output += "<pre class='searchbox'>"
                    if(json.rows[i].State != null){
                        output += template.replace("{0}", json.rows[i].ActorName)
                        .replace("{1}", getStausName(json.rows[i].State))
                        .replace("{2}", DateHandler(json.rows[i].CreatedTime))
                        .replace("{3}", json.rows[i].ClaimedTime ? DateHandler(json.rows[i].ClaimedTime) : "")
                        .replace("{4}", json.rows[i].EndTime ? DateHandler(json.rows[i].EndTime) : "")
                        .replace("{5}", json.rows[i].Comments ? json.rows[i].Comments : "")
                        .replace("{6}", json.rows[i].DisplayName ? json.rows[i].DisplayName : "")
                        .replace("{7}", json.rows[i].CompleteActorName ? json.rows[i].CompleteActorName : "")
                    }

                    output += "</pre>"
                }
                $('#workitems').html(output);

                //$('#workitems').slideDown('fast', function() {});  
                $("#dialog").dialog("open");
            }
            else{
                 $("#dialog").dialog("close"); 
            }
        }
        function showToolTip(uiobject){
            $('#workitems').html("读取中...");
            if(cache[currid + uiobject.props.Id.value ])
            {
               var json = cache[currid + uiobject.props.Id.value ];
               loadWorkItems(json);
            }
            else{
                $.ajax({
                    url: baseurl + "/ProcessTraceItemView" + "?t=" + new Date().toLocaleString(),
                    data: {
                        id:currid, 
                        actid:uiobject.props.Id.value, 
                        type:uiobject.type
                    },
                    dataType :"json",
                    success: function(json) {
                      cache[currid + uiobject.props.Id.value ] = json;
                      loadWorkItems(json);
                    }
                });  
            }  
        }

        $(function(){
           
            $(document).mousemove(function(e){
                gleft = e.pageX, gtop = e.pageY;
                $('#status').html("x:" + e.pageX +', y:'+ e.pageY);
            }); 

            $('#myflow').myflow($.extend(true, {
                basePath: "",
                restore: jsondata,
                editable: false},
                {
                    "activeRects": activeRects,
                    "historyRects": historyRects
                }

            ));
            
           $("#dialog").dialog({closed:true,shadow:false});

           title = $('#dialog').parent().find(".panel-header > .panel-title");
        });
    </script>
    <style type="text/css">
        body
        {
            margin: 0;
            padding: 0;
            text-align: left;
            font-family: Arial, sans-serif, Helvetica, Tahoma;
            font-size: 12px;
            line-height: 1.5;
            color: black;
        }
     
        #workitems
        {
            width: 99%;
            border:solid 1px #969696;           
            overflow: auto;
        }
       
        #status
        {
            position:absolute;
            top:10px;
            left:10px;
        }      
    </style>
    <uc1:ScriptBlock ID="ScriptBlockA" />
</head>
<body>
    <ld:Loading ID="Loading1" />
    <div id="status"></div>
    <div id="myflow" style="width: 100%" >
    </div>
    <div id="dlg" style="display:none;">
        <div id="dialog" class="easyui-dialog" title="详情" style="width: 320px;height:240px;">
             <div id="workitems">        
            </div>
        </div>
    </div>
</body>
</html>
