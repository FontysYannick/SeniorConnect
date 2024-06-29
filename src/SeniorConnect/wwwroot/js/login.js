$(document).ready(() => {
    toggleShowPassword();
    const togglePassword = $('.js-toggle-password-icon');

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    var forms = $('.needs-validation');


    // Loop over them and prevent submission
    forms.each(function () {
        $(this).on('submit', function (event) {
            if (!this.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
                togglePassword.css({ "top": "22px", "left": "83%" });
            }

            var confirmPassword = $('#js-confirm-password');
            var password = $('#js-password-register');

            if (confirmPassword.length > 0) {
                if (password.val() !== confirmPassword.val()) {
                    event.preventDefault();
                    event.stopPropagation();
                    confirmPassword.get(0).setCustomValidity("Passwords do not match");
                    confirmPassword.addClass('is-invalid');
                    confirmPassword.parent().find('.invalid-feedback').text('Uw wachtwoord komt niet overeen met elkaar');
                } else {
                    confirmPassword.get(0).setCustomValidity("");
                    confirmPassword.removeClass('is-invalid');
                }
            }


            $(this).addClass('was-validated');
        });
    });
})


function toggleShowPassword() {
    let ICON_PASS_TOGGLE_HIDDE = 'bi-eye-slash';
    let ICON_PASS_TOGGLE_SHOW = 'bi-eye';

    const allTogglePasswordIcon = $('.js-toggle-password-icon');
    const allInputPassword = $('input[type="password"]');

    allInputPassword.attr('type', 'password');

    allTogglePasswordIcon.off().on('click', function (e) {
        const targetToggle = $(e.target);
        const passwordInput = targetToggle.parent('.js-toggle-password').find('input');

        let iconClass = targetToggle.attr('class');

        if (passwordInput.attr('type') === 'password') {
            iconClass = iconClass.replace(ICON_PASS_TOGGLE_HIDDE, ICON_PASS_TOGGLE_SHOW)
            passwordInput.attr('type', 'text');
            targetToggle.attr('class', iconClass);

            return;
        }

        iconClass = iconClass.replace(ICON_PASS_TOGGLE_SHOW, ICON_PASS_TOGGLE_HIDDE)

        targetToggle.attr('class', iconClass);
        passwordInput.attr('type', 'password');
    })

}