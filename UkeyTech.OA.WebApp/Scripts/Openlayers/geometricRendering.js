var constructionDate = $("#ConstructionDate").val();
var mapNO = $("#MapNO").val();
var sourceBussId = "";



var flashFeature;  //热区要素
var preFeature;  //前一个热区要素
var flag = false; //是否是同一个要素的标识
var feature; //当前鼠标选中要素
var draw; //绘制对象
var geoStr = null; // 当前绘制图形的坐标串
var currentFeature = null; //当前绘制的几何要素
var isDraw = true;
var graphShapeDetails = []; //当前施工申请绘制图形集合

function ini_GraphShapeDetails() {
    graphShapeDetails = [];
}


var vectStyle1 = function (feature) {
    return function (feature) {
        var style = new ol.style.Style({
            fill: new ol.style.Fill({
                color: feature.get('color'),
                opacity: 0.5
            }),
            stroke: new ol.style.Stroke({
                color: '#FF0000',
                width: 1
            }),
            image: new ol.style.Circle({
                radius: 7,
                fill: new ol.style.Fill({
                    color: '#0099ff'
                })
            }),
            text: new ol.style.Text({
                textAlign: 'center', //位置
                textBaseline: 'middle', //基准线
                font: '28px', //文字样式
                text: feature.get('name'), //文本内容
                fill: new ol.style.Fill({ color: 'blue' }), //文本填充样式（即文字颜色）
                stroke: new ol.style.Stroke({ color: '#ffffff', width: '2' }), //文本外框样式（颜色与宽度）
                offsetX: 0, //偏移量X
                offsetY: 0, //偏移量Y
                rotation: 0 //角度
            })
        });
        return [style];
    }
}

//矢量数据源
var vectSource = new ol.source.Vector({
});
//矢量图层
var vectLayer = new ol.layer.Vector({
    source: vectSource,
    style: vectStyle1(),
    opacity: 0.9
});
var map = new ol.Map({
    layers: [
        new ol.layer.Tile({
            source: new ol.source.XYZ({
                url: getSourceMapUrl
            })
        }),

        vectLayer  //矢量要素图层
        //hotSpotsLayer  //热区绘制图层
    ],
    controls: ol.control.defaults().extend([
          new ol.control.MousePosition(),
          new ol.control.FullScreen()
    ]),
    target: 'map', //地图容器div的ID
    view: new ol.View({
        center: ol.proj.transform([2, 41], 'EPSG:4326', 'EPSG:3857'), //地图初始中心点
        minZoom: 0,
        maxZoom: 7,
        zoom: 1
    })
});
/**
* 在地图容器中创建一个Overlay
*/
var element = document.getElementById('popup');
var popup = new ol.Overlay(/** @type {olx.OverlayOptions} */({
    element: element,
    positioning: 'bottom-center',
    stopEvent: false
}));
map.addOverlay(popup);



if (constructionDate == undefined) {
    constructionDate = $("#SGDate").val();
}

function changeDate() {
    if (constructionDate != $dp.cal.getNewDateStr()) {
        constructionDate = $dp.cal.getNewDateStr();
        //获取编号
        getMapNo();
        selectRegData();
    }
    return true;
}




if (constructionDate != "") {
    //如果施工时间不为空加载地图
    sourceBussId = $('#SourceBussId').val();
    selectRegData();
}

function getMapNo() {
    var strBillDate = constructionDate.replace(/-/g, '');
    $.ajax({
        type: "POST",
        url: getMapNoUrl,
        async: false,
        data: { strBillDate: strBillDate },
        dataType: "text",
        success: function (result) {
            $("#MapNO").val(result);
            mapNO = result;
        }
    });
}


/**
* 通过后台查询热区要素
*/
function selectRegData() {
    var result = false;
    $.ajax({
        url: getShapeListUrl, //请求地址
        type: 'POST', //请求方式为post
        data: { createTime: constructionDate, bussId: bussId, sourceBussId: sourceBussId }, //传入参数 
        dataType: 'json', //返回数据格式
        async: false,
        //请求成功完成后要执行的方法  
        success: function (msg) {
            showRegCallBack(msg);
            result = true;
        },
        error: function (err) {
            alert("加载施工区域失败");
        }
    });
    sourceBussId = "";
    return result;
}

