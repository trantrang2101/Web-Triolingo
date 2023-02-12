(function () {
    $('#formRegis').hide()
    $('#changeLogin').hide()
    $('#changeRegis').on('click', function () {
        $('#changeRegis').hide()
        $('#changeLogin').show()
        $('#formLogin').hide()
        $('#formRegis').show()
    })
    $('#changeLogin').on('click', function () {
        $('#changeRegis').show()
        $('#changeLogin').hide()
        $('#formLogin').show()
        $('#formRegis').hide()
    })
    $("#formLogin").validate({
        rules: {
            Email: {
                required: true,
                email: true
            },
            Password: {
                required: true,
            }
        },
        errorClass: "text-danger",
        messages: {
            Email: {
                required: "This field is required.",
                email: "Please enter a valid email address"
            },
            Password: {
                required: "This field is required.",
            }
        },
        errorPlacement: function (error, element) {
            var $formGroup = element.closest(".form-group");
            if (!$formGroup.find("label.text-danger:visible").length) {
                error.appendTo($formGroup);
            }
        }
    })
    $("#formRegis").validate({
        rules: {
            Age: {
                number: true
            },
            Email: {
                required: true,
                email: true
            },
            Password: {
                required: true,
            }
        },
        errorClass: "text-danger",
        messages: {
            Age: {
                number: "Please enter your age."
            },
            Email: {
                required: "This field is required.",
                email: "Please enter a valid email address"
            },
            Password: {
                required: "This field is required.",
            }
        },
        errorPlacement: function (error, element) {
            var $formGroup = element.closest(".form-group");
            if (!$formGroup.find("label.text-danger:visible").length) {
                error.appendTo($formGroup);
            }
        }
    })
    $('#loginbtn').on('click', function () {
        if ($('#formLogin').valid()) {
            $('#formLogin').submit()
        }
    })
    $('#regisbtn').on('click', function () {
        if ($('#formRegis').valid()) {
            $('#formRegis').submit()
        }
    })
})()