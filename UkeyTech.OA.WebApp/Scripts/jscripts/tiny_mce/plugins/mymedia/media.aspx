<%@ Page Language="C#" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Configuration" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    private string m_strBasePath = null;
    private string m_strCurrentPath = null;
    private string m_strCurrentFile = null;
    const string conUpFloder = "[ ... ]";
    
    protected override void OnLoad(EventArgs e)
    {
        m_strBasePath = "~/upload/Media";
        Response.Expires = 0;
        if (IsPostBack)
        {
            HandlePostBack();
        }
        else if (!IsPostBack)
        {
            LoadFiles();
        }
        base.OnLoad(e);
    }


    protected override void Render(HtmlTextWriter writer)
    {
        LoadFiles();
        base.Render(writer);
    }


    private void HandlePostBack()
    {
        string strArg = Request["__EVENTARGUMENT"];
        if (strArg != null)
        {
            general_tab.Attributes["class"] = "";
            general_panel.Attributes["class"] = "panel";




            search_panel.Attributes["class"] = "panel current";
            search_tab.Attributes["class"] = "current";

            if (strArg.StartsWith("CD|"))
            {
                string strFolder = strArg.Substring(3);
                if (strFolder.StartsWith("/")) strFolder = strFolder.Substring(1);
                if (!string.IsNullOrEmpty(strFolder))
                    m_strCurrentPath = m_strBasePath + "/" + strFolder;
                else if (strFolder == string.Empty)
                    m_strCurrentPath = m_strBasePath;

                m_strCurrentPath = m_strCurrentPath.Replace("//", "/");
                hdnPath.Value = m_strCurrentPath;
            }
            else if (strArg.StartsWith("UPLOAD"))
            {
                lblUpload.Text = "";
                
                m_strCurrentPath = m_strBasePath;
                if (!string.IsNullOrEmpty(hdnPath.Value))
                    m_strCurrentPath = hdnPath.Value;
                string strExt = System.IO.Path.GetExtension(upload.FileName.ToLower());
                if (upload.FileName.Length == 0)
                    lblUpload.Text = "请选择一个文件上传!";
                else if (upload.FileContent.Length == 0)
                    lblUpload.Text = "文件是空文件";

                else if (CheckExt(strExt.Replace(".", "")))
                {
                    string strFilename = System.IO.Path.GetFileName(upload.FileName);
                    int file_append = 0;
                    while (System.IO.File.Exists(Server.MapPath(m_strCurrentPath + "/" + strFilename)))
                    {
                        file_append++;
                        strFilename = System.IO.Path.GetFileNameWithoutExtension(upload.FileName)
                                         + "_" + file_append + System.IO.Path.GetExtension(upload.FileName);
                    }
                    strFilename = Server.MapPath(m_strCurrentPath + "/" + strFilename);
                    try
                    {
                        upload.SaveAs(strFilename);
                        m_strCurrentFile = ResolveURL(CurrentPath) + "/" + System.IO.Path.GetFileName(strFilename);
                        m_strCurrentFile = m_strCurrentFile.Replace("//", "/");
                    }
                    catch (Exception ex)
                    {
                        lblUpload.Text = "上传失败 : " + ex.Message;
                    }
                }
                else
                    lblUpload.Text = "格式:" + strExt + " 不是影片文件格式";
            }
        }
    }
    private static string BaseURL
    {
        get
        {
            string strURL = System.Web.HttpContext.Current.Request.ApplicationPath;
            if (!strURL.EndsWith("/"))
            {
                strURL += "/";
            }
            return strURL;
        }
    }

    protected string ResolveURL(string strURL)
    {
        if (strURL.Contains("~/"))
        {
            strURL = strURL.Replace("~/", BaseURL);
        }
        return strURL;
    }

    protected string CurrentPath
    {
        get
        {
            if (m_strCurrentPath == null) return m_strBasePath;
            return m_strCurrentPath;
        }
    }

    private void AddNewRow(DataTable dt, params string[] values)
    {
        dt.Rows.Add(values);
    }

    protected string getImg(object name, object ftype)
    {
        string rst = "";
        if (ftype.ToString() == "dir")
        {
            if (name.ToString() != conUpFloder)
            {
                rst = "<img align='top' src='img/folder.gif'>";
            }
            else
            {
                rst = "<img align='top' src='img/folderup.gif'>";
            }
        }

        return rst;
    }

    protected string getAction(object path, object ftype)
    {
        string rst = "";
        if (ftype.ToString() == "dir")
        {
            rst = string.Format("changeDir(\"{0}\");", path);
        }
        else
        {
            rst = string.Format("showPreview(\"{0}\");", path);
        }
        return rst;
    }

    protected string getDelLink(object name, object ftype)
    {
        string rst = "";
        if (ftype.ToString() == "dir")
        {
            if (name.ToString() != conUpFloder)
            {
                rst = "<a onclick=\"deleteDir(this)\" href=\"javascript:void(0);\">删除</a> ";
            }

        }
        else
        {
            rst = "<a onclick=\"deleteFile(this)\" href=\"javascript:void(0);\">删除</a> ";
        }

        return rst;
    }

    private bool CheckExt(string strExt)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(strExt, "wmv|swf|ra|rm|qt|asf", RegexOptions.IgnoreCase);
    }

    private void LoadFiles()
    {
        currFilePah.Value = "";

        if (string.IsNullOrEmpty(CurrentPath))
        {
            plcFileList.Controls.Add(new LiteralControl("该目录不存在"));
        }
        else
        {
            LinkButton lnk = new LinkButton();
            lnk.Text = "force!!!";
            plcHidden.Controls.Add(lnk);

            string strBase = Server.MapPath(CurrentPath);
            int nCount = 0;

            DataTable filesdt = new DataTable();
            filesdt.Columns.Add("FileName");
            filesdt.Columns.Add("FilePath");
            filesdt.Columns.Add("FileType");
            filesdt.Columns.Add("ID");

            if (CurrentPath != m_strBasePath)
            {
                string strUp = CurrentPath;
                strUp = strUp.Substring(0, strUp.LastIndexOf("/"));
                strUp = strUp.Replace(m_strBasePath, "");

                AddNewRow(filesdt, conUpFloder, strUp, "dir", "");

                nCount++;
            }
            //load list of sub directories
            foreach (DirectoryInfo dir in new DirectoryInfo(Server.MapPath(CurrentPath)).GetDirectories())
            {
                string strFile = dir.FullName;
                strFile = strFile.Replace(Server.MapPath(m_strBasePath), "").Replace("\\", "/");

                AddNewRow(filesdt, dir.Name, strFile, "dir", nCount.ToString());

                nCount++;
            }
            //load list of files

            foreach (FileInfo file in new DirectoryInfo(Server.MapPath(CurrentPath)).GetFiles())
            {
                string strExt = file.Extension.ToLower().Replace(".", "");
                if (CheckExt(strExt))
                {
                    string strFile = ResolveURL(CurrentPath) + "/" + file.Name;
                    strFile = strFile.Replace("//", "/");
                    string strStyle = nCount % 2 == 0 ? "SearchRow" : "SearchAltRow";


                    if (strFile == m_strCurrentFile)
                    {
                        //当前选中文件和改文件一致，则预览该文件
                        currFilePah.Value = m_strCurrentFile;
                    }

                    AddNewRow(filesdt, file.Name, strFile, "file", nCount.ToString());

                    nCount++;
                }
            }

            dgFileList.DataSource = filesdt;
            dgFileList.DataBind();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

    }

    protected void btnCreateNewDir_Click(object sender, EventArgs e)
    {
        lblDirMsg.Text = "";
        string newdirpath = Path.Combine(Path.Combine(Server.MapPath(m_strBasePath), hdnPath.Value), tbNewDir.Text);
        if (tbNewDir.Text == string.Empty)
        {
            lblDirMsg.Text = "创建的目录名不能为空";
            return;
        }
        
        //创建可上传目录
        if (!Directory.Exists(newdirpath))
        {
            try
            {
                Directory.CreateDirectory(newdirpath);
                LoadFiles();
            }
            catch (Exception ex)
            {
                lblDirMsg.Text = "创建失败：" + ex.Message;

            }
        }
        else
        {
            lblDirMsg.Text = string.Format("目录[{0}]已经存在，请使用其他名字", tbNewDir.Text);
        }
    }

    protected void dgFileList_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        this.dgFileList.CurrentPageIndex = e.NewPageIndex;
        LoadFiles();
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>{#media_dlg.title}</title>

    <script type="text/javascript" src="../../../../jquery.min.js"></script>
     <script type="text/javascript" src="../../../../jquery.scrollTo.min.js"></script>


    <script type="text/javascript" src="../../tiny_mce_popup.js"></script>
    
    <script type="text/javascript" src="js/media.js"></script>

    <script type="text/javascript" src="../../utils/mctabs.js"></script>

    <script type="text/javascript" src="../../utils/validate.js"></script>

    <script type="text/javascript" src="../../utils/form_utils.js"></script>

    <script type="text/javascript" src="../../utils/editable_selects.js"></script>

    <link href="css/media.css" rel="stylesheet" type="text/css" />

    <script language="javascript">
	var lastClass;
	var lastTD;
	function SetMedia(src)
	{
	    mcTabs.displayTab('general_tab','general_panel');
	    var s = document.getElementById('src');
        s.value = src;
        //s.value = convertURL(src);
        //showPreviewImage(src, 0);
        updatePreview();
	}  
 
    function SwitchClassBack(td, Class)
    {
        if (td == lastTD) {
            return;
        }
        td.className = Class;
    }

    function selectPic()
    {
        var img = document.getElementById('hidSelSrc');
        //alert(img.src);
        SetMedia(img.value);
    }
    
    function changeDir(dir){
        var theForm = document.forms['form1'];
        if (!theForm) {
            theForm = document.form1;
        }
        theForm.__EVENTTARGET.value = 'plcFileList';
        theForm.__EVENTARGUMENT.value = "CD|"+dir;
        theForm.submit();
    }
    
    function uploadPic() {
        var theForm = document.forms['form1'];
        if (!theForm) {
            theForm = document.form1;
        }
        theForm.__EVENTTARGET.value = 'plcFileList';
        theForm.__EVENTARGUMENT.value = "UPLOAD";
        theForm.submit();
    }
    
    function deleteFile(obj)
    {
        if(!confirm("你确定删除该文件?"))
            return;
         //delete temp files
          var delfile = encodeURI($(obj).parent().find("span").html());
                        
          $.ajax({
            url: '../../../../../Admin/ajax/file_upload.ashx' + "?type=DelTempImages" + "&file=" + delfile ,
            data: {
                t : new Date().getTime()
            },
            dataType: "Get",
          
            success: function(msg) {
                
                var arry = msg.split('##');
       
                if (arry[0] == 'success') {
                     $(obj).parent().parent().remove();                                        
                }
                else{
                    alert(arry);
                }
            }
        }); 
    }
    
    function deleteDir(obj)
    {
         if(!confirm("你确定删除该项目?该项目的文件会全部丢失!"))
            return;
        
        var delfile = encodeURI($(obj).parent().find("span").html());
                        
          $.ajax({
            url: '../../../../../Admin/ajax/file_upload.ashx' + "?type=DelTempDir" + "&file=" + delfile ,
            data: {
                t : new Date().getTime()
            },
            dataType: "Get",
          
            success: function(msg) {
                
                var arry = msg.split('##');
       
                if (arry[0] == 'success') {
                     $(obj).parent().parent().remove();                                        
                }
                else{
                    alert(arry);
                }
            }
        });   
    }
    
      function showPreview(src) {  
        //选择当前文件
        var hidspan = $(".search span:[href='" + src + "']");    
   
        var tr = $(hidspan).parent().parent().find("td:first");
        
        //设置滚动条      
        $(".search td").removeClass("SearchRowSelected");
	    $(tr).addClass("SearchRowSelected");
	    $('div.search').scrollTo($(hidspan),800);
		
        return false;
    }
    
     $(document).ready(function() {  
            $(".dgFileList tr").hover(
                function(){
                   $(this).addClass("SearchRowOver");  
            },
                function(){
                   $(this).removeClass("SearchRowOver"); 
                }
            );
            
             if($(".currFilePah").val()!="")
               {
              
                    showPreview($(".currFilePah").val());
               }
            
        
     });
    
    </script>

</head>
<body onload="tinyMCEPopup.executeOnLoad('init();');javascript:theForm = document.forms[0];" style="display: none">
    <form id="form1" runat="server">
        <input runat="server" id="hdnPath" type="hidden" />
        <input runat="server" id="currFilePah" type="hidden" class="currFilePah" value="" />
        <div class="tabs">
            <ul>
                <li id="general_tab" runat="server" class="current"><span><a href="javascript:mcTabs.displayTab('general_tab','general_panel');generatePreview();"
                    onmousedown="return false;">{#media_dlg.general}</a></span></li>
                <li runat="server" id="search_tab"><span><a href="javascript:mcTabs.displayTab('search_tab','search_panel');"
                    onmousedown="return false;">上传文件选择</a></span></li>
                <li id="advanced_tab"><span><a href="javascript:mcTabs.displayTab('advanced_tab','advanced_panel');"
                    onmousedown="return false;">{#media_dlg.advanced}</a></span></li>
            </ul>
        </div>
        <div class="panel_wrapper">
            <div id="general_panel" runat="server" class="panel current">
                <fieldset>
                    <legend>{#media_dlg.general}</legend>
                    <table border="0" cellpadding="4" cellspacing="0">
                        <tr>
                            <td>
                                <label for="media_type">
                                    {#media_dlg.type}</label></td>
                            <td>
                                <select id="media_type" name="media_type" onchange="changedType(this.value);generatePreview();">
                                    <option value="wmp">Windows Media</option>
                                    <option value="flash">Flash</option>
                                    <!-- <option value="flv">Flash video (FLV)</option> -->
                                    <option value="qt">Quicktime</option>
                                    <option value="shockwave">Shockwave</option>
                                    <option value="rmp">Real Media</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="src">
                                    {#media_dlg.file}</label></td>
                            <td>
                                <table border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <input id="src" name="src" type="text" value="" class="mceFocus" onchange="switchType(this.value);generatePreview();" /></td>
                                        <td id="filebrowsercontainer">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="linklistrow">
                            <td>
                                <label for="linklist">
                                    {#media_dlg.list}</label></td>
                            <td id="linklistcontainer">
                                <select id="linklist">
                                    <option value=""></option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="width">
                                    {#media_dlg.size}</label></td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="text" id="width" name="width" value="" class="size" onchange="generatePreview('width');" />
                                            x
                                            <input type="text" id="height" name="height" value="" class="size" onchange="generatePreview('height');" /></td>
                                        <td>
                                            &nbsp;&nbsp;<input id="constrain" type="checkbox" name="constrain" class="checkbox" /></td>
                                        <td>
                                            <label id="constrainlabel" for="constrain">
                                                {#media_dlg.constrain_proportions}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend>{#media_dlg.preview}</legend>
                    <div id="prev">
                    </div>
                </fieldset>
            </div>
            <div runat="server" id="search_panel" class="panel">
                <div class="search_panel">
                    <fieldset>
                        <legend>服务器上的文件</legend>
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td valign="top">
                                    <div class="search">
                                        <asp:PlaceHolder ID="plcFileList" runat="server"></asp:PlaceHolder>
                                         <asp:DataGrid ID="dgFileList" runat="server" AutoGenerateColumns="false" PageSize="50"
                                            CssClass="dgFileList" Width="95%" AllowPaging="true" OnPageIndexChanged="dgFileList_PageIndexChanged">
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr id='td<%#Eval("ID")%>'>
                                                                <td onclick='<%#getAction(Eval("FilePath"),Eval("FileType"))%>' width="80%"><%#getImg(Eval("FileName"), Eval("FileType"))%><%#Eval("FileName") %>
                                                                </td>
                                                                <td>
                                                                    <%# getDelLink(Eval("FileName"), Eval("FileType"))%>
                                                                    <span style="display: none;" href='<%# Eval("FileType").ToString() == "dir" ? (ResolveUrl(CurrentPath) + Eval("FilePath").ToString()) : Eval("FilePath").ToString()%>' class="hidUrl"><%# Eval("FileType").ToString() == "dir" ? (ResolveUrl(CurrentPath) + Eval("FilePath").ToString()) : Eval("FilePath").ToString()%></span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <AlternatingItemStyle CssClass="SearchAltRow" />
                                            <ItemStyle CssClass="SearchRow" />
                                            <PagerStyle NextPageText="下一页" PrevPageText="上一页" HorizontalAlign="Center" />
                                        </asp:DataGrid>
                                    </div>
                                    <input type='hidden' id="hidSelSrc" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset>
                        <legend>目录创建</legend>
                        目录名称：<asp:TextBox ID="tbNewDir" runat="server"></asp:TextBox>&nbsp;<asp:Button ID="btnCreateNewDir"
                            runat="server" OnClick="btnCreateNewDir_Click" Text="创建目录" />
                        <asp:Label ID="lblDirMsg" runat="server" ForeColor="Red"></asp:Label></fieldset>
                    </fieldset>
                    <fieldset>
                        <legend>上传文件</legend>
                        <asp:FileUpload CssClass="upload" ID="upload" runat="server" /><input class="uploadButton"
                            onclick="uploadPic();" type="button" name="btnUpload" value="上传" />
                        <asp:Label ID="lblUpload" runat="server" ForeColor="red" Text=""></asp:Label>
                    </fieldset>
                    <fieldset>
                        <legend>完成</legend>
                        <input class="uploadButton topSpace" onclick="selectPic();" type="button" name="btnSelect"
                            value="选择当前影片" />
                    </fieldset>
                </div>
            </div>
            <div id="advanced_panel" class="panel">
                <fieldset>
                    <legend>{#media_dlg.advanced}</legend>
                    <table border="0" cellpadding="4" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <label for="id">
                                    {#media_dlg.id}</label></td>
                            <td>
                                <input type="text" id="id" name="id" onchange="generatePreview();" /></td>
                            <td>
                                <label for="name">
                                    {#media_dlg.name}</label></td>
                            <td>
                                <input type="text" id="name" name="name" onchange="generatePreview();" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="align">
                                    {#media_dlg.align}</label></td>
                            <td>
                                <select id="align" name="align" onchange="generatePreview();">
                                    <option value="">{#not_set}</option>
                                    <option value="top">{#media_dlg.align_top}</option>
                                    <option value="right">{#media_dlg.align_right}</option>
                                    <option value="bottom">{#media_dlg.align_bottom}</option>
                                    <option value="left">{#media_dlg.align_left}</option>
                                </select>
                            </td>
                            <td>
                                <label for="bgcolor">
                                    {#media_dlg.bgcolor}</label></td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input id="bgcolor" name="bgcolor" type="text" value="" size="9" onchange="updateColor('bgcolor_pick','bgcolor');generatePreview();" /></td>
                                        <td id="bgcolor_pickcontainer">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="vspace">
                                    {#media_dlg.vspace}</label></td>
                            <td>
                                <input type="text" id="vspace" name="vspace" class="number" onchange="generatePreview();" /></td>
                            <td>
                                <label for="hspace">
                                    {#media_dlg.hspace}</label></td>
                            <td>
                                <input type="text" id="hspace" name="hspace" class="number" onchange="generatePreview();" /></td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset id="flash_options">
                    <legend>{#media_dlg.flash_options}</legend>
                    <table border="0" cellpadding="4" cellspacing="0">
                        <tr>
                            <td>
                                <label for="flash_quality">
                                    {#media_dlg.quality}</label></td>
                            <td>
                                <select id="flash_quality" name="flash_quality" onchange="generatePreview();">
                                    <option value="">{#not_set}</option>
                                    <option value="high">high</option>
                                    <option value="low">low</option>
                                    <option value="autolow">autolow</option>
                                    <option value="autohigh">autohigh</option>
                                    <option value="best">best</option>
                                </select>
                            </td>
                            <td>
                                <label for="flash_scale">
                                    {#media_dlg.scale}</label></td>
                            <td>
                                <select id="flash_scale" name="flash_scale" onchange="generatePreview();">
                                    <option value="">{#not_set}</option>
                                    <option value="showall">showall</option>
                                    <option value="noborder">noborder</option>
                                    <option value="exactfit">exactfit</option>
                                    <option value="noscale">noscale</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="flash_wmode">
                                    {#media_dlg.wmode}</label></td>
                            <td>
                                <select id="flash_wmode" name="flash_wmode" onchange="generatePreview();">
                                    <option value="">{#not_set}</option>
                                    <option value="window">window</option>
                                    <option value="opaque">opaque</option>
                                    <option value="transparent">transparent</option>
                                </select>
                            </td>
                            <td>
                                <label for="flash_salign">
                                    {#media_dlg.salign}</label></td>
                            <td>
                                <select id="flash_salign" name="flash_salign" onchange="generatePreview();">
                                    <option value="">{#not_set}</option>
                                    <option value="l">{#media_dlg.align_left}</option>
                                    <option value="t">{#media_dlg.align_top}</option>
                                    <option value="r">{#media_dlg.align_right}</option>
                                    <option value="b">{#media_dlg.align_bottom}</option>
                                    <option value="tl">{#media_dlg.align_top_left}</option>
                                    <option value="tr">{#media_dlg.align_top_right}</option>
                                    <option value="bl">{#media_dlg.align_bottom_left}</option>
                                    <option value="br">{#media_dlg.align_bottom_right}</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="flash_play" name="flash_play" checked="checked"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="flash_play">
                                                {#media_dlg.play}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="flash_loop" name="flash_loop" checked="checked"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="flash_loop">
                                                {#media_dlg.loop}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="flash_menu" name="flash_menu" checked="checked"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="flash_menu">
                                                {#media_dlg.menu}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="flash_swliveconnect" name="flash_swliveconnect"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="flash_swliveconnect">
                                                {#media_dlg.liveconnect}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <label for="flash_base">
                                    {#media_dlg.base}</label></td>
                            <td>
                                <input type="text" id="flash_base" name="flash_base" onchange="generatePreview();" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="flash_flashvars">
                                    {#media_dlg.flashvars}</label></td>
                            <td>
                                <input type="text" id="flash_flashvars" name="flash_flashvars" onchange="generatePreview();" /></td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset id="flv_options">
                    <legend>{#media_dlg.flv_options}</legend>
                    <table border="0" cellpadding="4" cellspacing="0">
                        <tr>
                            <td>
                                <label for="flv_scalemode">
                                    {#media_dlg.flv_scalemode}</label></td>
                            <td>
                                <select id="flv_scalemode" name="flv_scalemode" onchange="generatePreview();">
                                    <option value="">{#not_set}</option>
                                    <option value="none">none</option>
                                    <option value="double">double</option>
                                    <option value="full">full</option>
                                </select>
                            </td>
                            <td>
                                <label for="flv_buffer">
                                    {#media_dlg.flv_buffer}</label></td>
                            <td>
                                <input type="text" id="flv_buffer" name="flv_buffer" onchange="generatePreview();" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="flv_startimage">
                                    {#media_dlg.flv_startimage}</label></td>
                            <td>
                                <input type="text" id="flv_startimage" name="flv_startimage" onchange="generatePreview();" /></td>
                            <td>
                                <label for="flv_starttime">
                                    {#media_dlg.flv_starttime}</label></td>
                            <td>
                                <input type="text" id="flv_starttime" name="flv_starttime" onchange="generatePreview();" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="flv_defaultvolume">
                                    {#media_dlg.flv_defaultvolume}</label></td>
                            <td>
                                <input type="text" id="flv_defaultvolume" name="flv_defaultvolume" onchange="generatePreview();" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="flv_hiddengui" name="flv_hiddengui" checked="checked"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="flv_hiddengui">
                                                {#media_dlg.flv_hiddengui}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="flv_autostart" name="flv_autostart" checked="checked"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="flv_autostart">
                                                {#media_dlg.flv_autostart}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="flv_loop" name="flv_loop" checked="checked"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="flv_loop">
                                                {#media_dlg.flv_loop}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="flv_showscalemodes" name="flv_showscalemodes"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="flv_showscalemodes">
                                                {#media_dlg.flv_showscalemodes}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="flv_smoothvideo" name="flash_flv_flv_smoothvideosmoothvideo"
                                                checked="checked" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="flv_smoothvideo">
                                                {#media_dlg.flv_smoothvideo}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="flv_jscallback" name="flv_jscallback"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="flv_jscallback">
                                                {#media_dlg.flv_jscallback}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset id="qt_options">
                    <legend>{#media_dlg.qt_options}</legend>
                    <table border="0" cellpadding="4" cellspacing="0">
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="qt_loop" name="qt_loop" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="qt_loop">
                                                {#media_dlg.loop}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="qt_autoplay" name="qt_autoplay" checked="checked"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="qt_autoplay">
                                                {#media_dlg.play}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="qt_cache" name="qt_cache" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="qt_cache">
                                                {#media_dlg.cache}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="qt_controller" name="qt_controller" checked="checked"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="qt_controller">
                                                {#media_dlg.controller}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="qt_correction" name="qt_correction" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="qt_correction">
                                                {#media_dlg.correction}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="qt_enablejavascript" name="qt_enablejavascript"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="qt_enablejavascript">
                                                {#media_dlg.enablejavascript}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="qt_kioskmode" name="qt_kioskmode" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="qt_kioskmode">
                                                {#media_dlg.kioskmode}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="qt_autohref" name="qt_autohref" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="qt_autohref">
                                                {#media_dlg.autohref}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="qt_playeveryframe" name="qt_playeveryframe"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="qt_playeveryframe">
                                                {#media_dlg.playeveryframe}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="qt_targetcache" name="qt_targetcache"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="qt_targetcache">
                                                {#media_dlg.targetcache}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="qt_scale">
                                    {#media_dlg.scale}</label></td>
                            <td>
                                <select id="qt_scale" name="qt_scale" class="mceEditableSelect" onchange="generatePreview();">
                                    <option value="">{#not_set}</option>
                                    <option value="tofit">tofit</option>
                                    <option value="aspect">aspect</option>
                                </select>
                            </td>
                            <td colspan="2">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <label for="qt_starttime">
                                    {#media_dlg.starttime}</label></td>
                            <td>
                                <input type="text" id="qt_starttime" name="qt_starttime" onchange="generatePreview();" /></td>
                            <td>
                                <label for="qt_endtime">
                                    {#media_dlg.endtime}</label></td>
                            <td>
                                <input type="text" id="qt_endtime" name="qt_endtime" onchange="generatePreview();" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="qt_target">
                                    {#media_dlg.target}</label></td>
                            <td>
                                <input type="text" id="qt_target" name="qt_target" onchange="generatePreview();" /></td>
                            <td>
                                <label for="qt_href">
                                    {#media_dlg.href}</label></td>
                            <td>
                                <input type="text" id="qt_href" name="qt_href" onchange="generatePreview();" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="qt_qtsrcchokespeed">
                                    {#media_dlg.qtsrcchokespeed}</label></td>
                            <td>
                                <input type="text" id="qt_qtsrcchokespeed" name="qt_qtsrcchokespeed" onchange="generatePreview();" /></td>
                            <td>
                                <label for="qt_volume">
                                    {#media_dlg.volume}</label></td>
                            <td>
                                <input type="text" id="qt_volume" name="qt_volume" onchange="generatePreview();" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="qt_qtsrc">
                                    {#media_dlg.qtsrc}</label></td>
                            <td colspan="4">
                                <table border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <input type="text" id="qt_qtsrc" name="qt_qtsrc" onchange="generatePreview();" /></td>
                                        <td id="qtsrcfilebrowsercontainer">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset id="wmp_options">
                    <legend>{#media_dlg.wmp_options}</legend>
                    <table border="0" cellpadding="4" cellspacing="0">
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="wmp_autostart" name="wmp_autostart" checked="checked"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="wmp_autostart">
                                                {#media_dlg.autostart}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="wmp_enabled" name="wmp_enabled" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="wmp_enabled">
                                                {#media_dlg.enabled}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="wmp_enablecontextmenu" name="wmp_enablecontextmenu"
                                                checked="checked" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="wmp_enablecontextmenu">
                                                {#media_dlg.menu}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="wmp_fullscreen" name="wmp_fullscreen"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="wmp_fullscreen">
                                                {#media_dlg.fullscreen}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="wmp_invokeurls" name="wmp_invokeurls"
                                                checked="checked" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="wmp_invokeurls">
                                                {#media_dlg.invokeurls}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="wmp_mute" name="wmp_mute" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="wmp_mute">
                                                {#media_dlg.mute}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="wmp_stretchtofit" name="wmp_stretchtofit"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="wmp_stretchtofit">
                                                {#media_dlg.stretchtofit}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="wmp_windowlessvideo" name="wmp_windowlessvideo"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="wmp_windowlessvideo">
                                                {#media_dlg.windowlessvideo}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="wmp_balance">
                                    {#media_dlg.balance}</label></td>
                            <td>
                                <input type="text" id="wmp_balance" name="wmp_balance" onchange="generatePreview();" /></td>
                            <td>
                                <label for="wmp_baseurl">
                                    {#media_dlg.baseurl}</label></td>
                            <td>
                                <input type="text" id="wmp_baseurl" name="wmp_baseurl" onchange="generatePreview();" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="wmp_captioningid">
                                    {#media_dlg.captioningid}</label></td>
                            <td>
                                <input type="text" id="wmp_captioningid" name="wmp_captioningid" onchange="generatePreview();" /></td>
                            <td>
                                <label for="wmp_currentmarker">
                                    {#media_dlg.currentmarker}</label></td>
                            <td>
                                <input type="text" id="wmp_currentmarker" name="wmp_currentmarker" onchange="generatePreview();" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="wmp_currentposition">
                                    {#media_dlg.currentposition}</label></td>
                            <td>
                                <input type="text" id="wmp_currentposition" name="wmp_currentposition" onchange="generatePreview();" /></td>
                            <td>
                                <label for="wmp_defaultframe">
                                    {#media_dlg.defaultframe}</label></td>
                            <td>
                                <input type="text" id="wmp_defaultframe" name="wmp_defaultframe" onchange="generatePreview();" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="wmp_playcount">
                                    {#media_dlg.playcount}</label></td>
                            <td>
                                <input type="text" id="wmp_playcount" name="wmp_playcount" onchange="generatePreview();" /></td>
                            <td>
                                <label for="wmp_rate">
                                    {#media_dlg.rate}</label></td>
                            <td>
                                <input type="text" id="wmp_rate" name="wmp_rate" onchange="generatePreview();" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="wmp_uimode">
                                    {#media_dlg.uimode}</label></td>
                            <td>
                                <input type="text" id="wmp_uimode" name="wmp_uimode" onchange="generatePreview();" /></td>
                            <td>
                                <label for="wmp_volume">
                                    {#media_dlg.volume}</label></td>
                            <td>
                                <input type="text" id="wmp_volume" name="wmp_volume" onchange="generatePreview();" /></td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset id="rmp_options">
                    <legend>{#media_dlg.rmp_options}</legend>
                    <table border="0" cellpadding="4" cellspacing="0">
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="rmp_autostart" name="rmp_autostart" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="rmp_autostart">
                                                {#media_dlg.autostart}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="rmp_loop" name="rmp_loop" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="rmp_loop">
                                                {#media_dlg.loop}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="rmp_autogotourl" name="rmp_autogotourl"
                                                checked="checked" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="rmp_autogotourl">
                                                {#media_dlg.autogotourl}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="rmp_center" name="rmp_center" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="rmp_center">
                                                {#media_dlg.center}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="rmp_imagestatus" name="rmp_imagestatus"
                                                checked="checked" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="rmp_imagestatus">
                                                {#media_dlg.imagestatus}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="rmp_maintainaspect" name="rmp_maintainaspect"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="rmp_maintainaspect">
                                                {#media_dlg.maintainaspect}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="rmp_nojava" name="rmp_nojava" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="rmp_nojava">
                                                {#media_dlg.nojava}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="rmp_prefetch" name="rmp_prefetch" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="rmp_prefetch">
                                                {#media_dlg.prefetch}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="rmp_shuffle" name="rmp_shuffle" onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="rmp_shuffle">
                                                {#media_dlg.shuffle}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label for="rmp_console">
                                    {#media_dlg.console}</label></td>
                            <td>
                                <input type="text" id="rmp_console" name="rmp_console" onchange="generatePreview();" /></td>
                            <td>
                                <label for="rmp_controls">
                                    {#media_dlg.controls}</label></td>
                            <td>
                                <input type="text" id="rmp_controls" name="rmp_controls" onchange="generatePreview();" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="rmp_numloop">
                                    {#media_dlg.numloop}</label></td>
                            <td>
                                <input type="text" id="rmp_numloop" name="rmp_numloop" onchange="generatePreview();" /></td>
                            <td>
                                <label for="rmp_scriptcallbacks">
                                    {#media_dlg.scriptcallbacks}</label></td>
                            <td>
                                <input type="text" id="rmp_scriptcallbacks" name="rmp_scriptcallbacks" onchange="generatePreview();" /></td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset id="shockwave_options">
                    <legend>{#media_dlg.shockwave_options}</legend>
                    <table border="0" cellpadding="4" cellspacing="0">
                        <tr>
                            <td>
                                <label for="shockwave_swstretchstyle">
                                    {#media_dlg.swstretchstyle}</label></td>
                            <td>
                                <select id="shockwave_swstretchstyle" name="shockwave_swstretchstyle" onchange="generatePreview();">
                                    <option value="none">{#not_set}</option>
                                    <option value="meet">Meet</option>
                                    <option value="fill">Fill</option>
                                    <option value="stage">Stage</option>
                                </select>
                            </td>
                            <td>
                                <label for="shockwave_swvolume">
                                    {#media_dlg.volume}</label></td>
                            <td>
                                <input type="text" id="shockwave_swvolume" name="shockwave_swvolume" onchange="generatePreview();" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="shockwave_swstretchhalign">
                                    {#media_dlg.swstretchhalign}</label></td>
                            <td>
                                <select id="shockwave_swstretchhalign" name="shockwave_swstretchhalign" onchange="generatePreview();">
                                    <option value="none">{#not_set}</option>
                                    <option value="left">{#media_dlg.align_left}</option>
                                    <option value="center">{#media_dlg.align_center}</option>
                                    <option value="right">{#media_dlg.align_right}</option>
                                </select>
                            </td>
                            <td>
                                <label for="shockwave_swstretchvalign">
                                    {#media_dlg.swstretchvalign}</label></td>
                            <td>
                                <select id="shockwave_swstretchvalign" name="shockwave_swstretchvalign" onchange="generatePreview();">
                                    <option value="none">{#not_set}</option>
                                    <option value="meet">Meet</option>
                                    <option value="fill">Fill</option>
                                    <option value="stage">Stage</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="shockwave_autostart" name="shockwave_autostart"
                                                onchange="generatePreview();" checked="checked" /></td>
                                        <td>
                                            <label for="shockwave_autostart">
                                                {#media_dlg.autostart}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="shockwave_sound" name="shockwave_sound"
                                                onchange="generatePreview();" checked="checked" /></td>
                                        <td>
                                            <label for="shockwave_sound">
                                                {#media_dlg.sound}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="shockwave_swliveconnect" name="shockwave_swliveconnect"
                                                onchange="generatePreview();" /></td>
                                        <td>
                                            <label for="shockwave_swliveconnect">
                                                {#media_dlg.liveconnect}</label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkbox" id="shockwave_progress" name="shockwave_progress"
                                                onchange="generatePreview();" checked="checked" /></td>
                                        <td>
                                            <label for="shockwave_progress">
                                                {#media_dlg.progress}</label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </div>
        <div class="mceActionPanel">
            <input type="button" id="insert" name="insert" value="{#insert}" onclick="insertMedia();" />
            <input type="button" id="cancel" name="cancel" value="{#cancel}" onclick="tinyMCEPopup.close();" />
        </div>
        <div style="display: none">
            <asp:PlaceHolder ID="plcHidden" runat="server"></asp:PlaceHolder>
        </div>
        <div>
            <asp:Button ID="Button1" UseSubmitBehavior="false" Visible="false" runat="server"
                Text="Button" OnClick="Button1_Click" />
        </div>
    </form>
</body>
</html>
