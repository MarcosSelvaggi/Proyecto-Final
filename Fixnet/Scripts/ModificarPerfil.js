function previewImagen(input) {
    var preview = document.getElementById("previewFoto");
    var lblIniciales = document.querySelector("[id$='lblIniciales']");
    var imgActual = document.querySelector("[id$='imgFotoActual']");

    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
            preview.style.display = "inline-block";
            if (lblIniciales) lblIniciales.style.display = "none";
            if (imgActual) imgActual.style.display = "none";
        };
        reader.readAsDataURL(input.files[0]);
    }
}
document.addEventListener("DOMContentLoaded", function () {

    const nombre = document.getElementById("txtNombre");
    const apellido = document.getElementById("txtApellido");
    const telefono = document.getElementById("txtTelefono");
    const email = document.getElementById("txtEmail");
    const btn = document.getElementById("BtnModificar");

    telefono.addEventListener("input", function () {

        let valor = this.value.replace(/\D/g, "");

        this.value = valor.substring(0, 10);

    });
    function validarTexto(input, errorId) {
        const error = document.getElementById(errorId);
        const field = input.closest(".field");

        const valido = input.value.trim().length > 2;

        input.classList.toggle("input-ok", valido);
        input.classList.toggle("input-error", !valido);
        field?.classList.toggle("valid", valido);

        if (error) error.innerText = valido ? "" : "Campo obligatorio";

        return valido;
    }

    function validarTelefono() {
        const field = telefono.closest(".field");
        const error = document.getElementById("errorTelefono");

        const valido = /^[0-9]{10}$/.test(telefono.value);

        telefono.classList.toggle("input-ok", valido);
        telefono.classList.toggle("input-error", !valido);
        field?.classList.toggle("valid", valido);

        if (error) error.innerText = valido ? "" : "10 números obligatorios";

        return valido;
    }

    function validarEmail() {
        const field = email.closest(".field");
        const error = document.getElementById("errorEmail");

        const valido = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value);

        email.classList.toggle("input-ok", valido);
        email.classList.toggle("input-error", !valido);
        field?.classList.toggle("valid", valido);

        if (error) error.innerText = valido ? "" : "Email inválido";

        return valido;
    }

    nombre?.addEventListener("input", () => validarTexto(nombre, "errorNombre"));
    apellido?.addEventListener("input", () => validarTexto(apellido, "errorApellido"));
    telefono?.addEventListener("input", validarTelefono);
    email?.addEventListener("input", validarEmail);

    document.addEventListener("DOMContentLoaded", function () {

        const form = document.forms[0];

        form.addEventListener("submit", function (e) {

            const ok =
                validarTexto(nombre, "errorNombre") &&
                validarTexto(apellido, "errorApellido") &&
                validarTelefono() &&
                validarEmail();

            if (!ok) {
                e.preventDefault();
                return;
            }

            btn.disabled = true;
            btn.value = "Actualizando...";
        });

    });

});