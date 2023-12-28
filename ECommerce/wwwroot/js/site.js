
showInPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").modal('show')


        }
    })
}

showInPopupCrud = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#form-modal-crud .modal-body").html(res);
            $("#form-modal-crud .modal-title").html(title);
            $("#form-modal-crud").modal('show')


        }
    })
}


    var isSubmitting = false;

    $('#loginForm').on('submit', function (e) {
        e.preventDefault(); 

        if (isSubmitting) {
            return; 
        }

        $('#submitButton').prop('disabled', true);

        $('#loadingSpinner').show();

    $.ajax({
        url: '/Account/Login',
    type: 'POST',
    data: $('#loginForm').serialize(),
    success: function (data) {
        $('#loadingSpinner').hide();

        if (data.success) {

            $('#form-modal').hide();
            location.reload();
            //history.go(0);
        } else {
            $('#errorDiv').show();
            $('#loadingSpinner').hide();

        }


        setTimeout(function () {
            $('#submitButton').prop('disabled', false);
        }, 2000);

        isSubmitting = false;


    },
    error: function (error) {
        console.log(error);
        $('#loadingSpinner').hide();

        setTimeout(function () {
            $('#submitButton').prop('disabled', false);
        }, 2000); 
        isSubmitting = false;

            }
        });
    });


$('#loginForm input').on('input', function () {
    var emailField = $('#loginForm input[name="Email"]');
    var passwordField = $('#loginForm input[name="Password"]');

    var isEmailValid = isValidEmail(emailField.val());
    var isPasswordValid = isStrongPassword(passwordField.val());

    if (isEmailValid && isPasswordValid) {
        $('#submitButton').prop('disabled', false);
    } else {
        $('#submitButton').prop('disabled', true);
    }
});

function isValidEmail(email) {

    var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    return emailPattern.test(email);
}

function isStrongPassword(password) {

    return password.length >= 8 && /[a-zA-Z0-9]/.test(password);

}

    function notficationDoneRegister(){
        Swal.fire({
            title: 'تم تسجيلك بنجاح',
            text: '',
            icon: "success"

        })

        console.log('تم الاستدعاء');
}



document.getElementById('showImageInNewTab').addEventListener('click', function () {
    var imageSource = document.getElementById('imageToShow').src;
    window.open(imageSource, '_blank');
});


