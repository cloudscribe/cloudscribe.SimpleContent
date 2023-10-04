﻿// Author: J Audette 2017-11-19
$(function () {
    function IsAllowedFile(file) {
        switch (file.type) {
            case "image/jpeg":
            case "image/jpg":
            case "image/gif":
            case "image/png":
                return true;
                break;
        }
        return false;
    }
    var $elems = $('textarea[data-markdown-unobtrusive]');
    if ($elems) {
        $elems.each(function (index, ele) {
            var config = {};
            config.element = ele;
            config.autoDownloadFontAwesome = $(ele).data('markdown-autoDownloadFontAwesome');
            var autosave = {};
            autosave.enabled = ($(ele).data('markdown-autosave') || "false");
            autosave.uniqueId = $(ele).data('markdown-contentid');
            //console.log(autosave.uniqueId);
            config.autosave = autosave;
            config.forceSync = ($(ele).data('markdown-forceSync') || "false");
            config.indentWithTabs = ($(ele).data('markdown-indentWithTabs') || "true");
            config.spellChecker = ($(ele).data('markdown-spellChecker') || "true");
            var dropfileuploadurl = $(ele).data('markdown-dropfileuploadurl');
            var dropFileXsrfToken = $('[name="__RequestVerificationToken"]:first').val();

            var simplemde = new SimpleMDE(config);
            //var imgButton = simplemde.toolbar[9]; 
            //if (imgButton) {
            //    //alert(imgButton.name);
            //    imgButton.action = function customFunction(editor) {
            //       // Add your own code
            //            alert('you clicked image');
            //        };
            //}
            //simplemde.toolbar[16].action = function customFunction(editor) {
            //    // Add your own code
            //    alert('you clicked image');
            //};
            if (dropfileuploadurl) {
                simplemde.codemirror.on("drop", function (instance, event) {
                    //console.log("drop fired");
                    var files = event.dataTransfer.files || event.target.files;
                    if (files) {
                        event.preventDefault();
                        event.stopPropagation();
                        if (IsAllowedFile(files[0])) {
                            //console.log(files[0].name);
                            var formData = new FormData();
                            formData.append("__RequestVerificationToken", dropFileXsrfToken);
                            formData.append(files[0].name, files[0]);
                            $.ajax({
                                type: "POST",
                                processData: false,
                                contentType: false,
                                dataType: "json",
                                url: dropfileuploadurl,
                                data: formData,
                                success: function (data) {
                                    if (data[0].resizedUrl) {
                                        var md = "[![](" + data[0].resizedUrl + ")](" + data[0].originalUrl + ")";
                                        instance.replaceSelection(md);
                                    }
                                }
                            });
                        }   
                    }
                });
            } 
        });
    }
});