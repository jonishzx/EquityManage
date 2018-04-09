$(function () {
    BindRightAccordion();
})

//绑定新菜单
function BindRightAccordion() {
    $("#RightAccordion").accordion({ //初始化accordion
        fillSpace: true,
        fit: true,
        border: false,
        animate: false
    });
    //获取第一层初始目录
    $.post("/Home/GetFirstTree", { "id": "0" },
        function (data) {
           if (data == "0") {
               window.location.href = '/Login/Index';
           }
           $.each(data, function (i, e) {
               var id = e.Id;
               $('#RightAccordion').accordion('add', {
                   title: e.ModuleName,
                   content: "<ul id='tree" + id + "' ></ul>",
                   selected: true,
                   iconCls: e.ImageUrl
               });
               $.parser.parse();
               //获取二级以下目录 含2级
               $.post("/Home/GetTreeByEasyui?id=" + id, function (data) {
                   $("#tree" + id).tree({
                       data: data,
                       onBeforeExpand: function (node, param) {
                           debugger;
                           $("#tree" + id).tree('options').url = "/Home/GetTreeByEasyui?id=" + node.id;
                       },
                       onClick: function (node) {
                           debugger;
                           if (node.state == 'closed') {
                               $(this).tree('expand', node.target);
                           } else if (node.state == 'open') {
                               $(this).tree('collapse', node.target);
                               var tabTitle = node.text;
                               var url = node.attributes;
                               var icon = node.iconCls;
                               addTab(tabTitle, url, icon);
                           }
                       }
                   });
               }, 'json');
           });
       }, "json");
}

//绑定前台菜单栏 
function BindTreeData() {
    $('#LeftMenuTree').tree({    //初始化左侧功能树（不同用户显示的树是不同的）
        method: 'GET',
        url: '/Home/LoadMenuData',
        lines: true,
        onClick: function (node) {    //点击左侧的tree节点  打开右侧tabs显示内容
            if (node.attributes) {
                addTab(node.text, node.attributes.url, node.iconCls);
            }
        }
    });
}

//选项卡
function addTab(subtitle, url, icon) {
    debugger;
    var strHtml = '<iframe name="' + subtitle + '" scrolling="no" frameborder="0"  src="' + url + '" style="width:100%;height:99.9%;" marginheight="0" marginwidth="0"></iframe>';
    //var strHtml = '<iframe id="frmWorkArea" width="99.5%" height="99%" style="padding:1px;" frameborder="0" scrolling="yes" src="' + url + '"></iframe>';
    if (!$('#tabs').tabs('exists', subtitle)) {
        $('#tabs').tabs('add', {
            title: subtitle,
            content: strHtml,
            iconCls: icon,
            closable: true,
            loadingMessage: '正在加载中......'
        });
    } else {
        $('#tabs').tabs('select', subtitle);
    }
}
