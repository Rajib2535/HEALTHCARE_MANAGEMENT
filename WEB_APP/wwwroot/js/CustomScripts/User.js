// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//showInPopup = (url, title) => {
//    debugger;
//    $('#form-modal .modal-body').html('');
//    debugger;
//    $.ajax({
//        type: 'GET',
//        url: url,
//        success: function (res) {
//            $('#form-modal .modal-body').html(res);
//            $('#form-modal .modal-title').html(title);
//            $('#form-modal').modal({
//                show: true,
//                backdrop: true,
//            });
//            //activateSummerNote();
//        }
//    })
//}
function showInPopup(url, title) {
    const modalElement = document.getElementById('form-modal');
    const modalTitle = modalElement.querySelector('#modalTitle');
    const modalBody = modalElement.querySelector('.modal-body');

    modalTitle.textContent = title; // Set modal title
    modalBody.innerHTML = ''; // Clear previous content

    // Fetch the form via AJAX
    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to load the form.');
            }
            return response.text();
        })
        .then(html => {
            modalBody.innerHTML = html;

            // Initialize and show the modal
            const modal = new bootstrap.Modal(modalElement);
            modal.show();
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Could not load the form. Please try again.');
        });
}


jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.is_valid) {
                    toastr.success('Information updated successfully');
                    $('#view-all').html(res.html);
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                    activatejQueryTable();
                    //activateSummerNote();
                }
                else {
                    if (res.session_expired) {
                        debugger;
                        window.location.reload();
                    }
                    else {
                        toastr.error('Could not update information');
                        $('#form-modal .modal-body').html(res.html);
                    }
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
jQueryAjaxDelete = form => {
    debugger;
    if (confirm('Are you sure you want to deactivate this user?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    if (res.is_valid) {
                        toastr.success('Information updated successfully');
                        $('#view-all').html(res.html);
                        $('#form-modal .modal-body').html('');
                        $('#form-modal .modal-title').html('');
                        $('#form-modal').modal('hide');
                        activatejQueryTable();
                        //activateSummerNote();
                    }
                    else {
                        if (res.session_expired) {
                            debugger;
                            window.location.href = res.redirect_url;
                        }
                        else {
                            toastr.error('Could not update information');
                            $('#form-modal .modal-body').html(res.html);
                        }
                    }
                },
                error: function (err) {
                    errorResponse(err);
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;
}
jQueryAjaxResetPassword = form => {
    debugger;
    if (confirm('Are you sure you want to reset password of this user?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    debugger;
                    if (res.is_valid) {
                        toastr.success('Password Reset successfully done!');
                        $('#view-all').html(res.html);
                        $('#form-modal .modal-body').html('');
                        $('#form-modal .modal-title').html('');
                        $('#form-modal').modal('hide');
                        activatejQueryTable();
                        //activateSummerNote();
                    }
                    else {
                        if (res.session_expired) {
                            debugger;
                            window.location.href = res.redirect_url;
                        }
                        else {
                            toastr.error('Could not update information');
                            $('#form-modal .modal-body').html(res.html);
                        }
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;
}



