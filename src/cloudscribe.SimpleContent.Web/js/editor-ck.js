(function ($) {

    var xsrfToken = $("#__RequestVerificationToken").val();
    var dfUrl = $("#editorconfig").data("dropfileuploadurl") || '/filemanager/upload';
    var fbUrl = $("#editorconfig").data("filebrowserurl") || '/filemanager/ckfiledialog?type=file';
    var ibUrl = $("#editorconfig").data("imagebrowseurl") || '/filemanager/ckfiledialog?type=image';
    var editorId = $("#editorconfig").data("editorid") || 'foo';
    var datepickerid = $("#editorconfig").data("datepickerid") || 'foo';
    var usingCdn = $("#editorconfig").data("usingcdn");

    var editorConfig = {
        toolbar: [['Source', 'Maximize'],
    ['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print'],
    ['Undo', 'Redo', '-', 'Find', 'Replace', 'Bold', 'Italic', 'Underline', '-', 'Strike', 'Superscript'],
        '/',
    ['Blockquote'], ['NumberedList', 'BulletedList'],
    ['Link', 'Unlink', 'Anchor'],
    ['Image', 'oembed', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar']],
        extraPlugins:'oembed,cloudscribe-filedrop',
        removePlugins:'scayt,wsc',
        dropFileUploadUrl: dfUrl,
        dropFileXsrfToken:xsrfToken,
        linkWebSizeToOriginal:true,
        forcePasteAsPlainText:true,
        filebrowserWindowHeight:'70%',
        filebrowserWindowWidth:'80%',
        filebrowserBrowseUrl:fbUrl,
        filebrowserImageBrowseUrl: ibUrl
    };

    if (usingCdn === true) {
        //alert('using cdn');
        CKEDITOR.plugins.addExternal('widget', '/ckjs/plugins/widget/', 'plugin.js');
        CKEDITOR.plugins.addExternal('widgetselection', '/ckjs/plugins/widgetselection/', 'plugin.js');
        CKEDITOR.plugins.addExternal('lineutils', '/ckjs/plugins/lineutils/', 'plugin.js');
        CKEDITOR.plugins.addExternal('oembed', '/ckjs/plugins/oembed/', 'plugin.js');
        CKEDITOR.plugins.addExternal('cloudscribe-filedrop', '/ckjs/plugins/cloudscribe-filedrop/', 'plugin.js');

    }
    

    var ck = CKEDITOR.replace(editorId, editorConfig);

    var userLocale = $('#' + datepickerid).data("locale");
    $('#' + datepickerid).datetimepicker({
        debug: false,
        widgetPositioning: { horizontal: 'left', vertical: 'bottom' },
        keepOpen: true,
        allowInputToggle: true,
        locale: userLocale
    });

})(jQuery);