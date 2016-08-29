(function ($) {

    // #region Helpers

    function ConvertMarkupToValidXhtml(markup) {
        var docImplementation = document.implementation;
        var htmlDocument = docImplementation.createHTMLDocument("temp");
        var xHtmlDocument = docImplementation.createDocument('http://www.w3.org/1999/xhtml', 'html', null);
        var xhtmlBody = xHtmlDocument.createElementNS('http://www.w3.org/1999/xhtml', 'body');

        htmlDocument.body.innerHTML = "<div>" + markup + "</div>";

        xHtmlDocument.documentElement.appendChild(xhtmlBody);
        xHtmlDocument.importNode(htmlDocument.body, true);
        xhtmlBody.appendChild(htmlDocument.body.firstChild);

        /<body.*?><div>([\s\S]*?)<\/div><\/body>/i.exec(xHtmlDocument.documentElement.innerHTML);
        return RegExp.$1;
    }

    // #endregion

    var contentId, editMode, currentSlug, supportsCategories, contentType,
        txtTitle, txtDateTime, txtExcerpt, txtContent, txtMessage, txtImage, txtPageOrder,
        txtParentPage, txtViewRoles, chkPublish,
        editorBar, btnNew, btnEdit, btnDelete, btnSave, btnCancel, btnOuterToggle,
        indexPath, categoryPath, savePath, deletePath, cancelEditPath

    editContent = function () {
        txtTitle.attr('contentEditable', true);
        txtExcerpt.attr('contentEditable', true);
        txtExcerpt.css({ minHeight: "100px" });
        txtExcerpt.parent().css('display', 'block');
        txtContent.wysiwyg({ hotKeys: {}, activeToolbarClass: "active" });
        txtContent.css({ minHeight: "400px" });
        if (editMode == "new") {
            // TODO: localize
            txtTitle.attr('placeholder', 'type your title here');
            txtExcerpt.attr('placeholder', 'type your meta description here');
            txtContent.attr('placeholder', 'type your main content here');
            //txtTitle.focus();
        } else {
            //txtContent.focus();
        }

        
        txtDateTime.datetimepicker({
            debug:false,
            widgetPositioning: { horizontal: 'left', vertical: 'bottom' },
            keepOpen: true,
            allowInputToggle:true
        });
        
        btnNew.attr("disabled", true);
        btnEdit.attr("disabled", true);
        btnSave.removeAttr("disabled");
        btnCancel.removeAttr("disabled");
        

        if (editMode == "new")
        {
            $("#liDelete").hide();
        }
        else
        {
            $("#liDelete").show();
        }
        
        $("#liSave").show();
        $("#liCancel").show();
        $("#liPublished").show();

        if (contentType == "Page") {
            $("#liPageOrder").show();
        }
        else
        {
            $("#liPageOrder").hide();
        }

        $('#liNewItem').hide();
        $('#liEdit').hide();

        //txtTitle.keyup(function () {
        //    if ($(this).val().length != 0)
        //        btnSave.removeAttr("disabled");
        //    else
        //        btnSave.attr('disabled', true);
        //})
        //if (txtTitle.is(':empty'))
        //{
        //    txtTitle.keyup(function () {
        //        btnSave.prop('disabled', this.value == "" ? true : false);
        //    })
        //}
        
        if (supportsCategories) { showCategoriesForEditing(); }

        toggleSourceView();

        $("#tools").fadeIn().css("display", "inline-block");
    },
    cancelEdit = function () {
        if (editMode.length > 0) {
            if (confirm("Do you want to leave this page?")) {
                history.back();
            }
        } else
        {  
            window.location = cancelEditPath;
        }
    },
    toggleSourceView = function () {
        $(".source").bind("click", function () {
            var self = $(this);
            if (self.attr("data-cmd") === "source") {
                self.attr("data-cmd", "design");
                self.addClass("active");
                txtContent.text(txtContent.html());
            } else {
                self.attr("data-cmd", "source");
                self.removeClass("active");
                txtContent.html(txtContent.text());
            }
        });
    },
    saveContent = function (e) {
        if ($(".source").attr("data-cmd") === "design") {
            $(".source").click();
        }

        txtContent.cleanHtml();

        var parsedDOM;

        /*  IE9 doesn't support text/html MimeType https://github.com/madskristensen/MiniBlog/issues/35
        
            parsedDOM = new DOMParser().parseFromString(txtContent.html(), 'text/html');
            parsedDOM = new XMLSerializer().serializeToString(parsedDOM);

            /<body>(.*)<\/body>/im.exec(parsedDOM);
            parsedDOM = RegExp.$1;
        
        */

        /* When its time to drop IE9 support toggle commented region with 
           the following statement and ConvertMarkupToXhtml function */
        parsedDOM = ConvertMarkupToValidXhtml(txtContent.html());

        //alert(txtDateTime.val());
        var pageSort = 0;
        var parentPage = "";
        var roles = "";
        if (contentType == "Page")
        {
            pageSort = txtPageOrder.val();
            parentPage = txtParentPage.val();
            roles = txtViewRoles.val();
        }

        $.post(savePath, {
            id: contentId,
            isPublished: chkPublish.prop("checked"),
            title: txtTitle.text().trim(),
            pubDate: txtDateTime.val(),
            pageOrder:pageSort,
            metaDescription: txtExcerpt.text().trim(),
            content: parsedDOM,
            categories: getCategories(),
            parentSlug: parentPage,
            viewRoles:roles,
            __RequestVerificationToken: document.querySelector("input[name=__RequestVerificationToken]").getAttribute("value")
        },null,"text")
          .success(function (data) {
              location.href = data;
              return;
          })
          .fail(function (data) {
              if ((data) && (data.status === 409)) {
                  showMessage(false, "The title is already in use");
              } else {
                  showMessage(false, "Something went wrong on the server ");
              }
          })
        
        ;
    },
    deleteContent = function () {

        if (confirm("Are you sure you want to delete this " + contentType + "?")) {
            $.post(deletePath,
                {
                    id: contentId,
                    __RequestVerificationToken: document.querySelector("input[name=__RequestVerificationToken]").getAttribute("value")
                })
                .success(function () { location.href = indexPath; })
                .fail(function () { showMessage(false, "Something went wrong. Please try again"); });
        }
    },
    showMessage = function (success, message) {
        var className = success ? "alert-success" : "alert-error";
        txtMessage.addClass(className);
        txtMessage.text(message);
        txtMessage.parent().fadeIn();

        setTimeout(function () {
            txtMessage.parent().fadeOut("slow", function () {
                txtMessage.removeClass(className);
            });
        }, 4000);
        alert(message);
    },
    getCategories = function () {
        var categories = '';

        if ($("#txtCategories").length > 0) {
            categories = $("#txtCategories").val();
        } else {
            $("ul.categories li a").each(function (index, item) {
                if (categories.length > 0) {
                    categories += ",";
                }
                categories += $(item).html();
            });
        }
        return categories;
    },
    showCategoriesForEditing = function () {
        var firstItemPassed = false;
        var categoriesString = getCategories();
        $("ul.categories li").each(function (index, item) {
            if (!firstItemPassed) {
                firstItemPassed = true;
            } else {
                $(item).remove();
            }
        });
        $("ul.categories").append("<li><input id='txtCategories' class='form-control' /></li>");
        $("#txtCategories").val(categoriesString);
        $("#txtCategories").attr('placeholder', 'type comma separated categories here');
    },
    showCategoriesForDisplay = function () {
        if ($("#txtCategories").length > 0) {
            var categoriesArray = $("#txtCategories").val().split(',');
            $("#txtCategories").parent().remove();

            $.each(categoriesArray, function (index, category) {
                $("ul.categories").append(' <li itemprop="articleSection" title="'
                    + category + '"> <a href="'
                    + categoryPath
                    + encodeURIComponent(category.toLowerCase()) + '">'
                    + category
                    + '</a> </li> ');
            });
        }
    },
    setToolbarCookie = function () {
        $.cookie('editor-toggle', 'hide', { path: '/' }); // cookie exists means hide toolbar
        //alert('cookie set');
    },
    clearToolbarCookie = function () {
        $.removeCookie('editor-toggle', { path: '/' }); // cookie exists means hide toolbar
        //alert('cookie removed');
    },
    toolBarCookieExists = function () {
        var toggleCookie = $.cookie('editor-toggle');
        if (toggleCookie) {
           //alert('cookie exists');
            return true;
        } // cookie exists means show
        //alert('no cookie');
        return false;
    },
    toggleToolbar = function () {
        var editorHidden = editorBar.hasClass("invisible");
        var isVisible = editorBar.is(':visible');
        if (!isVisible) {
            // editor was hidden, show it
            editorBar.show();
            btnOuterToggle.hide();
            if (!toolBarCookieExists()) { setToolbarCookie(); }  
        }
        else {
            //editor was visible toggle it to invisible
            // show the button to get the toolbar back
            editorBar.hide();
            btnOuterToggle.show();
            btnOuterToggle.animate({ "top": mainNavHeight, right: "0px" }, 500, "swing");
            clearToolbarCookie();
        }
    },
    setInitialToggleState = function () {
        if (toolBarCookieExists()) {
            // show the toolbar
            //alert('cookie exists');
            editorBar.show();

        }
        else { // no cookie so toolbar is hidden
            editorBar.hide();
            btnOuterToggle.animate({ "top": mainNavHeight, right: "0px" }, 500, "swing");  
        }
    },
    addToolBarPadding = function () {
        toolbarHeight = editorBar.height();
    },
    removeToolbarPadding = function () {
        //$(".body-content").first().animate({ "padding-top": mainNavHeight }, 500, "swing");
    }
    ;

    editorBar = $("#editor-toolbar");
    contentType = $("#editor-toolbar").data("content-type");

    if (contentType == "Page") {
        contentId = $("article").first().attr("data-id");
        txtTitle = $("#article-title");
        txtPageOrder = $("#txtPageOrder");
        txtParentPage = $("#txtParentPage");
        txtViewRoles = $("#txtViewRoles");
    }
    else
    {
        contentId = $("[itemprop~='blogPost']").attr("data-id");
        txtTitle = $("[itemprop~='blogPost'] [itemprop~='name']");
    }
    

    
    txtExcerpt = $("[itemprop~='description']");
    txtContent = $("[itemprop~='articleBody']");
    txtMessage = $("#editor-toolbar .alert");
    txtImage = $("#editor-toolbar #txtImage");
    txtDateTime = $("[itemprop~='datePublished']");

    btnNew = $("#btnNew");
    btnEdit = $("#btnEdit");
    btnDelete = $("#btnDelete").bind("click", deleteContent);
    btnSave = $("#btnSave").bind("click", saveContent);
    btnCancel = $("#btnCancel").bind("click", cancelEdit);
    btnOuterToggle = $('#editor-outer-toggle');
    chkPublish = $("#published");
    indexPath = $("#editor-toolbar").data("index-path");
    categoryPath = $("#editor-toolbar").data("category-path");
    savePath = $("#editor-toolbar").data("save-path");
    deletePath = $("#editor-toolbar").data("delete-path");
    cancelEditPath = $("#editor-toolbar").data("cancel-edit-path");
    currentSlug = $("#editor-toolbar").data("current-slug");
    supportsCategories = ($("#editor-toolbar").data("supports-categories")) == 'True';
    editMode = $("#editor-toolbar").data("edit-mode");
    
    //alert(contentId);
    var mainNavHeight = $(".navbar-fixed-top").first().height();
    var toolbarHeight = editorBar.height();
    var tbOriginalHeight = editorBar.height();
    
    // different no slug initial state for blog vs page
    if (contentType == "Post") {
        //alert('hey');
        if (currentSlug.length == 0) {
            $('#liEdit').hide();
        }
       
    } 
   
    $('#btnHide').click(function (event) {
        event.preventDefault();
        toggleToolbar();
    });
    btnOuterToggle.click(function (event) {
        event.preventDefault();
        toggleToolbar();
    });

    setInitialToggleState();
    
    
    if (currentSlug.length == 0) {
        
    }
    

    $(document).keyup(function (e) {
        if (!document.activeElement.isContentEditable) {
            if (e.keyCode === 46) { // Delete key
                deleteContent();
            } else if (e.keyCode === 27) { // ESC key
                cancelEdit();
            }
        }
    });

    $('.uploadimage').click(function (e) {
        e.preventDefault();
        $('#txtImage').click();
    });

    if (editMode == "new" || editMode == "edit") {
        editContent(); 
    }
    else if(contentType == "Page") {
        if (currentSlug) { btnEdit.removeAttr("disabled"); }
    }
     
    $(".dropdown-menu > input").click(function (e) {
        e.stopPropagation();
    });
})(jQuery);
