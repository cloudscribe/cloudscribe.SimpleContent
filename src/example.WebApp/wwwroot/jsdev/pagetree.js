$(function () {


    var $tree = $('#tree1');
    var $selPageId = $('#hdnSelPage');
    var $cmdBar = $('#ulCommands');
    var $pageLabel = $('#liInfo');
    var $liEdit = $('#liEdit');
    var $liSettings = $('#liSettings');
    var $liPermissions = $('#liPermissions');
    var $liView = $('#liView');
    var $liSort = $('#liSort');
    var $liNewChild = $('#liNewChild');
    var $liDeletePage = $('#liDeletePage');

    var $btnEdit = $('#lnkEdit');
    //var $btnSettings = $('#lnkSettings');
    //var $btnPermissions = $('#lnkPermissions');
    var $btnView = $('#lnkView');
    var $btnSort = $('#lnkSort');
    var $btnNewPage = $('#lnkNewChild');
    var $btnDelete = $('#lnkDeletePage');
    var $spnPermissions = $('#spnPermissions');

    var $serviceUrl = $('#config').data("service-url");
    var $sortUrl = $('#config').data("sort-url");
    var $moveUrl = $('#config').data("move-url");
    var $deleteUrl = $('#config').data("delete-url");
    //var $viewUrl = $('#config').data("view-url");
    var $editUrl = $('#config').data("edit-url");


    var $showCommands = function (node) {

        $selPageId.val(node.id);
        $pageLabel.html(node.name);

        
        $btnEdit.attr('href', $editUrl + '/' + node.slug);
        //$url = '" + SiteRoot + "/Admin/PageSettings.aspx?pageid=' + node.id;
        //$btnSettings.attr('href', $url);
        $btnView.attr('href', node.url);
        //$url = '" + SiteRoot + "/Admin/PageSettings.aspx?start=' + $selPageId.val();
        $btnNewPage.attr('href', $editUrl + "?parentslug=" + node.slug);


        //$url = '" + SiteRoot + "/Admin/PagePermission.aspx?pageid=' + $selPageId.val() + '&p=v';
        //$btnPermissions.attr('href', $url);


        $cmdBar.show();
        var $liActive = $('li.jqtree-selected').last();
        //alert($liActive);

        //$cmdBar.position({
        //    my: "left top", at: "right+10 top",
        //    of: $liActive
        //    , collision: "none"
        //});
        new Tether({
            element: '.treecommands',
            target: '.jqtree-selected',
            attachment: 'top left',
            targetAttachment: 'top right'
        });

        if (node.childcount > 1) {
            $liSort.show();
        } else {
            $liSort.hide();
        }; // end if(node.childcount > 1)

        if (node.canEdit) {
            $btnEdit.show();
            //$btnSettings.show();
            //if (canEditAnything)
            //{
            $liPermissions.show();
            $spnPermissions.html(node.protection);
            //}
            //else
            //{
            //    script.Append("$liPermissions.hide();");
            //}

        } else {
            $btnEdit.hide();
            //$btnSettings.hide();
            //$liPermissions.hide();
        };  // end CanEdit

        if (node.canDelete) {
            $btnDelete.show();
        } else {
            $btnDelete.hide();
        };  // end CanEdit

        if (node.canCreateChild) {
            $btnNewPage.show();
        } else {
            $btnNewPage.hide();
        }; // end CanEdit


    };  // end showCommands

    var $moveNode = function (movedNode, targetNode, previousParent, position) {

        //alert('you moved ' + movedNode.id);

        //post to server and get result

        var obj = {};
        obj['movedNode'] = movedNode.id;
        obj['targetNode'] = targetNode.id;
        obj['previousParent'] = previousParent.id;
        obj['position'] = position;

        var moveResult = false;

        $.ajax({
            type: "POST",
            async: false,
            //processData: true,
            contentType: 'application/json; charset=utf-8',

            dataType: "json",

            //string moveUrl = SiteRoot + "/Services/SiteMapJson.ashx?cmd=move";

            url: $moveUrl,
            data: obj,
            success: function (data, textStatus, jqXHR) {
                if (data.Success) { moveResult = true; } else {
                    alert(data.Message);
                }

            },  //end success
            complete: function (jqXHR, textStatus) {



            },  //end complete

            error: function (jqXHR, textStatus, errorThrown) {

                alert(errorThrown);

            } //end error
        }); //end ajax

        return moveResult;
    }; // end moveNode


    $btnSort.click(function (e) {


        e.preventDefault();

        //if (promptOnSort)
        //{
        //    script.Append("if (confirm('" + HttpUtility.HtmlAttributeEncode(PageManagerResources.SortAlphaPrompt) + "')) {");
        //}

        var node = $tree.tree('getNodeById', $selPageId.val());
        var objSort = {};
        objSort['pageId'] = node.id;

        $.ajax({
            type: "POST",
            async: false,
            processData: true,
            //contentType: 'application/json; charset=utf-8',

            dataType: "json",

            //string sortUrl = SiteRoot + "/Services/SiteMapJson.ashx?cmd=sortalpha";

            url: $sortUrl,
            data: objSort,
            success: function (data, textStatus, jqXHR) {
                if (data.Success) {

                    // reload the node
                    $('#tree1').tree('loadDataFromUrl', node);

                } else {
                    alert(data.Message);
                }

            },  //end success
            complete: function (jqXHR, textStatus) {

            },  //end complete

            error: function (jqXHR, textStatus, errorThrown) {

                alert(errorThrown);

            } //end error
        }); //end ajax



        //if (promptOnSort)
        //{
        //    script.Append("} "); //end if confirm
        //}



    });



    $btnDelete.click(function (e) {
        e.preventDefault();
        var node = $tree.tree('getNodeById', $selPageId.val());
        //if (promptOnDelete)
        //{
        var doDelete = false;
        if (node.childcount > 0) {
            if (confirm("Are you sure you want to delete the page and all of it's child pages?")) {
                doDelete = true;
            }
            

            } else {

            if (confirm("Are you sure you want to delete the page?")) {
            doDelete = true;
            } 


        } //end else nodecount
        //}
        //else
        //{
        //    script.Append("var doDelete = true; ");
        //}


        if (doDelete) {
            //alert('about to delete page ' + node.id);

            var objDel = {};
            objDel['id'] = node.id;

            $.ajax({
                type: "POST",
                async: false,
                processData: true,
                //script.Append("contentType: false,");
                dataType: "json",

               

                url: $deleteUrl,
                data: objDel,
                success: function (data, textStatus, jqXHR) {
                    if (data.Success) {

                        // remove the node from the tree
                        $('#tree1').tree('removeNode', node);

                    } else {
                        alert(data.Message);
                    }

                }, //end success
                complete: function (jqXHR, textStatus) {



                },  //end complete

                error: function (jqXHR, textStatus, errorThrown) {

                    alert(errorThrown);

                }  //end error
            }); //end ajax


        }

    });


    $('#tree1').tree({

        dataUrl: $serviceUrl
        , dragAndDrop: true
        , onLoadFailed: function (response) {
            alert(response);
        }



    }); // end tree




    $('#tree1').bind(
        'tree.click',
        function (event) {

            var node = event.node;


        }
    ); // end bind tree.click


    $('#tree1').bind(
        'tree.dblclick',
        function (event) {
            console.log(e.node);
        }
    ); // end bind tree.dblclick


    $('#tree1').bind(
        'tree.select',
        function (event) {
            if (event.node) {

                $showCommands(event.node);

            } else {
                // event.node is null
                // a node was deselected
                // e.previous_node contains the deselected node
                var node = event.previous_node;
                $selPageId.val(-1);
                $cmdBar.hide();
            }
        }
    ); //end tree.select


    $('#tree1').bind(
        'tree.contextmenu',
        function (event) {
            var node = event.node;

        }
    ); //end tree.contextmenu

    $('#tree1').bind(
        'tree.move',
        function (event) {
            event.preventDefault();
            //if (promptOnMove)
            //{
            if (confirm('are you sure you want to move the node')) {
                //}


                // if did move it on the server
                if ($moveNode(event.move_info.moved_node, event.move_info.target_node, event.move_info.previous_parent, event.move_info.position)) {
                    event.move_info.do_move(); // this moves it in the ui


                } //end if $moveNode

                //if (promptOnMove)
                //{
            } //end if prompt
            //}
        }
    ); //end bind tree.move

    $('#tree1').bind(
        'tree.init',
        function () {

        }
    ); //end bind tree.init

    $('#tree1').bind(
        'tree.open',
        function (event) {

            $tree.tree('selectNode', null);
            $tree.tree('selectNode', event.node);

        }
    ); //end bind tree.open

    $('#tree1').bind(
        'tree.close',
        function (event) {
            $tree.tree('selectNode', null); //deselect
            $tree.tree('selectNode', event.node);

        }
    ); //end bind tree.close



}); // end self exe function
