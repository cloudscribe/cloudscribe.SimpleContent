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
        extraPlugins: 'oembed,cloudscribe-filedrop',
        removePlugins: 'scayt,wsc',
        dropFileUploadUrl: "/filemanager/upload",
        dropFileXsrfToken: xsrfToken,
        linkWebSizeToOriginal: true,
        filebrowserBrowseUrl: '/filemanager/ckfiledialog?type=file',
        filebrowserImageBrowseUrl: '/filemanager/ckfiledialog?type=image'
    };

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