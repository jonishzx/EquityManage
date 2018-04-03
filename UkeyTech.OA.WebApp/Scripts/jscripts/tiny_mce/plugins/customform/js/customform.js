tinyMCEPopup.requireLangPack();

function checkHasSelectedFields(content) {
    //check which fields has selected
    var doc = $(content);
    var fields = $(doc).find("span.DataField");

    var fieldcontent = '';
    $.each(fields, function () {
        fieldcontent += $(this).clone().wrap('<div>').parent().html();
    });

    $("#hasSelected").html(fieldcontent);
}

function LoadCanSelectedDataFields() {
    //新建数据表
    var url = tinyMCEPopup.getWindowArg('baseurl') + '/GetCustomFormVisibleColumnList' + '? t=' + new Date().toLocaleDateString();
    var formid = tinyMCEPopup.getWindowArg('formid');
    var template = "<span  ColCaption='{0}' ColType='{1}' ColName='{2}' SelectColType='{3}' SelectTypeId='{4}' {5} class='DataField'>{{0}}</span>";
    var output = '';
    $.ajax({
        type: "GET",
        url: url,
        data: { FormID: formid },
        success: function (msg) {
            //load exits fields
            var content = tinyMCEPopup.editor.getContent();
            checkHasSelectedFields(content);
            var exitsFields = $("#hasSelected > .DataField");

            //get server fields
            var json = eval('(' + msg + ")");

            $("#selectFields").html('');
            //check fields
            var i = 0; var exist = false;
            $.each(json.rows, function () {

                for (i = 0; i < exitsFields.length; i++) {
                    if ($(exitsFields[i]).attr("ColName") == this.ColName) {
                        exist = true
                        break;
                    }
                }

                if (!exist) {
                    output = output + template.replace(/(\{0\})/g, this.ColCaption)
                                    .replace(/(\{1\})/g, this.ColType)
                                    .replace(/(\{2\})/g, this.ColName)
                                    .replace(/(\{3\})/g, this.SelectColType)
                                    .replace(/(\{4\})/g, this.SelectTypeId)
                                    .replace(/(\{5\})/g, this.Required == 1 ? "required" : "");
                }

                 exist = false;
            });

            $("#selectFields").html(output);

            $("#selectFields > .DataField").click(function () {
                $("#currentField").html($(this).clone());
            });
            delete output;
            delete json;
        }
    });
}

var CustomFormDialog = {
    init: function () {
        //init
        document.getElementById('FormName').innerHTML = tinyMCEPopup.getWindowArg('formname');
      
        //load columns
        LoadCanSelectedDataFields();
    },

    insert: function () {

        //get the field and insert
        var ele = $("#currentField").html();
     
        tinyMCEPopup.editor.execCommand('mceInsertContent', false, ele);
        tinyMCEPopup.close();
    }
};

tinyMCEPopup.onInit.add(CustomFormDialog.init, CustomFormDialog);