/**
* 显示热区查询请求回调函数
* @param {json} data 查询返回的数据
*/
function showRegCallBack(data) {
    preFeature = null;
    flag = false;  //还原要素判断标识
    //hotSpotsSource.clear(); //清空绘图层数据源
    //hotSpotsLayer.setVisible(true); //显示绘图层
    vectSource.clear(); //清空矢量图层数据源
    var resultData = data.rows; //查询结果json数
      
  $.each(graphShapeDetails,function(i, item) {
      var result = resultData.filter(m => m.ShapeID === item.ShapeID);
      if (result.length === 0) {
          resultData.push(item); //把新画的施工区域追加到区域
      }
  });

    ini_GraphShapeDetails();
    $.each(resultData, function (i, item) {
        //解析结果集中的几何字符串，并转换为多边形的几何数组
        var polyCoords = item.ShapeData.split(";");
        //创建一个新的要素，并添加到数据源中
        var feature;
        if (item.BussID == bussId) {
            item.ShapeName = mapNO;
            item.ShapeID = ("undefined" != typeof ($("#SourceBussName").val()) && $("#SourceBussName").val() == bussId) ? newGuid(true) : item.ShapeID;  //如果是新增修改 加载修改单据图形要赋值新的shapeID
            var model = {};
            model.ShapeID = item.ShapeID;
            model.ShapeName = item.ShapeName; //图形名称
            model.ShapeType = item.ShapeType; //图形类型(园 / 矩形 / 曲线)
            model.ShapeData = item.ShapeData; //形状内容,以json文本格式存储
            model.CreateTime = constructionDate; //施工时间
            model.PrintDetailID = item.PrintDetailID;
            model.Version = "1.0.0.0";
            model.BussID = $("#SourceBussName").val() == bussId ? $("#NewModifiedNo").val() : bussId;//如果是新增修改 加载修改单据图形要赋值新的BussID
            model.Color = getColors();
   
            graphShapeDetails.push(model);
        }
        debugger
        

        switch (item.ShapeType) {
            case "Circle":
                var geoCenter = [new Number(polyCoords[0]).valueOf(), new Number(polyCoords[1]).valueOf()];
                var geoOptRadius = new Number(polyCoords[2]).valueOf();
                var geoOptLayout = polyCoords[3];
                var geometry = new ol.geom.Circle(geoCenter, geoOptRadius, geoOptLayout);
                feature = new ol.Feature({
                    geometry: geometry,
                    name: item.ShapeName,
                    id: item.ShapeID,
                    color: item.Color,
                    bussID: item.BussID
                });
                break;
            default:
                var coordinates = new Array();
                coordinates[0] = new Array();
                for (var i = 0; i < polyCoords.length; i++) {
                    coordinates[0][i] = polyCoords[i].split(",");
                }
                feature = new ol.Feature({
                    geometry: new ol.geom.Polygon(coordinates),
                    name: item.ShapeName,
                    id: item.ShapeID,
                    color: item.Color,
                    bussID: item.BussID
                });
                break;
        }
        vectSource.addFeature(feature);
    });
    //map.on('pointermove', pointermoveFun, this); //添加鼠标移动事件监听，捕获要素时添加热区功能
}

/**
* 【绘制热区】功能按钮处理函数
*/
function drawReg() {
    map.removeInteraction(draw); //移除绘制控件
    map.un('singleclick', singleclickFun, this); //移除鼠标单击事件监听
    var createTime = $("#ConstructionDate").val();
    if (createTime == undefined) {
        createTime = $("#SGDate").val();
    }
    if (createTime == "") {
        // setTimeout("MsgShow('系统提示','请填写施工日期');", 800);
        alert("请填写施工日期");
        return;
    }
    //实例化交互绘制类对象并添加到地图容器中
    getTypeSelect();
    //添加绘制结束事件监听，在绘制结束后保存信息到数据库

    draw.on('drawend', drawEndCallBack, this);
};

