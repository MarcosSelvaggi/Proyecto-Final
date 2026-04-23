async function enviarContacto() {
    var nombre = document.getElementById("inputNombre").value.trim();
    var asunto = document.getElementById("inputAsunto").value.trim();
    var mensaje = document.getElementById("inputMensaje").value.trim();
    var lblError = document.getElementById("lblErrorContacto");

    // Validaciones
    if (!nombre || !asunto || !mensaje) {
        lblError.innerText = "Por favor completá todos los campos.";
        return;
    }
    if (mensaje.length < 10) {
        lblError.innerText = "El mensaje es demasiado corto.";
        return;
    }
    lblError.innerText = "";

    var btn = document.getElementById("btnEnviarContacto");
    btn.disabled = true;
    btn.innerText = "Enviando...";

    try {
        var response = await fetch("https://formspree.io/f/xgorrzad", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                nombre: nombre,
                asunto: asunto,
                mensaje: mensaje
            })
        });

        if (response.ok) {
            // Limpiar campos
            document.getElementById("inputNombre").value = "";
            document.getElementById("inputAsunto").value = "";
            document.getElementById("inputMensaje").value = "";

            // Mostrar modal
            var m = new bootstrap.Modal(document.getElementById("modalEnviado"));
            m.show();
        } else {
            lblError.innerText = "Hubo un error al enviar el mensaje. Intentá de nuevo.";
        }
    } catch (e) {
        lblError.innerText = "No se pudo conectar. Verificá tu conexión e intentá de nuevo.";
    }

    btn.disabled = false;
    btn.innerText = "Enviar mensaje";
}