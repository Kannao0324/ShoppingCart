// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let theme = localStorage.getItem("Theme");

if (theme === "Secondary") {
    document.getElementById("themeStyle").setAttribute("href", "/css/secondary-theme.css")
}
else {
    document.getElementById("themeStyle").setAttribute("href", "/css/primary-theme.css")
}

window.addEventListener('load', () => {

    document.getElementById('btnTheme').addEventListener('click', () => {
        theme = localStorage.getItem("Theme");

        if (theme === "Secondary") {
            localStorage.setItem("Theme", "Primary");
            document.getElementById("themeStyle").setAttribute("href", "/css/primary-theme.css")
        }
        else {
            localStorage.setItem("Theme", "Secondary");
            document.getElementById("themeStyle").setAttribute("href", "/css/secondary-theme.css")
        }
    })
});

// Password Login
const togglePassword = document.querySelector("#togglePassword");
const password = document.querySelector("#password");

togglePassword.addEventListener("click", function () {
    // toggle the type attribute
    const type = password.getAttribute("type") === "password" ? "text" : "password";
    password.setAttribute("type", type);

    // toggle the icon
    this.classList.toggle("bi-eye");
});

// Password create user 1
const togglePassword2 = document.querySelector("#togglePasswordCreate");
const password2 = document.querySelector("#password");

togglePassword2.addEventListener("click", function () {
    // toggle the type attribute
    const type = password2.getAttribute("type") === "password" ? "text" : "password";
    password2.setAttribute("type", type);

    // toggle the icon
    this.classList.toggle("bi-eye");
});

// Password create user 2
const togglePassword3 = document.querySelector("#togglePasswordConfirm");
const password3 = document.querySelector("#password");

togglePassword3.addEventListener("click", function () {
    // toggle the type attribute
    const type = password3.getAttribute("type") === "password" ? "text" : "password";
    password3.setAttribute("type", type);

    // toggle the icon
    this.classList.toggle("bi-eye");
});
