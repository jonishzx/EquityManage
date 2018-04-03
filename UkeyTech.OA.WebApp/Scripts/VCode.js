//set validcode
var checkcode = "";
$(document).ready(function(e) {
    var hasshowimg = false;
   
    $('.imgVCode').bind("click", function() {
        $('.imgVCode').attr("src", "../ajax/ValidCode.ashx?t=" + new Date().getTime());

        hasshowimg = true;

        $.get("../ajax/GetSessionCode.ashx?t=" + new Date().getTime(), function(data) {
             $('.imgVCode').parent().find(".hidVCode").val(data);
            checkcode = data;

        });
    });

//    $('.VCode').bind("focus", function() {
//        if (!hasshowimg) {
//            $('.imgVCode').attr("src", "../ajax/ValidCode.ashx");

//            hasshowimg = true;

//            $.get("../ajax/GetSessionCode.ashx?t=" + new Date().getTime(), function(data) {
//                $('.imgVCode').parent().find(".hidVCode").val(data);
//                checkcode = data;

//            });
//        }
//    });

    
    //valid method
//    $.validator.addMethod("checkVCode", function(value) {
//        return value == checkcode;
//    }, '验证吗不正确，请重新输入');

});