/**
*  获取选择的绘制类型
*/

function getTypeSelect() {
    var value = document.getElementById('type').value; //绘制类型对象
    if (value !== 'None') {
        if (vectSource == null) {
            vectSource = new ol.source.Vector({ wrapX: false });
            vectLayer.setSource(vectSource); //添加绘制层数据源
        }
        var geometryFunction, maxPoints;
        if (value === 'Square') {
            value = 'Circle';
            maxPoints = 2;
            geometryFunction = ol.interaction.Draw.createRegularPolygon(4); //正方形图形（圆）
        } else if (value === 'Box') {
            value = 'LineString';
            maxPoints = 2;
            geometryFunction = function (coordinates, geometry) {
                if (!geometry) {
                    geometry = new ol.geom.Polygon(null); //多边形
                }
                var start = coordinates[0];
                var end = coordinates[1];
                geometry.setCoordinates([
                    [start, [start[0], end[1]], end, [end[0], start[1]], start]
                ]);
                return geometry;
            };
        }
        //实例化交互绘制类对象并添加到地图容器中
        draw = new ol.interaction.Draw({
            source: vectSource, //绘制层数据源
            type: /** @type {ol.geom.GeometryType} */(value), //几何图形类型
            geometryFunction: geometryFunction, //几何信息变更时调用函数
            maxPoints: maxPoints //最大点数
        });
        map.addInteraction(draw);
    }
};

/**
* 绘制结束事件的回调函数，
* @param {ol.interaction.DrawEvent} evt 绘制结束事件
*/
function drawEndCallBack(evt) {
    var geoType = document.getElementById('type').value;

    map.removeInteraction(draw); //移除绘制控件
    //$("#dialog-confirm").dialog("open"); //打开属性信息设置对话框
    currentFeature = evt.feature; //当前绘制的要素

    var geo = currentFeature.getGeometry(); //获取要素的几何信息

    if (geoType == "Circle") {
        geoStr = geo.getCenter().join(";") + ";" + geo.getRadius() + ";" + geo.getLayout();
    } else {
        var coordinates = geo.getCoordinates(); //获取几何坐标
        geoStr = coordinates[0].join(";");
    }
    // "Polygon"绘制图形类型   
    // OpenInsertConfirm();
    //submitData();
    drawPolygon();
}

function drawPolygon() {
    var shapeType = document.getElementById('type').value;
    if (geoStr != null) {
        var model = {};
        model.ShapeID = newGuid(true);
        model.ShapeName = mapNO; //图形名称
        model.ShapeType = shapeType; //图形类型(园 / 矩形 / 曲线)
        model.ShapeData = geoStr; //形状内容,以json文本格式存储
        model.CreateTime = constructionDate; //施工时间
        model.PrintDetailID = printDetailID;
        model.Version = "1.0.0.0";
        model.BussID =("undefined" != typeof ($("#NewModifiedNo").val()))!="" ? $("#NewModifiedNo").val(): bussId;
        model.Color = getColors();

        currentFeature.set("name", model.ShapeName);
        currentFeature.set("id", model.ShapeID);
        currentFeature.set("color", model.Color);
        currentFeature.set("bussID", model.BussID);

        graphShapeDetails.push(model);
        currentFeature = null;  //置空当前绘制的几何要素
        geoStr = null; //置空当前绘制图形的geoStr
    }
    else {
        alert("未得到绘制图形几何信息！");
        vectLayer.getSource().removeFeature(currentFeature); //删除当前绘制图形
    }

}



/**
* 将绘制的几何数据与对话框设置的属性数据提交到后台处理
*/
//function submitData() {
//    //  var shapeName = $("#shapeName").val(); //名称
//    var shapeName;
//    var shapeType = document.getElementById('type').value;
//    var features = vectSource.getFeatures();
//    var intersectionGraphMappingItems = [];

//    var intNameList = [];
//    intNameList.push(0);
//    var isPopup = true;//是否弹出提示框
//    var currentFeature1, currentFeature2;

