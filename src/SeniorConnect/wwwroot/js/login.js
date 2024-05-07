$(document).ready(() => {
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
})