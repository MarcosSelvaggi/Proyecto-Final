document.addEventListener("DOMContentLoaded", function () {

    function setEstado(input, valido, mensaje, errorId) {
        const error = document.getElementById(errorId);
        const field = input?.closest(".form-group");

        if (!input) return;

        input.classList.toggle("input-error", !valido);
        input.classList.toggle("input-ok", valido);

        field?.classList.toggle("valid", valido);

        if (error) error.innerText = valido ? "" : mensaje;
    }

    const nombre = document.getElementById("txtNombre");
    const apellido = document.getElementById("txtApellido");
    const telefono = document.getElementById("txtTelefono");
    const email = document.getElementById("txtEmail");
    const password = document.getElementById("txtPassword");

    nombre?.addEventListener("input", () => {
        setEstado(nombre, nombre.value.trim() !== "", "Ingrese el nombre", "errorNombre");
    });

    apellido?.addEventListener("input", () => {
        setEstado(apellido, apellido.value.trim() !== "", "Ingrese el apellido", "errorApellido");
    });

    telefono?.addEventListener("input", () => {

        let valor = telefono.value.replace(/\D/g, "");

        valor = valor.slice(0, 10);

        telefono.value = valor;

        const valido = valor.length === 10;

        setEstado(
            telefono,
            valido,
            "Debe tener 10 números",
            "errorTelefono"
        );
    });

    telefono?.addEventListener("keypress", function (e) {
        if (!/[0-9]/.test(e.key)) {
            e.preventDefault();
        }
    });

    email?.addEventListener("input", () => {
        setEstado(
            email,
            /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value),
            "Email inválido",
            "errorEmail"
        );
    });

    password?.addEventListener("input", () => {
        const valido = /^(?=.*[A-Z])(?=.*\d).{8,}$/.test(password.value);
        setEstado(password, valido, "Mínimo 8 caracteres, 1 mayúscula y 1 número", "errorPassword");
    });

    document.querySelector(".btn-registro")?.addEventListener("click", function (e) {

        let valido = true;

        [nombre, apellido, telefono, email, password].forEach(input => {
            if (!input || input.value.trim() === "" || input.classList.contains("input-error")) {
                valido = false;
                input?.classList.add("input-error");
            }
        });

        if (!valido) {
            e.preventDefault();
            return;
        }

        this.innerText = "Creando cuenta...";
        setTimeout(() => {
            this.disabled = true;
        }, 0);
    });

    window.togglePass = function (id) {
        const input = document.getElementById(id);
        if (!input) return;

        input.type = input.type === "password" ? "text" : "password";
    };

});