//    if (currentFeature.getGeometry().getType() == "Circle") {
//        currentFeature1 = conversionFeature(currentFeature);
//    } else {
//        currentFeature1 = currentFeature;
//    }

//    if (features.length > 0) {
//        for (var j = 0; j < features.length; j++) {
//            if (currentFeature != features[j]) {
//                if (features[j].getGeometry().getType() == "Circle") {
//                    currentFeature2 = conversionFeature(features[j]);
//                } else {
//                    currentFeature2 = features[j];
//                }
//                if (getInterFeature(currentFeature1, currentFeature2)) {
//                    if (isPopup) {
//                        alert("和已有的项目有交集");
//                        isPopup = false;
//                    }
//                    var model = {};
//                    model.shapeid1 = features[j].get('id');
//                    intersectionGraphMappingItems.push(model);
//                }
//                intNameList.push(parseInt(features[j].get('name')));
//            }
//        }
//    }

//    shapeName = Math.max.apply(null, intNameList) + 1;
//    if (geoStr != null) {
//        var model = {};
//        model.ShapeID = newGuid();
//        model.shapeName = mapNO; //图形名称
//        model.shapeType = shapeType; //图形类型(园 / 矩形 / 曲线)
//        model.shapeData = geoStr; //形状内容,以json文本格式存储
//        model.createTime = constructionDate; //施工时间
//        model.PrintDetailID = printDetailID;
//        model.Version = "1.0.0.0";
//        model.BussID = bussId;
//        model.Color = getColors();
//        model.IntersectionGraphMappingItems = intersectionGraphMappingItems;
//        graphShapeDetails.push(model);
//        currentFeature.set("name", model.shapeName);
//        currentFeature.set("id", model.ShapeID);
//        currentFeature.set("color", model.Color);
//        currentFeature.set("bussID", model.BussID);
//        vectSource.addFeature(currentFeature);
//        //saveData(shapeName, shapeType, createTime, geoStr, intersectionGraphMappingItems); //将数据提交到后台处理（保存到数据库中）
//            currentFeature = null;  //置空当前绘制的几何要素
//            geoStr = null; //置空当前绘制图形的geoStr
//    }
//    else {
//        alert("未得到绘制图形几何信息！");
//        vectLayer.getSource().removeFeature(currentFeature); //删除当前绘制图形
//    }
//}





/**
* 鼠标移动事件监听处理函数（添加热区功能）
*/
function pointermoveFun(e) {
    var pixel = map.getEventPixel(e.originalEvent);
    var hit = map.hasFeatureAtPixel(pixel);
    map.getTargetElement().style.cursor = hit ? 'pointer' : '';//改变鼠标光标状态
    if (hit) {
        //当前鼠标位置选中要素
        var feature = map.forEachFeatureAtPixel(e.pixel,
            function (feature, layer) {
                return feature;
            });
        //如果当前存在热区要素                   
        if (feature) {
            //显示热区图层
            //hotSpotsLayer.setVisible(true);
            //控制添加热区要素的标识（默认为false）
            if (preFeature != null) {
                if (preFeature === feature) {
                    flag = true; //当前鼠标选中要素与前一个选中要素相同
                }
                else {

                    flag = false; //当前鼠标选中要素不是前一个选中要素
                    ////hotSpotsSource.removeFeature(preFeature); //将前一个热区要素移除
                    preFeature = feature; //更新前一个热区要素对象
                }
            }
            //如果当前选中要素与之前选中要素不同，在热区绘制层添加当前要素
            if (!flag) {
                $(element).popover('destroy'); //销毁popup
                flashFeature = feature; //当前热区要素
                flashFeature.setStyle(flashStyle); //设置要素样式
                //hotSpotsSource.addFeature(flashFeature); //添加要素
                //hotSpotsLayer.setVisible(true); //显示热区图层
                preFeature = flashFeature; //更新前一个热区要素对象                           
            }
            //弹出popup显示热区信息
            popup.setPosition(e.coordinate); //设置popup的位置
            $(element).popover({
                placement: 'top',
                html: true,
                content: feature.get('name')
            });
            $(element).css("width", "120px");
            $(element).popover('show'); //显示popup

        }
        else {
            //hotSpotsSource.clear(); //清空热区图层数据源
            flashFeature = null; //置空热区要素
            $(element).popover('destroy'); //销毁popup
            //hotSpotsLayer.setVisible(false); //隐藏热区图层
        }
    }
}

