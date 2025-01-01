// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

showInPopup = (url, title) => {
    var message = "";
    $('#form-modal .modal-body').html('');
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            debugger;
            if (res.is_valid) {
                $('#form-modal .modal-body').html(res.html);
                $('#form-modal .modal-title').html(title);
                $('#form-modal').modal({
                    show: true,
                    backdrop: true,
                });
            }
            else {
                res.error_messages.forEach(generateMessage);
                Swal.fire("Error!", message, "info");
            }
            //activateSummerNote();
        }
    });
    function generateMessage(item, index) {
        message += (index + 1) + ": " + item + "\n";
    }
}

jQueryAjaxPost = form => {
    try {
        var message = "";
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.is_valid) {
                    toastr.success('Information updated successfully');
                    //$('#refund-request-view-all').html(res.html);
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                    $('#btnSearch').click();
                    //activatejQueryTable();
                    //activateSummerNote();
                }
                else {
                    if (res.session_expired) {
                        debugger;
                        window.location.href = res.redirect_url;
                    }
                    else {
                        res.error_messages.forEach(generateMessage);
                        Swal.fire("Action incomplete!", message, "info");
                    }
                }
                function generateMessage(item, index) {
                    message += (index + 1) + ": " + item + "\n";
                }                    
            },
            error: function (err) {
                errorResponse(err);
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}





