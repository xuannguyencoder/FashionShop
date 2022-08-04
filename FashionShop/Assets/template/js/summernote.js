jQuery(document).ready(function () {
    $(".summernote").summernote({
        height: 250,
        minHeight: null,
        maxHeight: null,
        focus: !1,
        callbacks: {
            onImageUpload: function (files) {
                for (var i = 0; i < files.length; i++) {
                    uploadImage(files[i]);
                }
            }
        }
    }), $(".inline-editor").summernote({
        airMode: !0
    })
}), window.edit = function () {
    $(".click2edit").summernote()
}, window.save = function () {
    $(".click2edit").summernote("destroy")
    };
function uploadImage(file) {
    var formData = new FormData();
    formData.append("uploadFiles", file);
    $.ajax({
        data: formData,
        type: "POST",
        url: '/Admin/Product/UploadFile',
        cache: false,
        contentType: false,
        processData: false,
        success: function (FileUrl) {
            var imgNode = document.createElement('img');
            imgNode.src = FileUrl;
            $('.summernote').summernote('insertNode', imgNode);
        },
        error: function (data) {
            alert(data.responseText);
        }
    });
}
jQuery(document).ready(function () {
    $(".summernote2").summernote({
        height: 250,
        minHeight: null,
        maxHeight: null,
        focus: !1,
        callbacks: {
            onImageUpload: function (files) {
                for (var i = 0; i < files.length; i++) {
                    uploadImage(files[i]);
                }
            }
        }
    }), $(".inline-editor").summernote({
        airMode: !0
    })
}), window.edit = function () {
    $(".click2edit").summernote()
}, window.save = function () {
    $(".click2edit").summernote("destroy")
    };

function uploadImage(file) {
    var formData = new FormData();
    formData.append("uploadFiles", file);
    $.ajax({
        data: formData,
        type: "POST",
        url: '/Admin/Product/UploadFile',
        cache: false,
        contentType: false,
        processData: false,
        success: function (FileUrl) {
            var imgNode = document.createElement('img');
            imgNode.src = FileUrl;
            $('.summernote2').summernote('insertNode', imgNode);
        },
        error: function (data) {
            alert(data.responseText);
        }
    });
}