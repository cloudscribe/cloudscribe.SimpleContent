(function ($) {

    var xsrfToken = $("#__RequestVerificationToken").val();
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
        dropFileUploadUrl:"/filemanager/upload",
        dropFileXsrfToken:xsrfToken,
        linkWebSizeToOriginal:true,
        forcePasteAsPlainText:true,
        filebrowserWindowHeight:'70%',
        filebrowserWindowWidth:'80%',
        filebrowserBrowseUrl:'/filemanager/ckfiledialog?type=file',
        filebrowserImageBrowseUrl:'/filemanager/ckfiledialog?type=image'
    };

    CKEDITOR.plugins.addExternal('widget', '/js/ckeditor461/plugins/widget/', 'plugin.js');
    CKEDITOR.plugins.addExternal('widgetselection', '/js/ckeditor461/plugins/widgetselection/', 'plugin.js');
    CKEDITOR.plugins.addExternal('lineutils', '/js/ckeditor461/plugins/lineutils/', 'plugin.js');

    CKEDITOR.plugins.addExternal('oembed', '/js/ckeditor461/plugins/oembed/', 'plugin.js');
    CKEDITOR.plugins.addExternal('cloudscribe-filedrop', '/js/ckeditor461/plugins/cloudscribe-filedrop/', 'plugin.js');

    var ck = CKEDITOR.replace('Content', editorConfig);

    var userLocale = $("#PubDate").data("locale");
    $("#PubDate").datetimepicker({
        debug: false,
        widgetPositioning: { horizontal: 'left', vertical: 'bottom' },
        keepOpen: true,
        allowInputToggle: true,
        locale: userLocale
    });

})(jQuery);