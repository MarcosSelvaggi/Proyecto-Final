document.addEventListener("DOMContentLoaded", function () {

    const password = document.getElementById("txtPassword");
    const btn = document.getElementById("BtnModificar");
    const error = document.getElementById("errorPassword");

    function validar() {
        if (!password) return false;

        const valido = /^(?=.*[A-Z])(?=.*\d).{8,}$/.test(password.value);

        const field = password.closest(".field");
        const check = document.getElementById("checkPassword");

        if (valido) {
            password.classList.add("input-ok");
            password.classList.remove("input-error");

            field.classList.add("valid");

            if (error) error.innerText = "";
        } else {
            password.classList.add("input-error");
            password.classList.remove("input-ok");

            field.classList.remove("valid");

            if (error) error.innerText = "Mínimo 8 caracteres, 1 mayúscula y 1 número";
        }

        return valido;
    }

    password?.addEventListener("input", validar);

    btn?.addEventListener("click", function (e) {
        if (!validar()) {
            e.preventDefault();
            return;
        }

        this.disabled = true;
        this.innerText = "Actualizando...";
    });

});