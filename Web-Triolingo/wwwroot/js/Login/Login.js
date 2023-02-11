(function () {
    $("#formLogin").validate({
        rules: {
            Email: {
                required: true,
            },
            Password: {
                required: true,
            }
        },
        errorClass: "text-danger",
        messages: {
            Email: {
                required: "This field is required.",
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
})()