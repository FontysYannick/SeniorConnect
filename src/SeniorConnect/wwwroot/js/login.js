$(document).ready(() => {

    toggleShowPassword();
    const togglePassword = $('.js-toggle-password');

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    var forms = $('.needs-validation');


    // Loop over them and prevent submission
    forms.each(function () {
        $(this).on('submit', function (event) {
            if (!this.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
                togglePassword.css({ "top": "17px", "left": "89%" });
            }

            $(this).addClass('was-validated');
        });
    });
 })
   

function toggleShowPassword() {
    let ICON_PASS_TOGGLE_HIDDE = 'bi-eye-slash';
    let ICON_PASS_TOGGLE_SHOW = 'bi-eye';

    const togglePassword = $('.js-toggle-password');
    const passwordInput = $('#password');

    passwordInput.attr('type', 'password');

    togglePassword.on('click', () => {
        let iconClass = togglePassword.attr('class');

        if (passwordInput.attr('type') === 'password') {
            iconClass = iconClass.replace(ICON_PASS_TOGGLE_HIDDE, ICON_PASS_TOGGLE_SHOW)
            passwordInput.attr('type', 'text');
            togglePassword.attr('class', iconClass);

            return;
        }

        iconClass = iconClass.replace(ICON_PASS_TOGGLE_SHOW, ICON_PASS_TOGGLE_HIDDE)
        togglePassword.attr('class', iconClass);
        passwordInput.attr('type', 'password');
    })

}