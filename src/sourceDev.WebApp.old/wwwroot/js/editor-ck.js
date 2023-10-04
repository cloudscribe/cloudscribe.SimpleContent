(function ($) {

    var xsrfToken = $('[name="__RequestVerificationToken"]:first').val();
    //alert(xsrfToken);
    var dfUrl = $("#editorconfig").data("dropfileuploadurl") || '/filemanager/upload';
    var fbUrl = $("#editorconfig").data("filebrowserurl") || '/filemanager/filedialog?type=file';
    var ibUrl = $("#editorconfig").data("imagebrowseurl") || '/filemanager/filedialog?type=image';
    var editorId = $("#editorconfig").data("editorid") || 'foo';
    var datepickerid = $("#editorconfig").data("datepickerid") || 'foo';
    var usingCdn = $("#editorconfig").data("usingcdn");

    var editorConfig = {
        toolbar: [['Sourcedialog', 'Maximize'],
    ['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print'],
    ['Undo', 'Redo', '-', 'Find', 'Replace', 'Bold', 'Italic', 'Underline', '-', 'Strike', 'Superscript'],
        '/',
        ['Blockquote', 'Format'], ['NumberedList', 'BulletedList'],
    ['Link', 'Unlink', 'Anchor'],
    ['Image', 'oembed', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar','CodeSnippet']],
        extraPlugins:'oembed,cloudscribe-filedrop,sourcedialog,codesnippet',
        removePlugins: 'scayt,wsc',
        format_tags: 'p;h1;h2;h3;h4;pre;address;div',
        dropFileUploadUrl: dfUrl,
        dropFileXsrfToken:xsrfToken,
        linkWebSizeToOriginal:true,
        forcePasteAsPlainText:true,
        filebrowserWindowHeight:'70%',
        filebrowserWindowWidth:'80%',
        filebrowserBrowseUrl:fbUrl,
        filebrowserImageBrowseUrl: ibUrl,
        //basicEntities: false,
        //htmlEncodeOutput : false,
        allowedContent : true, //temporary trying to disable filtering
        extraAllowedContent: 'div(*){*}[*]; ol li span a(*){*}[*]', // allow all classes and attributes for these tags
        fillEmptyBlocks: false
    };

    if (usingCdn === true) {
        //alert('using cdn');
        CKEDITOR.plugins.addExternal('widget', '/ckjs/plugins/widget/', 'plugin.js');
        CKEDITOR.plugins.addExternal('widgetselection', '/ckjs/plugins/widgetselection/', 'plugin.js');
        CKEDITOR.plugins.addExternal('lineutils', '/ckjs/plugins/lineutils/', 'plugin.js');
        CKEDITOR.plugins.addExternal('oembed', '/ckjs/plugins/oembed/', 'plugin.js');
        CKEDITOR.plugins.addExternal('cloudscribe-filedrop', '/ckjs/plugins/cloudscribe-filedrop/', 'plugin.js');

    }

    //editorConfig.protectedSource.push(/<div[^>]*><\/div>/g);
    //CKEDITOR.dtd.$removeEmpty['div'] = false;

    //$.each(CKEDITOR.dtd.$removeEmpty, function (i, value) {
    //    CKEDITOR.dtd.$removeEmpty[i] = 0;
    //});
    
    //CKEDITOR.disableAutoInline = true;
    var ck = CKEDITOR.replace(editorId, editorConfig);
    //ck.on('mode', function (ev) {
    //    var editor = ev.editor;
    //    editor.setReadOnly(false);
    //    alert('mode');
    //});
    ck.on('instanceCreated', function (ev) {
        CKEDITOR.dtd.$removeEmpty['div'] = false;
    });


    var userLocale = $('#' + datepickerid).data("locale");
    $('#' + datepickerid).datetimepicker({
        debug: false,
        widgetPositioning: { horizontal: 'left', vertical: 'bottom' },
        keepOpen: true,
        allowInputToggle: true,
        locale: userLocale
    });

})(jQuery);