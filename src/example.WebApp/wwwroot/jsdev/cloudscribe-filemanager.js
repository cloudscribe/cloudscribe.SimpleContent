(function () {
    var fileManager = {
        treeDataApiUrl: $("#config").data("filetree-url"),
        uploadApiUrl: $("#config").data("upload-url"),
        createFolderApiUrl: $("#config").data("create-folder-url"),
        fileSelectorButton: $('#btnSelector'),
        selectedFileInput: $("#fileSelection"),
        newFolderButton: $('#btnCreateFolder'),
        progressUI: $('#progress'),
        cropTab: $('#tab3'),
        treeData: [],
        selectedFileList: [],
        setPreview: function (url) {
            $("#filePreview").attr("src", url);
            $("#fileCropPreview").attr("src", url);

        },
        setCurrentDirectory: function (virtualPath) {
            $("#newFolderCurrentDir").val(virtualPath);
            $("#hdnCurrentVirtualPath").val(virtualPath);
            $("#uploadCurrentDir").val(virtualPath);
            $("#currentFolder").html(virtualPath);

        },
        notify: function (message, cssClass) {
            $('#alert_placeholder').html('<div class="alert ' + cssClass + '"><a class="close" data-dismiss="alert">×</a><span>' + message + '</span></div>')
        },
        addFileToList: function (data, fileList, index, file) {
            var d = $("<span class='fa fa-trash-o' aria-role='button' title='Remove'></span>").click(function () {
                data.files.splice(index, 1);
                fileList = data.files;
                $('#fileList li').eq(index).remove();
                if (fileList.length === 0) {
                    $('#fileList').html('');
                }
            });
            var item = $("<li>", { text: file.name }).append("&nbsp;").append(d);
            $('#fileList ul').append(item);
        },
        addErrorToList: function (index, file) {
            var item = $("<li>", { text: file.ErrorMessage });
            $('#fileList ul').append(item);
        },

        createFolder: function () {
            var formData = $('#frmNewFolder').serializeArray();
            //alert(JSON.stringify(formData));
            $.ajax({
                method: "POST",
                url: fileManager.createFolderApiUrl,
                data: formData
            }).done(function (data) {
                // alert(JSON.stringify(data));
                if (data.succeeded) {
                    fileManager.reloadSubTree();
                    $("#newFolderName").val('');
                    //fileManager.notify('Folder created', 'alert-success');
                }
                else {
                    fileManager.notify(data.message, 'alert-danger');

                }

            })
            .fail(function () {
                fileManager.notify('An error occured', 'alert-danger');
            });

            return false; //cancel form submit
        },
        ckReturnFile: function () {
            var funcNum = '@Model.CKEditorFuncNum';
            var fileUrl = fileManager.selectedFileInput.val();
            if (fileUrl.length === 0) {
                fileManager.notify('Please select a file in the browse tab', 'alert-danger');
            }
            else {
                window.opener.CKEDITOR.tools.callFunction(funcNum, fileUrl);
                window.close();
            }
        },
        reloadSubTree: function () {
            var tree = $('#tree').treeview(true);
            var currentFolderId = $("#uploadCurrentDir").val();
            //alert(currentFolderId);
            var matchingNodes = tree.findNodes(currentFolderId, 'id');
            if (matchingNodes.length > 0) {
                tree.collapseNode(matchingNodes, { silent: true, ignoreChildren: false });
                var theNode = matchingNodes[0];
                //alert(JSON.stringify(theNode));
                //alert(theNode.id)
                var newNode = {
                    text: theNode.text,
                    id: theNode.id,
                    type: theNode.type,
                    icon: theNode.icon,
                    expandedIcon: theNode.expandedIcon,
                    virtualPath: theNode.virtualPath,
                    nodes: [],
                    lazyLoad: true //this makes it load child nodes on expand
                };
                tree.updateNode(theNode, newNode, { silent: true });
                matchingNodes = tree.findNodes(currentFolderId, 'id');
                tree.expandNode(matchingNodes, { silent: true, ignoreChildren: false });

            }
            else {
                alert('node not found');
            }
        },
        loadTree: function () {
            $('#tree').treeview({
                dataUrl: {
                    method: 'GET',
                    dataType: 'json',
                    url: fileManager.treeDataApiUrl,
                    cache: false
                },
                nodeIcon: 'fa fa-folder',
                //selectedIcon: null,
                collapseIcon: 'fa fa-minus',
                emptyIcon: 'fa',
                expandIcon: 'fa fa-plus',
                loadingIcon: 'fa fa-hourglass-o',
                levels: 2,
                onhoverColor: '#F5F5F5',
                highlightSelected: true,
                showBorder: true,
                showCheckbox: false,
                showIcon: true,
                wrapNodeText: false,
                lazyLoad: function (node, dataFunc) {
                    //alert(node.text + ' lazyload');
                    $.ajax({
                        dataType: "json",
                        url: fileManager.treeDataApiUrl + '?virtualStartPath=' + node.virtualPath

                    })
                      .done(function (data) {
                          dataFunc(data);
                      })
                    ;

                },
                onNodeSelected: function (event, data) {
                    //alert(data.virtualPath + ' selected');
                    if (data.canPreview) {
                        fileManager.setPreview(data.virtualPath);
                        fileManager.cropTab.show();

                    }
                    else {
                        fileManager.cropTab.hide();
                    }
                    if (data.type === "d") {
                        fileManager.setCurrentDirectory(data.virtualPath);

                        // alert(uploadCurrentDir.val());

                    }
                    else {
                        fileManager.selectedFileInput.val(data.virtualPath);
                    }
                },
                onNodeExpanded: function (event, node) {
                    //if (node.type === "d") {
                    //    node.icon = "fa fa-folder-open-o";
                    //    node.selectedIcon = "fa fa-folder-open-o";
                    //   // alert('expanded');
                    //}
                },
                onNodeCollapsed: function (event, node) {
                    //if (node.type === "d") {
                    //    node.icon = "fa fa-folder-o";
                    //    node.selectedIcon = "fa fa-folder-o";
                    //}

                }
            });

        },
        setupFileLoader: function () {
            $('#pnlFiles').fileupload({
                fileInput: $('#fileupload'),
                url: fileManager.uploadApiUrl,
                dataType: 'json',
                autoUpload: false,
                singleFileUploads: false,
                limitMultiFileUploads: 10,
                limitConcurrentUploads: 10,
                dropZone: $('#dropZone'),
                pasteZone: $('#dropZone'),
                add: function (e, data) {
                    $('#fileList').html('');
                    $('#fileList').append($("<ul class='filelist'></ul>"));
                    var regx = allowedFilesRegex;
                    var j = 0;
                    var k = data.files.length;
                    //alert(k);
                    while (j < k) {
                        if ((regx.test(data.files[j].name)) === false) {
                            //alert('false');
                            data.files.splice(j, 1);
                            k = data.files.length;
                            j = -1;
                        }
                        j++;
                    }
                    fileManager.selectedFileList = fileManager.selectedFileList.concat(data.files);
                    var maxAllowed = 10;
                    while (fileManager.selectedFileList.length > maxAllowed) {
                        fileManager.selectedFileList.pop();
                    }
                    data.files = fileManager.selectedFileList;
                    if (data.files.length > 0) {
                        var btnSend = $("<button id='btnSend' class='btn btn-success'><i class='fa fa-cloud-upload' aria-hidden='true'></i> Upload</button>");
                        btnSend.appendTo($('#fileList'));
                    }
                    $.each(data.files, function (index, file) { fileManager.addFileToList(data, fileManager.selectedFileList, index, file); });
                    $('#btnSend').click(function () {
                        data.context = $('<p/>').text('Uploading...').replaceAll($(this));
                        data.submit();
                    });
                },
                done: function (e, data) {
                    $('#progress').hide();
                    $('#fileList').html('');
                    fileListuploader = [];
                    $('#fileList').append($("<ul class='filelist file-errors'></ul>"));
                    var j = 0;
                    var errorsOccurred = false;
                    while (j < data.length) {
                        if (data[j].errorMessage) {
                            errorsOccurred = true;
                            addErrorToList(j, data[j]);
                        }
                        j++;
                    }

                    fileManager.reloadSubTree();
                },
                progressall: function (e, data) {
                    var progress = parseInt(data.loaded / data.total * 100, 10);
                    fileManager.progressUI.show();
                    $('#progress .progress-bar').css(
                        'width',
                        progress + '%'
                    );
                }

            }).prop('disabled', !$.support.fileInput)
              .parent().addClass($.support.fileInput ? undefined : 'disabled');

            $('#fileupload').bind('fileuploadsubmit', function (e, data) {
                data.formData = $('#frmUpload').serializeArray();
                //alert(data.formData);
                return true;
            });

        },
        init: function () {
            $(document).bind('drop dragover', function (e) { e.preventDefault(); });
            this.progressUI.hide();
            this.cropTab.hide();
            this.loadTree();
            this.setupFileLoader();
            this.newFolderButton.on('click', fileManager.createFolder);
            this.fileSelectorButton.on('click', fileManager.ckReturnFile);


        }


    };

    fileManager.init();

})();