/**
* 提交数据到后台保存
* @param {string} geoData 区几何数据
* @param {string} attData 区属性数据
*/
function saveData(data) {
    //通过ajax请求将数据传到后台文件进行保存处理


    $.ajax({
        url: insertShapeUrl, //请求地址
        type: 'POST', //请求方式为post
        data: { strData: jsonToStr(data) }, //传入参数 
        dataType: 'text', //返回数据格式
        //请求成功完成后要执行的方法  
        success: function (response) {
            selectRegData(); //查询数据库中热区要素实现热区功能
        },
        error: function (err) {
            alert("执行失败");
        }
    });
}


// 初始化绘制热区要素信息设置对话框
function OpenInsertConfirm() {
    var dlg = $("#insertConfirm");
    dlg.css("display", "block");
    dlg.dialog({
        closed: false,
        showType: null,
        modal: true,
        buttons: [{
            text: '确定',
            iconCls: 'icon-add',
            handler: function () {
                submitData(); //提交几何与属性信息到后台处理
                $('#insertConfirm').dialog({ closed: true });

            }
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#insertConfirm').dialog({ closed: true });
                vectLayer.getSource().removeFeature(currentFeature); //删除当前绘制图形
            }
        }]
    });
}

/**
* 鼠标单击事件监听处理函数
*/
function singleclickFun(e) {
    var pixel = map.getEventPixel(e.originalEvent);
    var hit = map.hasFeatureAtPixel(pixel);
    map.getTargetElement().style.cursor = hit ? 'pointer' : '';
    //当前鼠标位置选中要素
    var feature = map.forEachFeatureAtPixel(e.pixel,
        function (feature, layer) {
            return feature;
        });
    //如果当前存在热区要素                   
    if (feature) {
        debugger;
        OpenUserDialog();
        //$("#dialog-delete").dialog("open"); //打开删除要素设置对话框
        currentFeature = feature; //当前绘制的要素
    }
}

/**
* 通过后台删除热区要素
*/
function deleteData(feature) {
    var ShapeID = feature.get('id'); //要素的
    $.each(graphShapeDetails,
        function (index, item) {
            if (item.ShapeID == ShapeID) {
                graphShapeDetails.splice(index, 1);
            }
        });
    vectLayer.getSource().removeFeature(currentFeature); //删除当前选中热区要素
    //通过ajax请求将数据传到后台文件进行删除处理

    //$.ajax({
    //    url: deleteShapeUrl, //请求地址
    //    type: 'POST',  //请求方式为post
    //    data: { 'ShapeID': ShapeID }, //传入参数 
    //    dataType: 'text', //返回数据格式
    //    //请求成功完成后要执行的方法  
    //    success: function (response) {
    //        // alert(response); //提示删除成功
    //        vectLayer.getSource().removeFeature(currentFeature); //删除当前选中热区要素
    //    },
    //    error: function (err) {
    //        alert("执行失败");
    //    }
    //});
}

// 初始化删除要素信息设置对话框
function OpenUserDialog() {
    var dlg = $("#ContractCharacter");
    dlg.css("display", "block");
    dlg.dialog({
        closed: false,
        showType: null,
        modal: true,
        buttons: [{
            text: '确定',
            iconCls: 'icon-add',
            handler: function () {
                deleteData(currentFeature);  //通过后台删除数据库中的热区要素数据并同时删除前端绘图  
                CloseGoodsDialog();

            }
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                CloseGoodsDialog(); //关闭对话框
            }
        }]
    });
}

function CloseGoodsDialog() {
    $('#ContractCharacter').dialog({ closed: true });
}

