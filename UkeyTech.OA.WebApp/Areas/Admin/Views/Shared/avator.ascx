<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript" src="<%=Url.Content("~/Scripts/ajaxupload.3.5.js")%>"></script>
<script type="text/javascript">


    function initImgCover_<%=ViewData.Eval("ID")%>()
    {
        var uploadimg = $("#avator<%=ViewData.Eval("ID")%>").find(".hidImgCover").val();
           var container = $("#avator<%=ViewData.Eval("ID")%>");
          var status = $(container).find(".status");
          var img = $(container).find(".avator");
          var hideimg = $(container).find(".hidImgCover");
          var delpic = $(container).find(".uploadContainer");
        if(uploadimg){
               $("#avator<%=ViewData.Eval("ID")%>").find(".avator").attr("src", uploadimg);
               $(delpic).show();
        }
       
        
          var a_<%=ViewData.Eval("ID")%>  = new AjaxUpload($(container).find(".upload"), {
			    action: '<%=Url.Action("UploadFile","Attachment",new {area = "Admin"})%>' + "?type=AvatorImages" + "&guid=<%=ViewData.Eval("GUID")%>",
			    name: 'uploadfile',
			    onSubmit: function(file, ext){
				    if (! (ext && /^(jpg|png|jpeg|gif|bmp)$/.test(ext))){ 
                        // extension is not allowed 
					    $(status).text('只允许上传 JPG, PNG, GIF或 BMP 的文件');
					    return false;
				    }
				    $(status).text('上传中...');
			    },
			    onComplete: function(file, response){
				    $(status).text('');
				    var rtnMsg = response.split("##");
                    
				    if(rtnMsg[0]==="success"){				    				  
                        var msg = strToJson(rtnMsg[1]);
					    $(img).attr("src", msg.url);
					    $(hideimg).val(msg.url);
                        $(delpic).show();
				    } else{
    				    $(status).text(rtnMsg[1]);				        					    
				    }				  
			    }
		      });

         
    }

    function delAvator<%=ViewData.Eval("ID")%>() { 
        if(confirm('你确定删除该图片?')){
            var container = $("#avator<%=ViewData.Eval("ID")%>");
            $(container).find(".hidImgCover").val('');
            $(container).find(".oldImgCover").val('');
            $(container).find(".avator").attr("src",
                    '<%=Url.Content("~/Content/Images/noimg.gif") %>');
             $(container).find(".uploadContainer").hide();
        }
    }
    $(function(){
          initImgCover_<%=ViewData.Eval("ID")%>();
    });
</script>
<div id="avator<%=ViewData.Eval("ID")%>">   
    <!--封面图片上传-->
    <img class="avator upload" title="点击上传图片" runat="server" src="~/Content/Images/noimg.gif" width="100" height="100" /> 
    <input type="hidden" name="<%=ViewData.Eval("ID")%>" class="hidImgCover" value="<% =ViewData.Eval("Value")%>" />
    <input type="hidden" value="<%=ViewData.Eval("OldAvator") %>" class="oldImgCover" value="<% =ViewData.Eval("Value")%>" />
    <div class="uploadContainer" style="  display: none;
  cursor: pointer;
  background: #00b7ee;
  padding: 3px 5px;
  color: #fff;
  width : 90px;
  text-align: center;
  border-radius: 3px;
  overflow: hidden;">
        <a class="del-pic" style="width:100%;display:inline-block" onclick="return delAvator<%=ViewData.Eval("ID")%>();"> 
        <span class="upload"><span>删除</span></span></a>
    </div>
    <span class="status"></span>
</div>