/**
* 【显示热区】功能按钮处理函数
*/
//function showReg() {
//    map.un('pointermove', pointermoveFun, this); //移除鼠标移动事件监听
//    selectRegData(); //通过后台查询热区要素显示并实现热区功能
//};


/**
* 【删除热区】功能按钮处理函数
*/

function deleteReg() {
    map.un('pointermove', pointermoveFun, this); //移除鼠标移动事件监听
    map.un('singleclick', singleclickFun, this); //移除鼠标单击事件监听
    map.on('singleclick', singleclickFun, this); //添加鼠标单击事件监听
};



function getInterFeature(f1, f2) {
    //参数f1 ,f2  为 要判断的多边形feature
    var r1 = getRings(f1);
    var r2 = getRings(f2);
    //根据网上的写的自己写的方法
    var istrue = intersectsPolygonAndPolygon(r1, r2);
    return istrue;

}


var point = []; //结果
/*
* 求圆周上等分点的坐标
* ox,oy为圆心坐标
* r为半径
* count为等分个数
*/
function getPoints(r, ox, oy, count) {
    var radians = (Math.PI / 180) * Math.round(360 / count), //弧度
        i = 0;
    for (; i < count; i++) {
        var x = ox + r * Math.sin(radians * i),
            y = oy + r * Math.cos(radians * i);

        point.unshift({ x: x, y: y }); //为保持数据顺时针
    }
}
/*
* 将圆坐标转化为千正多边形
* circleFeature为圆特征
* 返回polygonFeature为千多边形特征
*/

function conversionFeature(circleFeature) {
    var r = circleFeature.getGeometry().getRadius();
    var x = circleFeature.getGeometry().getCenter()[0];
    var y = circleFeature.getGeometry().getCenter()[1];
    getPoints(r, x, y, 80);
    var coordinates = new Array();
    coordinates[0] = new Array();
    for (var i = 0; i < point.length; i++) {
        coordinates[0][i] = [point[i].x.toString(), point[i].y.toString()];
    }
    var polygonFeature = new ol.Feature({
        geometry: new ol.geom.Polygon(coordinates)
    });
    point = [];
    return polygonFeature;
}


function getColors() {
    var result;
    var selectChecked;
    if (bussId.substr(0, 2) == 'NS') {
        selectChecked = $('input[name="IsMajorHazard"]:checked').val() == "N" ? "Z" : "D";
    } else {
        selectChecked = $('input[name="SGType"]:checked').val();
    }
    switch (selectChecked) {
        case "B":
            result = "green";
            break;
        case "C":
            result = "orange";
            break;
        case "D":
            result = "purple";
            break;
        default:
            result = "blue";
    }
    return result;
}


function intersection() {
    var features = vectSource.getFeatures();
    var isPopup = false;//是否弹出提示框
    var currentFeature1, currentFeature2;
    debugger
    for (var i = 0; i < graphShapeDetails.length; i++) {
        var intersectionGraphMappingItems = [];
        $.each(features,
                function (index, item) {
                    if (item.get('id') == graphShapeDetails[i].ShapeID) {
                        currentFeature = item;
                    }
                });
        features.splice($.inArray(currentFeature, features), 1);
        if (features.length > 0) {
            if (currentFeature.getGeometry().getType() == "Circle") {
                currentFeature1 = conversionFeature(currentFeature);
            } else {
                currentFeature1 = currentFeature;
            }
            for (var j = 0; j < features.length; j++) {
                if (features[j].getGeometry().getType() == "Circle") {
                    currentFeature2 = conversionFeature(features[j]);
                } else {
                    currentFeature2 = features[j];
                }
                if (getInterFeature(currentFeature1, currentFeature2)) {
                    //if (isPopup) {
                    //    //alert("和已有的项目有交集");
                    //    isPopup = false;
                    //}
                    isPopup = true;
                    var model = {};
                    model.shapeid1 = graphShapeDetails[i].ShapeID;
                    model.shapeid2 = features[j].get('id');
                    intersectionGraphMappingItems.push(model);
                }
                graphShapeDetails[i].intersectionGraphMappingItems = intersectionGraphMappingItems;
            }
        }
    }


    return isPopup;
}